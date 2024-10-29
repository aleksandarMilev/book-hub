namespace BookHub.Server
{
    using BookHub.Server.Infrastructure;

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
                .AddServices()
                .AddSwagger()
                .AddControllers();

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
                .UseCors(opt =>
                {
                    opt.AllowAnyOrigin();
                    opt.AllowAnyHeader();
                    opt.AllowAnyMethod();

                })
                .UseAuthorization()
                .UseAuthorization()
                .UseEndpoints(e => e.MapControllers())
                .UseSwaggerUI()
                .ApplyMigrations();

            app.Run();
        }
    }
}
