namespace BookHub.Tests.Workflows;

using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.WebUtilities;
using Shared.Utils;

using static BookHubWebApplicationFactory;

// An account's life through the Identity endpoints, with real email: register and receive the
// welcome email, log in, forget the password, follow the link in the reset email that Mailpit
// received, set a new password, and log in again. (Registration with a failing email is in
// IdentityIntegration.)
public sealed class IdentityWorkflowIntegration : IAsyncLifetime
{
    private const string Username = "forgetful";
    private const string NewPassword = "N3wPassw0rd!";

    private readonly BookHubWebApplicationFactory factory = new();

    private string email = null!;

    public async ValueTask InitializeAsync()
    {
        await this.factory.ResetDatabase();

        this.email = this.factory.Email(Username);
    }

    public ValueTask DisposeAsync()
    {
        this.factory.Dispose();
        return ValueTask.CompletedTask;
    }

    [Fact]
    public async Task Register_Login_ForgotPassword_ResetPassword_ShouldOnlyAcceptTheNewPassword()
    {
        var anonymous = this.factory.CreateClient();

        // 1. Register: the welcome email arrives, with a link to the client.
        var registered = await (await anonymous.PostAsync(
                "/Identity/register/",
                RegisterForm(Username, this.email),
                TestContext.Current.CancellationToken))
            .ShouldHaveBody<TokenResponse>();

        registered.Token.Should().NotBeNullOrWhiteSpace();

        var welcome = await this.factory.Mailbox.WaitForMessage(this.email, "Welcome to BookHub");

        welcome.From.Address.Should().Be("noreply@bookhub.test");
        welcome.Html.Should().Contain($"Hi {Username},");
        welcome.Links.Should().Contain("http://localhost:5173");

        // 2. Log in with the password: the token works on a protected endpoint.
        var firstLogin = await (await this.Login(DefaultPassword))
            .ShouldHaveBody<TokenResponse>();

        await (await this.factory.CreateBearerClient(firstLogin.Token).GetAsync("/Profile/mine/", TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.OK);

        // 3. Forgot password: a generic answer, and the reset link arrives by email.
        var forgot = await (await anonymous.PostAsJsonAsync(
                "/Identity/forgot-password/",
                new { email = this.email },
                TestContext.Current.CancellationToken))
            .ShouldHaveBody<MessageResponse>();

        forgot.Message.Should().Be("If an account exists for that email, a password reset link has been sent.");

        var resetEmail = await this.factory.Mailbox.WaitForMessage(this.email, "Reset your BookHub password");

        var resetLink = resetEmail
            .Links
            .Should()
            .ContainSingle(l => l.StartsWith("http://localhost:5173/identity/reset-password?", StringComparison.Ordinal))
            .Subject;

        var query = QueryHelpers.ParseQuery(new Uri(resetLink).Query);
        query["email"].ToString().Should().Be(this.email);

        var token = query["token"].ToString();
        token.Should().NotBeNullOrWhiteSpace();

        // 4. Reset the password with the token from the link, as the client's reset page does.
        var reset = await (await anonymous.PostAsJsonAsync(
                "/Identity/reset-password/",
                new { email = query["email"].ToString(), token, newPassword = NewPassword },
                TestContext.Current.CancellationToken))
            .ShouldHaveBody<MessageResponse>();

        reset.Message.Should().Be("Password successfully reset.");

        // 5. Only the new password works now.
        await (await this.Login(NewPassword)).ShouldHaveStatus(HttpStatusCode.OK);

        await (await this.Login(DefaultPassword)).ShouldBeProblem(
            HttpStatusCode.BadRequest,
            "Invalid log in attempt!");

        // 6. The reset token is single-use: the password change invalidated it.
        var reused = await anonymous.PostAsJsonAsync(
            "/Identity/reset-password/",
            new { email = this.email, token, newPassword = "An0therPassw0rd" },
            TestContext.Current.CancellationToken);

        await reused.ShouldBeProblem(HttpStatusCode.BadRequest);

        await (await this.Login(NewPassword)).ShouldHaveStatus(HttpStatusCode.OK);
    }

    private Task<HttpResponseMessage> Login(string password)
        => this.factory.CreateClient().PostAsJsonAsync(
            "/Identity/login/",
            new { credentials = Username, password, rememberMe = false },
            TestContext.Current.CancellationToken);

    private sealed record MessageResponse(string Message);
}
