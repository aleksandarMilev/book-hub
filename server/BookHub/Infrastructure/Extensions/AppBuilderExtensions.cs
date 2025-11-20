namespace BookHub.Infrastructure.Extensions
{
    using Data;
    using Features.Identity.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using static Common.Constants.Names;

    public static class AppBuilderExtensions
    {
        private static readonly string AdminEmail = "admin@mail.com";
        private static readonly string AdminPassword = "admin1234";

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
            => app.UseEndpoints(e => e.MapControllers());

        public static IApplicationBuilder UseSwaggerUI(
            this IApplicationBuilder app) 
            => app
                .UseSwagger()
                .UseSwaggerUI(opt =>
                {
                    opt.SwaggerEndpoint("swagger/v1/swagger.json", "BookHub API");
                    opt.RoutePrefix = string.Empty;
                });

        public static IApplicationBuilder UseAllowedCors(
            this IApplicationBuilder app)
            => app
                 .UseCors(opt =>
                 { 
                     opt.AllowAnyOrigin();
                     opt.AllowAnyHeader();
                     opt.AllowAnyMethod();
                 });

        public static async Task<IApplicationBuilder> UseAdminRole(
            this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            if (await roleManager.RoleExistsAsync(AdminRoleName))
            {
                return app;
            }

            var role = new IdentityRole() { Name = AdminRoleName };

            await roleManager.CreateAsync(role);

            var user = new User()
            {
                Email = AdminEmail,
                UserName = AdminRoleName
            };

            await userManager.CreateAsync(user, AdminPassword);
            await userManager.AddToRoleAsync(user, role.Name);

            return app;
        }      
    }
}
