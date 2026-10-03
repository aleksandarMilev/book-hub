namespace BookHub.Tests.Identity;

using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Data;
using Features.Emails;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shared.Seed;
using Shared.Utils;

public sealed class IdentityIntegration : IAsyncLifetime
{
    private readonly ThrowingEmailSenderFactory httpClientFactory = new();

    public async ValueTask InitializeAsync()
        => await this.httpClientFactory.ResetDatabase();

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

        var response = await httpClient.PostAsync("/Identity/register/", form);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        json.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();

        // The background sender is invoked (and throws) after the response was already returned.
        var attemptedEmail = await this
            .httpClientFactory
            .EmailSender
            .WelcomeAttempted
            .WaitAsync(TimeSpan.FromSeconds(10));

        attemptedEmail.Should().Be("newreader@test.local");

        using var scope = this.httpClientFactory.Services.CreateScope();
        var data = scope.ServiceProvider.GetRequiredService<BookHubDbContext>();

        var user = await data
            .Users
            .IgnoreQueryFilters()
            .SingleAsync(u => u.UserName == "newreader");

        user.IsDeleted.Should().BeFalse();

        var profileExists = await data
            .Profiles
            .AnyAsync(p => p.UserId == user.Id);

        profileExists.Should().BeTrue();
    }

    [Fact]
    public async Task Register_ShouldReturnConflictProblem_WhenTheUsernameIsTaken()
    {
        await this.httpClientFactory.WithData(data => data.SeedUser("existing-id", "takenname"));

        var httpClient = this.httpClientFactory.CreateClient();

        var response = await httpClient.PostAsync(
            "/Identity/register/",
            RegisterForm("takenname", "someone-new@test.local"));

        await response.ShouldBeProblem(
            HttpStatusCode.Conflict,
            "Username 'takenname' is already taken.");
    }

    [Fact]
    public async Task Login_ShouldReturnBadRequestProblem_WhenTheCredentialsAreWrong()
    {
        await this.httpClientFactory.WithData(data => data.SeedUser("existing-id", "reader"));

        var httpClient = this.httpClientFactory.CreateClient();

        var response = await httpClient.PostAsJsonAsync(
            "/Identity/login/",
            new { credentials = "reader", password = "WrongPassw0rd", rememberMe = false });

        await response.ShouldBeProblem(
            HttpStatusCode.BadRequest,
            "Invalid log in attempt!");
    }

    private static MultipartFormDataContent RegisterForm(
        string username,
        string email)
        => new()
        {
            { new StringContent(username), "Username" },
            { new StringContent(email), "Email" },
            { new StringContent("Passw0rd123"), "Password" },
            { new StringContent("New"), "FirstName" },
            { new StringContent("Reader"), "LastName" },
            { new StringContent(new DateTime(1995, 1, 1).ToString("O", CultureInfo.InvariantCulture)), "DateOfBirth" },
            { new StringContent("false"), "IsPrivate" }
        };

    private sealed class ThrowingEmailSenderFactory : BookHubWebApplicationFactory
    {
        public ThrowingEmailSender EmailSender { get; } = new();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureTestServices(services => services
                .RemoveAll<IEmailSender>()
                .AddSingleton<IEmailSender>(this.EmailSender));
        }
    }

    private sealed class ThrowingEmailSender : IEmailSender
    {
        private readonly TaskCompletionSource<string> welcomeAttempted =
            new(TaskCreationOptions.RunContinuationsAsynchronously);

        public Task<string> WelcomeAttempted => this.welcomeAttempted.Task;

        public Task SendWelcome(
            string email,
            string username,
            string baseUrl,
            CancellationToken cancellationToken = default)
        {
            this.welcomeAttempted.TrySetResult(email);
            throw new InvalidOperationException("SMTP server is down.");
        }

        public Task SendPasswordReset(
            string email,
            string resetUrl,
            CancellationToken cancellationToken = default)
            => throw new InvalidOperationException("SMTP server is down.");
    }
}
