namespace BookHub.Server
{
    using System.Text;

    using BookHub.Server.Data;
    using BookHub.Server.Data.Models;
    using BookHub.Server.Infrastructure;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder
                .Configuration
                .GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder
                .Services
                .AddDbContext<BookHubDbContext>(options => options.UseSqlServer(connectionString));

            builder
                 .Services
                 .AddIdentity<User, IdentityRole>(opt => 
                 {
                     opt.Password.RequireUppercase = false;
                     opt.Password.RequireLowercase = false;
                     opt.Password.RequireNonAlphanumeric = false;
                     opt.Password.RequireDigit = false;
                 })
                 .AddEntityFrameworkStores<BookHubDbContext>();

            var appSettingsConfiguration = builder.Configuration.GetSection("AppSettings");
            builder
                .Services
                .Configure<AppSettings>(appSettingsConfiguration);

            var appSettings = appSettingsConfiguration
                .Get<AppSettings>()
                ?? throw new InvalidOperationException("AppSettings not found.");

            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            builder
                .Services
                .AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(opt =>
                {
                    opt.RequireHttpsMetadata = false;
                    opt.SaveToken = true;
                    opt.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                });

            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            }

            builder.Services.AddControllers();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }

            app.UseRouting();
            app.UseCors(opt =>
            {
                opt.AllowAnyOrigin();
                opt.AllowAnyHeader();
                opt.AllowAnyMethod();

            });

            app.UseAuthorization();
            app.MapControllers();
            app.ApplyMigrations();
            app.Run();
        }
    }
}
