namespace BookHub.Tests.Genres;

using Areas.Admin.Service;
using BookHub.Data;
using Infrastructure.Services.ImageWriter;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shared.Identity;
using Shared.Mocks;

public sealed class GenresWebApplicationFactory : BookHubWebApplicationFactory
{
    private readonly InMemoryDatabaseRoot dbRoot = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
        => builder
            .UseEnvironment("Testing")
            .ConfigureServices(services =>
            {
                services.RemoveAll<DbContextOptions<BookHubDbContext>>();
                services.RemoveAll<BookHubDbContext>();

                services.AddDbContext<BookHubDbContext>(options =>
                    options.UseInMemoryDatabase("BookHub_Genres", this.dbRoot));

                services
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
