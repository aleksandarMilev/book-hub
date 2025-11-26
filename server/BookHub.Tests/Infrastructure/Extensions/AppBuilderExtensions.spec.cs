namespace BookHub.Tests.Infrastructure.Extensions;

using BookHub.Data;
using BookHub.Features.Identity.Data.Models;
using BookHub.Infrastructure.Extensions;
using BookHub.Infrastructure.Services.CurrentUser;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

using static BookHub.Common.Constants.Names;

public class AppBuilderExtensionsTests
{
    [Fact]
    public async Task UseAdminRole_CreatesAdminRoleAndUser_WhenRoleDoesNotExist()
    {
        var app = CreateApplicationBuilder(
            nameof(UseAdminRole_CreatesAdminRoleAndUser_WhenRoleDoesNotExist));

        await app.UseAdminRole();

        using var scope = app.ApplicationServices.CreateScope();
        var provider = scope.ServiceProvider;

        var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = provider.GetRequiredService<UserManager<User>>();

        Assert.True(await roleManager.RoleExistsAsync(AdminRoleName));

        var adminUser = await userManager.FindByEmailAsync("admin@mail.com");
        Assert.NotNull(adminUser);
        Assert.True(await userManager.IsInRoleAsync(adminUser!, AdminRoleName));
    }

    [Fact]
    public async Task UseAdminRole_DoesNothing_WhenRoleAlreadyExists()
    {
        var app = CreateApplicationBuilder(nameof(UseAdminRole_DoesNothing_WhenRoleAlreadyExists));

        using (var firstScope = app.ApplicationServices.CreateScope())
        {
            var provider = firstScope.ServiceProvider;
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();

            await roleManager.CreateAsync(new IdentityRole(AdminRoleName));
        }

        await app.UseAdminRole();

        using var secondScope = app.ApplicationServices.CreateScope();
        var provider2 = secondScope.ServiceProvider;
        var userManager = provider2.GetRequiredService<UserManager<User>>();
        var adminUser = await userManager.FindByEmailAsync("admin@mail.com");

        Assert.Null(adminUser);
    }

    [Fact]
    public void UseAllowedCors_ReturnsSameAppInstance()
    {
        var app = CreateApplicationBuilder(
            nameof(UseAllowedCors_ReturnsSameAppInstance));

        var result = app.UseAllowedCors();

        Assert.Same(app, result);
    }

    [Fact]
    public void UseSwaggerUI_ReturnsNonNullBuilder()
    {
        var app = CreateApplicationBuilder(nameof(UseSwaggerUI_ReturnsNonNullBuilder));

        var result = app.UseSwaggerUI();

        Assert.NotNull(result);
    }

    private IApplicationBuilder CreateApplicationBuilder(string dbName)
    {
        var services = new ServiceCollection();

        services.AddDbContext<BookHubDbContext>(options =>
            options.UseInMemoryDatabase(dbName));

        services.AddScoped<ICurrentUserService, FakeCurrentUserService>();

        services
            .AddLogging()
            .AddIdentityCore<User>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<BookHubDbContext>();

        services.AddRouting();
        services.AddControllers();
        services.AddHealthChecks();
        services.AddCors();
        services.AddSwaggerGen();

        var provider = services.BuildServiceProvider();
        return new ApplicationBuilder(provider);
    }

    private sealed class FakeCurrentUserService : ICurrentUserService
    {
        public string? GetUsername()
            => null;

        public string? GetId()
            => null;

        public bool IsAdmin()
            => false;
    }
}
