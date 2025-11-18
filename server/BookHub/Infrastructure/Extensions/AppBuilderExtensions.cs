namespace BookHub.Infrastructure.Extensions
{
    using Data;
    using Microsoft.EntityFrameworkCore;

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
            => app.UseEndpoints(e => e.MapControllers());

        public static IApplicationBuilder UseSwaggerUI(
            this IApplicationBuilder app) 
            => app
                .UseSwagger()
                .UseSwaggerUI(opt =>
                {
                    opt.SwaggerEndpoint("swagger/v1/swagger.json", "My BookHub API");
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
    }
}
