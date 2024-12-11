namespace BookHub.Server.Infrastructure.Extensions
{
    using Data;
    using Features.Identity.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using static BookHub.Server.Common.Constants;

    public static class AppBuilderExtensions
    {
        public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
        {
            using var services = app.ApplicationServices.CreateScope();
            var data = services.ServiceProvider.GetService<BookHubDbContext>();
            data?.Database.Migrate();

            return app;
        }

        public static IApplicationBuilder UseAppEndpoints(this IApplicationBuilder app)
            => app.UseEndpoints(e => e.MapControllers());

        public static IApplicationBuilder UseSwaggerUI(this IApplicationBuilder app) 
            => app
                .UseSwagger()
                .UseSwaggerUI(opt =>
                {
                    opt.SwaggerEndpoint("swagger/v1/swagger.json", "My BookHub API");
                    opt.RoutePrefix = string.Empty;
                });

        public static IApplicationBuilder UseAllowedCors(this IApplicationBuilder app)
            => app
                 .UseCors(opt =>
                 { 
                     opt.AllowAnyOrigin();
                     opt.AllowAnyHeader();
                     opt.AllowAnyMethod();
                 });

        public static IApplicationBuilder AddAdmin(this IApplicationBuilder app) 
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            CreateAdminRole(services);

            return app;
        }

        private static void CreateAdminRole(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(AdminRoleName))
                    {
                        return;
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
                })
                .GetAwaiter()
                .GetResult();
        }
    }
}
