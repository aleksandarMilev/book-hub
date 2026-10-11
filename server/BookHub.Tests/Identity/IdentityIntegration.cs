namespace BookHub.Tests.Identity;

using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Shared.Email;
using Shared.Seed;
using Shared.Utils;

using static BookHubWebApplicationFactory;

public sealed class IdentityIntegration : IAsyncLifetime
{
    private const string WelcomeSubject = "Welcome to BookHub";
    private const string ResetSubject = "Reset your BookHub password";
    private const string ForgotPasswordAnswer = "If an account exists for that email, a password reset link has been sent.";

    private readonly BookHubWebApplicationFactory httpClientFactory = new();

    public async ValueTask InitializeAsync()
        => await this.httpClientFactory.ResetDatabase();

    public ValueTask DisposeAsync()
    {
        this.httpClientFactory.Dispose();
        return ValueTask.CompletedTask;
    }

    [Fact]
    public async Task Register_ShouldSucceed_AndTheWelcomeSenderShouldKeepGoing_WhenAWelcomeEmailIsRejected()
    {
        // A real failure, no fake sender: Mailpit refuses this domain with an SMTP 550, so MailKit
        // throws in the background sender after the registration has already succeeded (B-02).
        var undeliverable = $"newreader@{MailpitServer.UndeliverableDomain}";
        var httpClient = this.httpClientFactory.CreateClient();

        var registered = await (await httpClient.PostAsync(
                "/Identity/register/",
                RegisterForm("newreader", undeliverable),
                TestContext.Current.CancellationToken))
            .ShouldHaveBody<TokenResponse>();

        registered.Token.Should().NotBeNullOrWhiteSpace();

        // The welcome emails go out one at a time, in order, so the next user's email arriving
        // proves the rejected one was attempted and the sender survived its failure.
        var next = this.httpClientFactory.Email("nextreader");

        await (await httpClient.PostAsync("/Identity/register/", RegisterForm("nextreader", next), TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.OK);

        await this.httpClientFactory.Mailbox.WaitForMessage(next, WelcomeSubject);

        (await this.httpClientFactory.Mailbox.Messages(undeliverable)).Should().BeEmpty();

        // The account whose email failed is kept, with its profile, and can log in.
        var userIsActive = await this.httpClientFactory.WithData(data => data
            .Users
            .AnyAsync(u => u.UserName == "newreader" && u.Profile != null, TestContext.Current.CancellationToken));

        userIsActive.Should().BeTrue();

        await (await this.Login("newreader", DefaultPassword)).ShouldHaveStatus(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Register_ShouldReturnConflictProblem_WhenTheUsernameIsTaken()
    {
        await this.httpClientFactory.WithData(data => data.SeedUser("existing-id", "takenname"));

        var httpClient = this.httpClientFactory.CreateClient();

        var response = await httpClient.PostAsync("/Identity/register/", RegisterForm("takenname", "someone-new@test.local"), TestContext.Current.CancellationToken);

        await response.ShouldBeProblem(
            HttpStatusCode.Conflict,
            "Username 'takenname' is already taken.");
    }

    [Fact]
    public async Task Login_ShouldReturnBadRequestProblem_WhenTheCredentialsAreWrong()
    {
        await this.httpClientFactory.WithData(data => data.SeedUser("existing-id", "reader"));

        var response = await this.Login("reader", "WrongPassw0rd");

        await response.ShouldBeProblem(
            HttpStatusCode.BadRequest,
            "Invalid log in attempt!");
    }

    [Fact]
    public async Task Login_ShouldLockTheAccount_AfterThreeWrongPasswords_EvenForTheRightPassword()
    {
        await this.Register("guessed");

        await (await this.Login("guessed", "WrongPassw0rd1")).ShouldBeProblem(HttpStatusCode.BadRequest, "Invalid log in attempt!");
        await (await this.Login("guessed", "WrongPassw0rd2")).ShouldBeProblem(HttpStatusCode.BadRequest, "Invalid log in attempt!");

        // The third failure (MaxFailedLoginAttempts) locks the account.
        await (await this.Login("guessed", "WrongPassw0rd3")).ShouldBeProblem(
            HttpStatusCode.BadRequest,
            "Account locked due to multiple failed attempts.");

        await (await this.Login("guessed", DefaultPassword)).ShouldBeProblem(
            HttpStatusCode.BadRequest,
            "Account is locked. Try again later.");

        var lockoutEnd = await this.httpClientFactory.WithData(data => data
            .Users
            .Where(u => u.UserName == "guessed")
            .Select(u => u.LockoutEnd)
            .SingleAsync(TestContext.Current.CancellationToken));

        lockoutEnd.Should().BeCloseTo(DateTimeOffset.UtcNow.AddMinutes(15), TimeSpan.FromMinutes(1));
    }

    [Fact]
    public async Task ForgotPassword_ShouldGiveTheSameAnswer_AndSendNothing_WhenNoAccountHasTheEmail()
    {
        var unknown = this.httpClientFactory.Email("nobody");

        var answer = await (await this.ForgotPassword(unknown)).ShouldHaveBody<MessageResponse>();

        answer.Message.Should().Be(ForgotPasswordAnswer);

        // Sent after the request above, so once it arrives, a reset email to `unknown` would have too.
        var known = this.httpClientFactory.Email("known");
        await this.Register("known", known);
        await this.ForgotPassword(known);
        await this.httpClientFactory.Mailbox.WaitForMessage(known, ResetSubject);

        (await this.httpClientFactory.Mailbox.Messages(unknown)).Should().BeEmpty();
    }

    [Fact]
    public async Task ForgotPassword_ShouldGiveTheSameAnswer_WhenTheResetEmailCannotBeSent()
    {
        // The SMTP server rejects the address (a real 550), so the send throws inside the service.
        var undeliverable = $"stranded@{MailpitServer.UndeliverableDomain}";
        await this.Register("stranded", undeliverable);

        var answer = await (await this.ForgotPassword(undeliverable)).ShouldHaveBody<MessageResponse>();

        answer.Message.Should().Be(ForgotPasswordAnswer);
    }

    [Fact]
    public async Task ResetPassword_ShouldReturnBadRequestProblem_AndKeepThePassword_WhenTheTokenIsMalformed()
    {
        var email = this.httpClientFactory.Email("resetter");
        await this.Register("resetter", email);

        // Not Base64Url, so it can't even be decoded.
        var response = await this.ResetPassword(email, "%%%not-a-token%%%");

        await response.ShouldBeProblem(HttpStatusCode.BadRequest, "Invalid password reset attempt!");

        await (await this.Login("resetter", DefaultPassword)).ShouldHaveStatus(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ResetPassword_ShouldReturnBadRequestProblem_WhenNoAccountHasTheEmail()
    {
        var response = await this.ResetPassword(this.httpClientFactory.Email("nobody"), "c29tZS10b2tlbg");

        await response.ShouldBeProblem(HttpStatusCode.BadRequest, "Invalid password reset attempt!");
    }

    [Fact]
    public async Task Register_ShouldLeaveNoUser_AndAllowTheSameUsernameAndEmail_WhenProfileCreationFails()
    {
        // B-02: the user and the profile are created in one transaction, so a failed profile
        // creation leaves nothing behind (before, the user was soft-deleted and its username
        // and email stayed taken for good). The failure is a real one: a file sits where the
        // profiles image folder must be created, so the real ImageWriter can't save the avatar.
        var blocker = Path.Combine(this.httpClientFactory.WebRoot, "images", "profiles");
        Directory.CreateDirectory(Path.GetDirectoryName(blocker)!);
        await File.WriteAllTextAsync(blocker, "not a folder", TestContext.Current.CancellationToken);

        var httpClient = this.httpClientFactory.CreateClient();

        var failed = await httpClient.PostAsync(
            "/Identity/register/",
            RegisterForm("unlucky", "unlucky@test.local").WithImage("avatar.jpg"),
            TestContext.Current.CancellationToken);

        await failed.ShouldBeProblem(
            HttpStatusCode.BadRequest,
            "Invalid register attempt!");

        var userRows = await this.httpClientFactory.WithData(data => data
            .Users
            .IgnoreQueryFilters()
            .CountAsync(u => u.NormalizedUserName == "UNLUCKY" || u.NormalizedEmail == "UNLUCKY@TEST.LOCAL"));

        userRows.Should().Be(0);

        File.Delete(blocker);

        var retried = await httpClient.PostAsync(
            "/Identity/register/",
            RegisterForm("unlucky", "unlucky@test.local").WithImage("avatar.jpg"),
            TestContext.Current.CancellationToken);

        retried.StatusCode.Should().Be(HttpStatusCode.OK);

        await (await this.Login("unlucky", DefaultPassword)).ShouldHaveStatus(HttpStatusCode.OK);

        var avatar = await this.httpClientFactory.WithData(data => data
            .Profiles
            .Where(p => p.User.UserName == "unlucky")
            .Select(p => p.ImagePath)
            .SingleAsync());

        this.httpClientFactory.ImageFiles("profiles").Should().Equal(avatar);
    }

    private async Task Register(
        string username,
        string? email = null)
        => await (await this.httpClientFactory.CreateClient().PostAsync(
                "/Identity/register/",
                RegisterForm(username, email ?? this.httpClientFactory.Email(username)),
                TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.OK);

    private Task<HttpResponseMessage> Login(
        string credentials,
        string password)
        => this.httpClientFactory.CreateClient().PostAsJsonAsync(
            "/Identity/login/",
            new { credentials, password, rememberMe = false },
            TestContext.Current.CancellationToken);

    private Task<HttpResponseMessage> ForgotPassword(string email)
        => this.httpClientFactory.CreateClient().PostAsJsonAsync(
            "/Identity/forgot-password/",
            new { email },
            TestContext.Current.CancellationToken);

    private Task<HttpResponseMessage> ResetPassword(
        string email,
        string token)
        => this.httpClientFactory.CreateClient().PostAsJsonAsync(
            "/Identity/reset-password/",
            new { email, token, newPassword = "N3wPassw0rd!" },
            TestContext.Current.CancellationToken);

    private sealed record MessageResponse(string Message);
}
