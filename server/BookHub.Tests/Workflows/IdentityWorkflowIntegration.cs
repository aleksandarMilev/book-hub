namespace BookHub.Tests.Workflows;

using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Shared.Identity;
using Shared.Utils;

// An account's life through the Identity endpoints, with the real JWT pipeline: register while
// SMTP is down, log in, forget the password, reset it from the emailed link, log in again.
public sealed class IdentityWorkflowIntegration : IAsyncLifetime
{
    private const string Username = "forgetful";
    private const string Email = "forgetful@test.local";
    private const string NewPassword = "N3wPassw0rd!";

    private readonly RealJwtFactory factory = new();

    public async ValueTask InitializeAsync()
    {
        await this.factory.ResetDatabase();

        this.factory.EmailSender.FailWelcomeEmails = true;
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

        // 1. Register while the welcome email fails: the account is kept (B-02).
        var registered = await (await anonymous.PostAsync(
                "/Identity/register/",
                RealJwtFactory.RegisterForm(Username, Email),
                TestContext.Current.CancellationToken))
            .ShouldHaveBody<RealJwtFactory.TokenResponse>();

        registered.Token.Should().NotBeNullOrWhiteSpace();

        var attemptedEmail = await this
            .factory
            .EmailSender
            .WelcomeAttempted
            .WaitAsync(TimeSpan.FromSeconds(10), TestContext.Current.CancellationToken);

        attemptedEmail.Should().Be(Email);
        this.factory.EmailSender.WelcomeEmails.Should().BeEmpty();

        var userIsActive = await this.factory.WithData(data => data
            .Users
            .AnyAsync(u => u.UserName == Username, TestContext.Current.CancellationToken));

        userIsActive.Should().BeTrue();

        // 2. Log in with the password: the token works on a protected endpoint.
        var firstLogin = await (await this.Login(RealJwtFactory.DefaultPassword))
            .ShouldHaveBody<RealJwtFactory.TokenResponse>();

        await (await this.factory.CreateBearerClient(firstLogin.Token).GetAsync("/Profile/mine/", TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.OK);

        // 3. Forgot password: a generic answer, and the reset link is emailed.
        var forgot = await (await anonymous.PostAsJsonAsync(
                "/Identity/forgot-password/",
                new { email = Email },
                TestContext.Current.CancellationToken))
            .ShouldHaveBody<MessageResponse>();

        forgot.Message.Should().Be("If an account exists for that email, a password reset link has been sent.");

        var resetEmail = this.factory.EmailSender.PasswordResets.Should().ContainSingle().Subject;
        resetEmail.To.Should().Be(Email);
        resetEmail.Url.Should().StartWith("http://localhost:5173/identity/reset-password?");

        var query = QueryHelpers.ParseQuery(new Uri(resetEmail.Url).Query);
        query["email"].ToString().Should().Be(Email);

        var token = query["token"].ToString();
        token.Should().NotBeNullOrWhiteSpace();

        // 4. Reset the password with the emailed token.
        var reset = await (await anonymous.PostAsJsonAsync(
                "/Identity/reset-password/",
                new { email = Email, token, newPassword = NewPassword },
                TestContext.Current.CancellationToken))
            .ShouldHaveBody<MessageResponse>();

        reset.Message.Should().Be("Password successfully reset.");

        // 5. Only the new password works now.
        await (await this.Login(NewPassword)).ShouldHaveStatus(HttpStatusCode.OK);

        await (await this.Login(RealJwtFactory.DefaultPassword)).ShouldBeProblem(
            HttpStatusCode.BadRequest,
            "Invalid log in attempt!");

        // 6. The reset token is single-use: the password change invalidated it.
        var reused = await anonymous.PostAsJsonAsync(
            "/Identity/reset-password/",
            new { email = Email, token, newPassword = "An0therPassw0rd" },
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
