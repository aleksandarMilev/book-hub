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
}
