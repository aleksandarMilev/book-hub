namespace BookHub.Tests.Identity;

using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Features.Emails;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shared.Utils;

// Runs the app's real JwtBearer pipeline (not the test scheme): the tokens are signed by
// /Identity/register, so OnTokenValidated's deleted-user check is what's under test (S-06).
public sealed class DeletedUserTokenIntegration : IAsyncLifetime
{
    private const string Username = "jwtreader";

    private readonly RealJwtFactory httpClientFactory = new();

    public async ValueTask InitializeAsync()
        => await this.httpClientFactory.ResetDatabase();

    public ValueTask DisposeAsync()
    {
        this.httpClientFactory.Dispose();
        return ValueTask.CompletedTask;
    }

    [Fact]
    public async Task Token_ShouldBeRejectedWith401_AfterTheUserDeletesTheirAccount()
    {
        var httpClient = await this.RegisterAndAuthenticate();

        var before = await httpClient.GetAsync("/Genres/");
        before.StatusCode.Should().Be(HttpStatusCode.OK);

        var delete = await httpClient.DeleteAsync("/Profile");
        delete.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // The same signed, unexpired token.
        var after = await httpClient.GetAsync("/Genres/");

        await after.ShouldBeProblem(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Token_ShouldBeRejectedWith401_WhenTheUserIsSoftDeletedElsewhere()
    {
        var httpClient = await this.RegisterAndAuthenticate();

        // As an admin's DELETE /Administrator/Profile/{id} would do: a soft delete.
        await this.httpClientFactory.WithData(async data =>
        {
            var user = await data.Users.SingleAsync(u => u.UserName == Username);

            data.Users.Remove(user);
            await data.SaveChangesAsync();
        });

        var response = await httpClient.GetAsync("/Profile/mine/");

        await response.ShouldBeProblem(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Token_ShouldBeAccepted_WhileTheUserExists()
    {
        var httpClient = await this.RegisterAndAuthenticate();

        var response = await httpClient.GetAsync("/Profile/mine/");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Request_ShouldBeRejectedWith401_WhenTheTokenIsNotValid()
    {
        var httpClient = this.httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            "not-a-jwt");

        var response = await httpClient.GetAsync("/Genres/");

        await response.ShouldBeProblem(HttpStatusCode.Unauthorized);
    }

    private async Task<HttpClient> RegisterAndAuthenticate()
    {
        var httpClient = this.httpClientFactory.CreateClient();

        var form = new MultipartFormDataContent
        {
            { new StringContent(Username), "Username" },
            { new StringContent($"{Username}@test.local"), "Email" },
            { new StringContent("Passw0rd123"), "Password" },
            { new StringContent("Jwt"), "FirstName" },
            { new StringContent("Reader"), "LastName" },
            { new StringContent(new DateTime(1995, 1, 1).ToString("O", CultureInfo.InvariantCulture)), "DateOfBirth" },
            { new StringContent("false"), "IsPrivate" },
        };

        var response = await httpClient.PostAsync("/Identity/register/", form);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<TokenResponse>();
        body!.Token.Should().NotBeNullOrWhiteSpace();

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            body.Token);

        return httpClient;
    }

    private sealed record TokenResponse(string Token);

    private sealed class RealJwtFactory : BookHubWebApplicationFactory
    {
        protected override bool UseTestAuthentication => false;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            // Email is a true external boundary: registration queues a welcome email.
            builder.ConfigureTestServices(services => services
                .RemoveAll<IEmailSender>()
                .AddSingleton<IEmailSender, NoOpEmailSender>());
        }
    }

    private sealed class NoOpEmailSender : IEmailSender
    {
        public Task SendWelcome(
            string email,
            string username,
            string baseUrl,
            CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        public Task SendPasswordReset(
            string email,
            string resetUrl,
            CancellationToken cancellationToken = default)
            => Task.CompletedTask;
    }
}
