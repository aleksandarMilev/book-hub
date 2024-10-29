namespace BookHub.Server
{
    using BookHub.Server.Data;
    using BookHub.Server.Infrastructure;
    using Microsoft.EntityFrameworkCore;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var appSettings = builder.Services.GetAppSettings(builder.Configuration);

            builder.Services
                .AddDbContext<BookHubDbContext>(options =>
                {
                    options.UseSqlServer(builder.Configuration.GetDefaultConnectionString());
                })
                .AddIdentity()
                .AddJwtAuthentication(appSettings)
                .AddControllers();

            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            }

            var app = builder.Build();

            app
                .UseRouting()
                .UseCors(opt =>
                {
                    opt.AllowAnyOrigin();
                    opt.AllowAnyHeader();
                    opt.AllowAnyMethod();

                })
                .UseAuthorization()
                .UseAuthorization()
                .UseEndpoints(e => e.MapControllers())
                .ApplyMigrations();

            app.Run();
        }
    }
}
