namespace BookHub.Server.Infrastructure.Extensions
{
    using Data;
    using Microsoft.EntityFrameworkCore;

    public static class AppBuilderExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using var services = app.ApplicationServices.CreateScope();
            var data = services.ServiceProvider.GetService<BookHubDbContext>();
            data?.Database.Migrate();
        }

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
    }
}
