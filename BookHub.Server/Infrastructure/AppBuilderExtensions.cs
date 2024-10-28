namespace BookHub.Server.Infrastructure
{
    using BookHub.Server.Data;
    using Microsoft.EntityFrameworkCore;

    public static class AppBuilderExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using var services = app.ApplicationServices.CreateScope();
            var data = services.ServiceProvider.GetService<BookHubDbContext>();
            data?.Database.Migrate();
        }
    }
}
