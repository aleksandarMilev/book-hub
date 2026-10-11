namespace BookHub.Tests.Identity;

using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Identity;
using Shared.Seed;
using Shared.Utils;

public sealed class IdentityIntegration : IAsyncLifetime
{
    private readonly RealJwtFactory httpClientFactory = new();

    public async ValueTask InitializeAsync()
    {
        await this.httpClientFactory.ResetDatabase();

        this.httpClientFactory.EmailSender.FailWelcomeEmails = true;
    }

    public ValueTask DisposeAsync()
    {
        this.httpClientFactory.Dispose();
        return ValueTask.CompletedTask;
    }

    [Fact]
    public async Task Register_ShouldSucceed_AndAlso_ShouldKeepTheUser_WhenTheWelcomeEmailFails()
    {
        var httpClient = this.httpClientFactory.CreateClient();

        var form = new MultipartFormDataContent
        {
            { new StringContent("newreader"), "Username" },
            { new StringContent("newreader@test.local"), "Email" },
            { new StringContent("Passw0rd123"), "Password" },
            { new StringContent("New"), "FirstName" },
            { new StringContent("Reader"), "LastName" },
            { new StringContent(new DateTime(1995, 1, 1).ToString("O", CultureInfo.InvariantCulture)), "DateOfBirth" },
            { new StringContent("false"), "IsPrivate" }
        };

        var response = await httpClient.PostAsync("/Identity/register/", form, TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken));
        json.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();

        // The background sender is invoked (and throws) after the response was already returned.
        var attemptedEmail = await this
            .httpClientFactory
            .EmailSender
            .WelcomeAttempted
            .WaitAsync(TimeSpan.FromSeconds(10), TestContext.Current.CancellationToken);

        attemptedEmail.Should().Be("newreader@test.local");

        using var scope = this.httpClientFactory.Services.CreateScope();
        var data = scope.ServiceProvider.GetRequiredService<BookHubDbContext>();

        var user = await data
            .Users
            .IgnoreQueryFilters()
            .SingleAsync(u => u.UserName == "newreader", cancellationToken: TestContext.Current.CancellationToken);

        user.IsDeleted.Should().BeFalse();

        var profileExists = await data
            .Profiles
            .AnyAsync(p => p.UserId == user.Id, cancellationToken: TestContext.Current.CancellationToken);

        profileExists.Should().BeTrue();
    }

    [Fact]
    public async Task Register_ShouldReturnConflictProblem_WhenTheUsernameIsTaken()
    {
        await this.httpClientFactory.WithData(data => data.SeedUser("existing-id", "takenname"));

        var httpClient = this.httpClientFactory.CreateClient();

        var response = await httpClient.PostAsync("/Identity/register/", RealJwtFactory.RegisterForm("takenname", "someone-new@test.local"), TestContext.Current.CancellationToken);

        await response.ShouldBeProblem(
            HttpStatusCode.Conflict,
            "Username 'takenname' is already taken.");
    }

    [Fact]
    public async Task Login_ShouldReturnBadRequestProblem_WhenTheCredentialsAreWrong()
    {
        await this.httpClientFactory.WithData(data => data.SeedUser("existing-id", "reader"));

        var httpClient = this.httpClientFactory.CreateClient();

        var response = await httpClient.PostAsJsonAsync("/Identity/login/", new { credentials = "reader", password = "WrongPassw0rd", rememberMe = false }, cancellationToken: TestContext.Current.CancellationToken);

        await response.ShouldBeProblem(
            HttpStatusCode.BadRequest,
            "Invalid log in attempt!");
    }

    [Fact]
    public async Task Register_ShouldLeaveNoUser_AndAllowTheSameUsernameAndEmail_WhenProfileCreationFails()
    {
        // B-02: the user and the profile are created in one transaction, so a failed profile
        // creation leaves nothing behind (before, the user was soft-deleted and its username
        // and email stayed taken for good).
        var imageWriter = this.httpClientFactory.GetImageWriterMock();
        imageWriter.FailWritesFor = "profiles";

        var httpClient = this.httpClientFactory.CreateClient();

        var failed = await httpClient.PostAsync("/Identity/register/", RealJwtFactory.RegisterForm("unlucky", "unlucky@test.local"), TestContext.Current.CancellationToken);

        await failed.ShouldBeProblem(
            HttpStatusCode.BadRequest,
            "Invalid register attempt!");

        var userRows = await this.httpClientFactory.WithData(data => data
            .Users
            .IgnoreQueryFilters()
            .CountAsync(u => u.NormalizedUserName == "UNLUCKY" || u.NormalizedEmail == "UNLUCKY@TEST.LOCAL"));

        userRows.Should().Be(0);

        imageWriter.FailWritesFor = null;

        var retried = await httpClient.PostAsync("/Identity/register/", RealJwtFactory.RegisterForm("unlucky", "unlucky@test.local"), TestContext.Current.CancellationToken);

        retried.StatusCode.Should().Be(HttpStatusCode.OK);

        var login = await httpClient.PostAsJsonAsync("/Identity/login/", new { credentials = "unlucky", password = RealJwtFactory.DefaultPassword, rememberMe = false }, cancellationToken: TestContext.Current.CancellationToken);

        login.StatusCode.Should().Be(HttpStatusCode.OK);

        var profileExists = await this.httpClientFactory.WithData(data => data
            .Profiles
            .AnyAsync(p => p.User.UserName == "unlucky"));

        profileExists.Should().BeTrue();
    }
}
