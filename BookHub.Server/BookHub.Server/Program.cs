namespace BookHub.Server
{
    using Infrastructure.Extensions;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var appSettings = builder.Services.GetAppSettings(builder.Configuration);

            builder.Services
                .AddDatabase(builder.Configuration)
                .AddIdentity()
                .AddJwtAuthentication(appSettings)
                .AddApiControllers()
                .AddServices()
                .AddAutoMapper()
                .AddSwagger()
                .AddSignalR();

            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            }

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseRouting()
                .UseAllowedCors()
                .UseAuthentication()
                .UseAuthorization()
                .UseAppEndpoints()
                .UseSwaggerUI()
                .AddAdmin()
                .ApplyMigrations();

            app.Run();
        }
    }
}
