namespace BookHub.Tests;

using System.Net.Http.Headers;
using Areas.Admin.Service;
using BookHub.Infrastructure.Services.ImageWriter;
using Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shared.Database;
using Shared.Identity;
using Shared.Mocks;

// Runs Program in the "Testing" environment against a real PostgreSQL database
// (see PostgresServer). Call ResetDatabase() before the first request or scope.
public class BookHubWebApplicationFactory : WebApplicationFactory<Program>
{
    private TestDatabase? database;

    public HttpClient CreateUserClient(
        string userId = "test-user",
        string username = "user")
    {
        var client = this.CreateClient();
        client
            .DefaultRequestHeaders
            .Authorization = new AuthenticationHeaderValue(
                IdentityHandler.SchemeName,
                $"user:{userId}:{username}");

        return client;
    }

    public HttpClient CreateAdminClient(
        string userId = "test-admin-id",
        string username = "admin")
    {
        var client = this.CreateClient();
        client
            .DefaultRequestHeaders
            .Authorization = new AuthenticationHeaderValue(
                IdentityHandler.SchemeName,
                $"admin:{userId}:{username}");

        return client;
    }

    public HttpClient CreateAnonymousClient()
        => this.CreateClient();

    // Replaces the current database with a fresh clone of the migrated template.
    public async Task ResetDatabase()
    {
        await this.DropDatabase();
        this.database = await PostgresServer.CreateDatabase();
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

    public ImageWriterMock GetImageWriterMock()
    {
        using var scope = this.Services.CreateScope();

        return (ImageWriterMock)scope
            .ServiceProvider
            .GetRequiredService<IImageWriter>();
    }

    // Settings validated on startup (ValidateOnStart) must be valid in the Testing environment too.
    internal static IWebHostBuilder ConfigureTestSettings(IWebHostBuilder builder)
        => builder
            .UseSetting("JwtSettings:Secret", "test-only-jwt-secret-that-is-at-least-32-bytes")
            .UseSetting("AppUrlsSettings:ClientBaseUrl", "http://localhost:5173");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
        => ConfigureTestSettings(builder)
            .UseEnvironment("Testing")
            .ConfigureServices(services =>
            {
                services
                    // The options are built per scope, so they always point at the
                    // database from the latest ResetDatabase().
                    .AddDbContext<BookHubDbContext>(options => options.UseNpgsql(
                        this.database?.ConnectionString
                            ?? throw new InvalidOperationException(
                                $"Call {nameof(ResetDatabase)}() before using the database.")))
                    .AddHttpContextAccessor()
                    .RemoveAll<IImageWriter>()
                    .AddSingleton<IImageWriter, ImageWriterMock>()
                    .RemoveAll<IAdminService>()
                    .AddScoped<IAdminService>(_ => new AdminServiceMock("test-admin-id"))
                    .AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = IdentityHandler.SchemeName;
                        options.DefaultChallengeScheme = IdentityHandler.SchemeName;
                        options.DefaultScheme = IdentityHandler.SchemeName;
                    })
                    .AddScheme<AuthenticationSchemeOptions, IdentityHandler>(
                        IdentityHandler.SchemeName, _ => { });
            });

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await this.DropDatabase();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            this.DropDatabase().GetAwaiter().GetResult();
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
}
