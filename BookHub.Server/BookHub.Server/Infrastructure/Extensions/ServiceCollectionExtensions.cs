namespace BookHub.Server.Infrastructure.Extensions
{
    using System.Reflection;
    using System.Text;

    using Data;
    using Features.Identity.Data.Models;
    using Filters;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;
    using Services.ServiceLifetimes;

    public static class ServiceCollectionExtensions
    {
        private const int AccountLockoutTimeSpan = 15;

        private const int MaxFailedLoginAttempts = 3;

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
            => services
                .AddDbContext<BookHubDbContext>(options =>
                {
                    options.UseSqlServer(configuration.GetDefaultConnectionString());
                });

        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services
                .AddIdentity<User, IdentityRole>(opt =>
                {
                    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(AccountLockoutTimeSpan); 
                    opt.Lockout.MaxFailedAccessAttempts = MaxFailedLoginAttempts; 
                    opt.Lockout.AllowedForNewUsers = true;

                    opt.User.RequireUniqueEmail = true;
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
                        ValidateAudience = false
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
            var singletonInterfaceType = typeof(ISingletonService);
            var scopedInterfaceType = typeof(IScopedService);
            var transientInterfaceType = typeof(ITransientService);

            Assembly
                .GetExecutingAssembly()
                .GetExportedTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Select(t => new
                {
                    Service = t.GetInterface($"I{t.Name}"),
                    Implementation = t

                })
                .Where(t => t.Service != null)
                .ToList()
                .ForEach(t =>
                {
                    if (singletonInterfaceType.IsAssignableFrom(t.Service))
                    {
                        services.AddSingleton(t.Service, t.Implementation);
                    }

                    if (scopedInterfaceType.IsAssignableFrom(t.Service))
                    {
                        services.AddScoped(t.Service, t.Implementation);
                    }

                    if (transientInterfaceType.IsAssignableFrom(t.Service))
                    {
                        services.AddTransient(t.Service, t.Implementation);
                    }
                });

            return services;
        }

        public static IServiceCollection AddApiControllers(this IServiceCollection services)
        {
            services
                .AddControllers(opt =>
                {
                    opt.Filters.Add<ModelOrNotFoundActionFilter>();
                });

            return services;
        }

        public static AppSettings GetAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsConfiguration = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsConfiguration);

            return appSettingsConfiguration
                    .Get<AppSettings>()
                    ?? throw new InvalidOperationException("AppSettings not found.");
        }

        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
            => services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}
