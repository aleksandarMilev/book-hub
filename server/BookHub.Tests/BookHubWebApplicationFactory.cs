namespace BookHub.Tests;

using BookHub.Tests.Shared.Data;
using Data;
using Infrastructure.Services.CurrentUser;
using Infrastructure.Services.ImageWriter;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shared.Identity;
using Shared.Mocks;
using System.Data.Common;
using System.Net.Http.Headers;
using System.Reflection;

public sealed class BookHubWebApplicationFactory : WebApplicationFactory<Program>
{
    private DbConnection? connection;

    public HttpClient CreateAdminClient()
    {
        var client = this.CreateClient();

        client
            .DefaultRequestHeaders
            .Authorization = new AuthenticationHeaderValue(
                IdentityHandler.SchemeName,
                "admin");

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

    public FakeImageWriter GetImageWriterMock()
    {
        using var scope = this.Services.CreateScope();

        return (FakeImageWriter)scope
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
                    .RemoveAll<ICurrentUserService>()
                    .AddScoped<ICurrentUserService>(
                        _ => new CurrentUserServiceMock("test-user"))
                    .RemoveAll<IImageWriter>()
                    .AddSingleton<IImageWriter, FakeImageWriter>()
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

    private static bool IsSqlServerDescriptor(ServiceDescriptor descriptor)
    {
        static bool IsFromSqlServerAssembly(Assembly? assembly)
            => assembly?
                .GetName()
                .Name?
                .StartsWith("Microsoft.EntityFrameworkCore.SqlServer") == true;

        if (IsFromSqlServerAssembly(descriptor.ImplementationType?.Assembly))
        {
            return true;
        }

        if (IsFromSqlServerAssembly(descriptor.ImplementationInstance?.GetType().Assembly))
        {
            return true;
        }

        if (descriptor.ImplementationFactory is not null)
        {
            var method = descriptor.ImplementationFactory.GetMethodInfo();
            if (IsFromSqlServerAssembly(method.DeclaringType?.Assembly))
            {
                return true;
            }

            if (descriptor.ImplementationFactory.Target is not null &&
                IsFromSqlServerAssembly(descriptor.ImplementationFactory.Target.GetType().Assembly))
            {
                return true;
            }
        }

        return false;
    }
}
