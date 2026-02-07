namespace BookHub.Tests;

using System.Data.Common;
using System.Net.Http.Headers;
using BookHub.Areas.Admin.Service;
using BookHub.Tests.Shared.Data;
using Data;
using Infrastructure.Services.ImageWriter;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shared.Identity;
using Shared.Mocks;

public sealed class BookHubWebApplicationFactory : WebApplicationFactory<Program>
{
    private DbConnection? connection;

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

    public async Task ResetDatabase()
    {
        using var scope = this.Services.CreateScope();
        var data = scope
            .ServiceProvider
            .GetRequiredService<BookHubDbContext>();

        await data.Database.EnsureDeletedAsync();
        await data.Database.EnsureCreatedAsync();
    }

    public ImageWriterMock GetImageWriterMock()
    {
        using var scope = this.Services.CreateScope();

        return (ImageWriterMock)scope
            .ServiceProvider
            .GetRequiredService<IImageWriter>();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        TestSeedFiles.EnsureSeedFileExists();

        builder
            .UseEnvironment("Testing")
            .ConfigureServices(services =>
            {
                this.connection = new SqliteConnection("DataSource=:memory:");
                this.connection.Open();

                services
                    .AddSingleton(this.connection)
                    .AddDbContext<BookHubDbContext>(
                        options => options.UseSqlite(this.connection))
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
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            this.connection?.Dispose();
            this.connection = null;
        }
    }
}
