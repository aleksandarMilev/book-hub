namespace BookHub.Tests.Shared.Identity;

using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Features.Emails;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mocks;

// Runs the app's real JwtBearer pipeline (not the test scheme), so tokens come from
// /Identity/register and /Identity/login and OnTokenValidated's deleted-user check runs.
// The admins are the real Administrator role members (none unless a test seeds them), and
// email goes to a RecordingEmailSender.
public class RealJwtFactory : BookHubWebApplicationFactory
{
    public const string DefaultPassword = "Passw0rd123";

    public RecordingEmailSender EmailSender { get; } = new();

    protected override bool UseTestAuthentication => false;

    protected override bool UseRealAdminService => true;

    public static MultipartFormDataContent RegisterForm(
        string username,
        string? email = null,
        string password = DefaultPassword,
        bool isPrivate = false)
        => new()
        {
            { new StringContent(username), "Username" },
            { new StringContent(email ?? $"{username}@test.local"), "Email" },
            { new StringContent(password), "Password" },
            { new StringContent("First"), "FirstName" },
            { new StringContent("Last"), "LastName" },
            { new StringContent(new DateTime(1995, 1, 1).ToString("O", CultureInfo.InvariantCulture)), "DateOfBirth" },
            { new StringContent(isPrivate ? "true" : "false"), "IsPrivate" },
        };

    public HttpClient CreateBearerClient(string token)
    {
        var client = this.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return client;
    }

    // Registers through the API and returns a client that sends the issued token.
    public async Task<HttpClient> RegisterAndAuthenticate(
        string username,
        bool isPrivate = false)
    {
        var response = await this
            .CreateClient()
            .PostAsync(
                "/Identity/register/",
                RegisterForm(username, isPrivate: isPrivate),
                TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<TokenResponse>(
            TestContext.Current.CancellationToken);

        body!.Token.Should().NotBeNullOrWhiteSpace();

        return this.CreateBearerClient(body.Token);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureTestServices(services => services
            .RemoveAll<IEmailSender>()
            .AddSingleton<IEmailSender>(this.EmailSender));
    }

    public sealed record TokenResponse(string Token);
}
