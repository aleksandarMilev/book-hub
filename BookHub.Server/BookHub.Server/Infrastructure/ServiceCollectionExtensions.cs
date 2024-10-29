namespace BookHub.Server.Infrastructure
{
    using System.Text;

    using BookHub.Server.Data;
    using BookHub.Server.Data.Models;
    using BookHub.Server.Features.Books;
    using BookHub.Server.Features.Identity;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BookHubDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetDefaultConnectionString());
            });

            return services;
        }

        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
               services
               .AddIdentity<User, IdentityRole>(opt =>
               {
                   opt.Password.RequireUppercase = false;
                   opt.Password.RequireLowercase = false;
                   opt.Password.RequireNonAlphanumeric = false;
                   opt.Password.RequireDigit = false;
               })
               .AddEntityFrameworkStores<BookHubDbContext>();

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, AppSettings appSettings)
        {
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services
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

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            var apiInfo = new OpenApiInfo() 
            { 
                Title = "My BookHub API",
                Version = "v1" 
            };

            services.AddSwaggerGen(c => c.SwaggerDoc("v1", apiInfo));
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services
                .AddTransient<IIdentityService, IdentityService>()
                .AddTransient<IBookService, BookService>();

            return services;
        }

        public static AppSettings GetAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsConfiguration = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsConfiguration);

            return appSettingsConfiguration.Get<AppSettings>()
                ?? throw new InvalidOperationException("AppSettings not found.");
        }
    }
}
