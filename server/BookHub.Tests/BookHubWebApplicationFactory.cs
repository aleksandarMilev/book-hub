namespace BookHub.Tests;

using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Data;
using FluentAssertions;
using Features.Identity.Data.Models;
using Features.Identity.Service;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Shared.Database;
using Shared.Email;
using Shared.Seed;
using Shared.Utils;

using static Common.Constants.Names;

// Runs Program in the "Testing" environment as the real application, with no test doubles:
// - PostgreSQL: a fresh clone of the migrated template per ResetDatabase() (PostgresServer),
//   registered through the app's own AddDatabase;
// - authentication: the real JwtBearer scheme, including OnTokenValidated's deleted-user check.
//   Create*Client mint a real token for a seeded user through the app's IJwtTokenGenerator;
// - admins: the real AdminService, so the admins are the users in the Administrator role;
// - images: the real ImageWriter and ImageValidator, writing under a temporary web root that is
//   deleted with the factory (WebRoot, ImageExists, ImageFiles);
// - email: the real MailKit sender, delivering to a Mailpit container (Mailbox).
// Call ResetDatabase() before the first request or scope.
public class BookHubWebApplicationFactory : WebApplicationFactory<Program>
{
    public const string DefaultPassword = "Passw0rd123";

    // The one CORS origin the test host allows (Cors:AllowedOrigins).
    public const string AllowedOrigin = "http://localhost:5173";

    private TestDatabase? database;

    public BookHubWebApplicationFactory()
        => Directory.CreateDirectory(this.WebRoot);

    // Uploaded images land under {WebRoot}/images/... and static files are served from here.
    public string WebRoot { get; } = Path.Combine(
        Path.GetTempPath(),
        "bookhub-tests",
        Guid.NewGuid().ToString("N"));

    // A mail domain of this factory's own, so parallel tests never read each other's mail.
    public Mailbox Mailbox { get; } = new($"{Guid.NewGuid():N}.test");

    public string Email(string localPart)
        => this.Mailbox.Address(localPart);

    // A client with a real token for the user, seeded first if missing (as "user-{id}").
    public async Task<HttpClient> CreateUserClient(
        string userId = "test-user",
        string? username = null)
    {
        await this.WithData(data => data.SeedUser(userId, username ?? $"user-{userId}"));

        return this.CreateBearerClient(await this.TokenFor(userId));
    }

    // A client with a real token for a user in the real Administrator role (seeded if missing).
    public async Task<HttpClient> CreateAdminClient(
        string userId = "test-admin-id",
        string? username = null)
    {
        await this.WithData(data => data.SeedAdmin(userId, username ?? $"admin-{userId}"));

        return this.CreateBearerClient(await this.TokenFor(userId));
    }

    public HttpClient CreateAnonymousClient()
        => this.CreateClient();

    public HttpClient CreateBearerClient(string token)
    {
        var client = this.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return client;
    }

    // Issues a token the way /Identity/login does: the user's name, email and real role
    // membership, signed by the app's IJwtTokenGenerator with the app's key.
    public async Task<string> TokenFor(string userId)
    {
        using var scope = this.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserDbModel>>();

        var user = await userManager.FindByIdAsync(userId)
            ?? throw new InvalidOperationException($"Seed the user '{userId}' first.");

        var isAdmin = await userManager.IsInRoleAsync(user, AdminRoleName);

        return scope
            .ServiceProvider
            .GetRequiredService<IJwtTokenGenerator>()
            .Generate(user.Id, user.UserName!, user.Email!, isAdmin: isAdmin);
    }

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

    // Logs in through the API and returns a client that sends the issued token.
    public async Task<HttpClient> LoginAndAuthenticate(
        string credentials,
        string password = DefaultPassword)
    {
        var body = await (await this.CreateClient().PostAsJsonAsync(
                "/Identity/login/",
                new { credentials, password, rememberMe = false },
                TestContext.Current.CancellationToken))
            .ShouldHaveBody<TokenResponse>();

        return this.CreateBearerClient(body.Token);
    }

    // Replaces the current database with a fresh clone of the migrated template. The first call
    // also starts the shared Mailpit container, alongside the PostgreSQL one.
    public async Task ResetDatabase()
    {
        var mailpitStarted = MailpitServer.EnsureStarted();

        await this.DropDatabase();
        this.database = await PostgresServer.CreateDatabase();

        await mailpitStarted;
    }

    // Runs work against the database through a scoped DbContext from the app's container
    // (the same DI registration the requests use). Intended for seeding and assertions.
    public async Task WithData(Func<BookHubDbContext, Task> work)
    {
        using var scope = this.Services.CreateScope();
        var data = scope
            .ServiceProvider
            .GetRequiredService<BookHubDbContext>();

        await work(data);
    }

    public async Task<T> WithData<T>(Func<BookHubDbContext, Task<T>> work)
    {
        using var scope = this.Services.CreateScope();
        var data = scope
            .ServiceProvider
            .GetRequiredService<BookHubDbContext>();

        return await work(data);
    }

    // The file behind an image path the app stored (e.g. "/images/books/{guid}.jpg").
    public string ImageFile(string imagePath)
        => Path.Combine(
            this.WebRoot,
            imagePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

    public bool ImageExists(string? imagePath)
        => imagePath is not null && File.Exists(this.ImageFile(imagePath));

    // The image paths stored directly in images/{folder} (not in its subfolders, such as
    // books/pending), as the app would store them.
    public IReadOnlyCollection<string> ImageFiles(string folder)
    {
        var directory = Path.Combine(this.WebRoot, "images", folder.Replace('/', Path.DirectorySeparatorChar));

        return Directory.Exists(directory)
            ? [.. Directory.GetFiles(directory).Select(f => $"/images/{folder}/{Path.GetFileName(f)}")]
            : [];
    }

    // Writes a real image file, so a seeded entity can point at it; returns its image path.
    public string SeedImageFile(
        string folder,
        string fileName = "seeded.jpg")
    {
        var imagePath = $"/images/{folder}/{fileName}";
        var file = this.ImageFile(imagePath);

        Directory.CreateDirectory(Path.GetDirectoryName(file)!);
        File.WriteAllBytes(file, Images.Jpeg);

        return imagePath;
    }

    // Settings validated on startup (ValidateOnStart) must be valid in the Testing environment too.
    private static IWebHostBuilder ConfigureTestSettings(IWebHostBuilder builder)
        => builder
            .UseSetting("JwtSettings:Secret", "test-only-jwt-secret-that-is-at-least-32-bytes")
            .UseSetting("JwtSettings:Issuer", "BookHub.Tests")
            .UseSetting("JwtSettings:Audience", "BookHub.Tests")
            .UseSetting("AppUrlsSettings:ClientBaseUrl", "http://localhost:5173")
            .UseSetting("Cors:AllowedOrigins", AllowedOrigin)
            .UseSetting("EmailSettings:Host", MailpitServer.SmtpHost)
            .UseSetting("EmailSettings:Port", MailpitServer.SmtpMappedPort.ToString(CultureInfo.InvariantCulture))
            .UseSetting("EmailSettings:From", "BookHub <noreply@bookhub.test>")
            .UseSetting("EmailSettings:UseSsl", "false");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
        => ConfigureTestSettings(builder)
            .UseEnvironment("Testing")
            .UseWebRoot(this.WebRoot)
            .ConfigureServices(services => services
                // The app's own registration (retry-on-failure included). The connection string is
                // read per DbContext, so it always points at the latest ResetDatabase().
                .AddDatabase(() => this.database?.ConnectionString
                    ?? throw new InvalidOperationException(
                        $"Call {nameof(ResetDatabase)}() before using the database.")));

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await this.DropDatabase();
        this.DeleteWebRoot();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            this.DropDatabase().GetAwaiter().GetResult();
            this.DeleteWebRoot();
        }
    }

    private async Task DropDatabase()
    {
        var current = this.database;
        this.database = null;

        if (current is not null)
        {
            await current.DisposeAsync();
        }
    }

    private void DeleteWebRoot()
    {
        try
        {
            if (Directory.Exists(this.WebRoot))
            {
                Directory.Delete(this.WebRoot, recursive: true);
            }
        }
        catch (IOException)
        {
            // A file still open by a disposing host; the OS temp cleanup takes it.
        }
        catch (UnauthorizedAccessException)
        {
        }
    }

    public sealed record TokenResponse(string Token);
}
