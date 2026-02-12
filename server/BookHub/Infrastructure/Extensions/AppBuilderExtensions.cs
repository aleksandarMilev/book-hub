namespace BookHub.Infrastructure.Extensions;

using Data;
using Features.Identity.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using static Common.Constants.Names;
using static Common.Constants.Cors;

public static class AppBuilderExtensions
{
    public static async Task<IApplicationBuilder> UseMigrations(
        this IApplicationBuilder app)
    {
        using var services = app.ApplicationServices.CreateScope();
        var data = services.ServiceProvider.GetRequiredService<BookHubDbContext>();

        await data.Database.MigrateAsync();

        return app;
    }

    public static IApplicationBuilder UseAppEndpoints(
        this IApplicationBuilder app)
    {
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health");
        });

        return app;
    }

    public static IApplicationBuilder UseSwaggerUI(
        this IApplicationBuilder app) 
        => app
            .UseSwagger()
            .UseSwaggerUI(opt =>
            {
                const string Url = "/swagger/v1/swagger.json";
                const string Name = "BookHub API";

                opt.SwaggerEndpoint(Url, Name);
                opt.RoutePrefix = string.Empty;
            });

    public static IApplicationBuilder UseAllowedCors(
        this IApplicationBuilder app)
        => app.UseCors(CorsPolicyName);

    public static async Task<IApplicationBuilder> UseDevAdminRole(
        this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var services = serviceScope.ServiceProvider;

        var userManager = services.GetRequiredService<UserManager<UserDbModel>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        if (await roleManager.RoleExistsAsync(AdminRoleName))
        {
            return app;
        }

        var role = new IdentityRole
        {   
            Name = AdminRoleName 
        };

        await roleManager.CreateAsync(role);

        const string AdminEmail = "admin@mail.com";
        const string AdminPassword = "admin1234";

        var user = new UserDbModel
        {
            Email = AdminEmail,
            UserName = AdminRoleName
        };

        await userManager.CreateAsync(user, AdminPassword);
        await userManager.AddToRoleAsync(user, role.Name);

        return app;
    }

    public static async Task<IApplicationBuilder> useProductionAdminRole(
        this IApplicationBuilder app)
    {
        using var scope = app
            .ApplicationServices
            .CreateScope();

        var services = scope.ServiceProvider;
        var config = services.GetRequiredService<IConfiguration>();

        var enabledEnvVar = config["BootstrapAdmin:Enabled"];
        var isEnabled = !string.Equals(
            enabledEnvVar,
            "true",
            StringComparison.OrdinalIgnoreCase);

        if (isEnabled)
        {
            return app;
        }

        var email = config["BootstrapAdmin:Email"];
        var password = config["BootstrapAdmin:Password"];
        var roleName = config["BootstrapAdmin:Role"] ?? "Administrator";

        var passwordOrEmailNotProvided = 
            string.IsNullOrWhiteSpace(email) || 
            string.IsNullOrWhiteSpace(password);

        if (passwordOrEmailNotProvided)
        {
            throw new InvalidOperationException(
                "BootstrapAdmin enabled, but Email/Password not set.");
        }

        var userManager = services.GetRequiredService<UserManager<UserDbModel>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        var roleDoNotExist = !await roleManager.RoleExistsAsync(roleName);
        if (roleDoNotExist)
        {
            var role = new IdentityRole
            {
                Name = roleName
            };

            var roleResult = await roleManager.CreateAsync(role);
            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    "Failed to create role: " + string.Join("; ", roleResult.Errors.Select(e => e.Description)));
            }
        }

        var user = await userManager.FindByEmailAsync(email!);
        if (user is null)
        {
            user = new UserDbModel
            {
                Email = email,
                UserName = email
            };

            var createResult = await userManager.CreateAsync(user, password!);
            if (!createResult.Succeeded)
            {
                var errorMessage = string.Join("; ", createResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException(
                    "Failed to create admin user: " + errorMessage);
            }
        }

        if (!await userManager.IsInRoleAsync(user, roleName))
        {
            var addRoleResult = await userManager.AddToRoleAsync(user, roleName);
            if (!addRoleResult.Succeeded)
            {
                var errorMessage = string.Join("; ", addRoleResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException(
                    "Failed to add role: " + errorMessage);
            }
        }

        return app;
    }
}
