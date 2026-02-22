namespace BookHub.Infrastructure.Extensions;

using Data;
using Features.Identity.Data.Models;
using Features.Identity.Service;
using Features.Identity.Service.Models;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using static Common.Constants.Cors;
using static Common.Constants.Names;

public static class AppBuilderExtensions
{
    public static async Task<IApplicationBuilder> UseCustomForwardedHeaders(
        this IApplicationBuilder app)
    {
        var forwardedHeadersOptions = new ForwardedHeadersOptions
        {
            ForwardedHeaders =
                ForwardedHeaders.XForwardedFor |
                ForwardedHeaders.XForwardedProto,
            ForwardLimit = 1,
        };

        forwardedHeadersOptions.KnownProxies.Clear();

        app.UseForwardedHeaders(forwardedHeadersOptions);

        return app;
    }

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

    public static async Task<IApplicationBuilder> UseBuiltInUser(
        this IApplicationBuilder app)
    {
        using var serviceScope = app
            .ApplicationServices
            .CreateScope();

        var identityService = serviceScope
            .ServiceProvider
            .GetRequiredService<IIdentityService>();

        var serviceModel = new RegisterServiceModel()
        {
            Username = "mileww.sasho",
            Email = "aleksandarmilev23@gmail.com",
            Password = "123456",
            FirstName = "Alexandar",
            LastName = "Milev",
            Image = null,
            DateOfBirth = new DateTime(2001, 2, 8),
            SocialMediaUrl = "https://github.com/aleksandarMilev/",
            Biography = "My bio is here and is long enough.",
            IsPrivate = false,
        };

        await identityService.Register(serviceModel);

        return app;
    }

    public static async Task<IApplicationBuilder> UseDevAdminRole(
        this IApplicationBuilder app)
    {
        using var serviceScope = app
            .ApplicationServices
            .CreateScope();

        var services = serviceScope.ServiceProvider;

        var userManager = services
            .GetRequiredService<UserManager<UserDbModel>>();

        var roleManager = services
            .GetRequiredService<RoleManager<IdentityRole>>();

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

    // We need this method to create administrator if we drop the production db for some reason.
    // Don't delete it!
    public static async Task<IApplicationBuilder> UseProductionAdminRole(
        this IApplicationBuilder app)
    {
        using var scope = app
            .ApplicationServices
            .CreateScope();

        var services = scope.ServiceProvider;

        var logger = services
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger("BootstrapAdmin");

        var config = services.GetRequiredService<IConfiguration>();
        var enabled = string.Equals(
            config["BootstrapAdmin:Enabled"],
            "true",
            StringComparison.OrdinalIgnoreCase);

        logger.LogInformation("BootstrapAdmin Enabled = {Enabled}", enabled);

        if (!enabled)
        {
            return app;
        }

        var email = config["BootstrapAdmin:Email"];
        var password = config["BootstrapAdmin:Password"];
        var roleName = config["BootstrapAdmin:Role"] ?? "Administrator";

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException(
                "BootstrapAdmin enabled but Email/Password not set.");
        }

        var userManager = services
            .GetRequiredService<UserManager<UserDbModel>>();

        var roleManager = services
            .GetRequiredService<RoleManager<IdentityRole>>();

        if (!await roleManager.RoleExistsAsync(roleName))
        {
            var role = new IdentityRole(roleName);
            var createRoleResult = await roleManager.CreateAsync(role);

            if (!createRoleResult.Succeeded)
            {
                var roleResultErrorMessage = createRoleResult
                    .Errors
                    .Select(e => e.Description);

                throw new InvalidOperationException(
                    "Failed to create role: " + string.Join("; ", createRoleResult));
            }

            logger.LogInformation("Created role {Role}", roleName);
        }
        else
        {
            logger.LogInformation("Role {Role} already exists", roleName);
        }

        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            user = new()
            { 
                Email = email,
                UserName = email 
            };

            var createUserResult = await userManager.CreateAsync(user, password);
            if (!createUserResult.Succeeded)
            {
                var errorMessage = createUserResult
                    .Errors
                    .Select(e => e.Description);

                throw new InvalidOperationException(
                    "Failed to create admin user: " + string.Join("; ", errorMessage));
            }

            logger.LogInformation("Created user {Email}", email);
        }
        else
        {
            logger.LogInformation("User {Email} already exists", email);
        }

        if (!await userManager.IsInRoleAsync(user, roleName))
        {
            var addToRoleResult = await userManager.AddToRoleAsync(user, roleName);
            if (!addToRoleResult.Succeeded)
            {
                var errorMessage = addToRoleResult
                    .Errors
                    .Select(e => e.Description);

                throw new InvalidOperationException(
                    "Failed to add role: " + string.Join("; ", errorMessage));
            }

            logger.LogInformation("Added user {Email} to role {Role}", email, roleName);
        }
        else
        {
            logger.LogInformation("User {Email} already in role {Role}", email, roleName);
        }

        return app;
    }
}
