namespace BookHub.Infrastructure.Extensions;

using System.Reflection;
using System.Text;
using Data;
using Features.Identity.Data.Models;
using Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Services.ServiceLifetimes;

using static Features.Identity.Shared.Constants;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppSettings(
        this IServiceCollection services, 
        IConfiguration configuration)
        => services.Configure<ApplicationSettings>(
            configuration.GetSection(nameof(ApplicationSettings)));

    public static IServiceCollection AddDatabase(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        var connectionString = Environment
            .GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
            ?? configuration.GetConnectionString("DefaultConnection");

        return services
            .AddDbContext<BookHubDbContext>(options =>
            {
                options
                 .UseSqlServer(connectionString, sqlOptions =>
                 {
                    sqlOptions.MigrationsAssembly(
                        typeof(BookHubDbContext).Assembly.FullName);
                    sqlOptions.EnableRetryOnFailure();
                 });
            });
    }

    public static IServiceCollection AddIdentity(
        this IServiceCollection services,
        IWebHostEnvironment env)
    {
        services
            .AddIdentity<User, IdentityRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(AccountLockoutTimeSpan);
                opt.Lockout.MaxFailedAccessAttempts = MaxFailedLoginAttempts;

                if (env.IsDevelopment())
                {
                    opt.Password.RequireDigit = false;
                    opt.Password.RequireLowercase = false;
                    opt.Password.RequireUppercase = false;
                    opt.Password.RequireNonAlphanumeric = false;
                    opt.Password.RequiredLength = 6;
                }
                else
                {
                    opt.Password.RequireDigit = true;
                    opt.Password.RequireLowercase = true;
                    opt.Password.RequireUppercase = true;
                    opt.Password.RequireNonAlphanumeric = false;
                    opt.Password.RequiredLength = 8;
                }
            })
            .AddEntityFrameworkStores<BookHubDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment env)
    {
        var appSettings = configuration
            .GetSection(nameof(ApplicationSettings))
            .Get<ApplicationSettings>()
            ?? throw new InvalidOperationException("ApplicationSettings section is missing!");

        var key = Encoding.ASCII.GetBytes(appSettings.Secret);

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.SaveToken = true;
                if (env.IsDevelopment())
                {
                    opt.RequireHttpsMetadata = false;
                    opt.IncludeErrorDetails = true;

                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateLifetime = true
                    };
                }
                else
                {
                    opt.RequireHttpsMetadata = true;
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),

                        ValidateIssuer = true,
                        ValidIssuer = appSettings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = appSettings.Audience,

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(2)
                    };
                }
            });

        return services;
    }

    public static IServiceCollection AddSwagger(
        this IServiceCollection services)
    {
        var apiInfo = new OpenApiInfo
        {
            Title = "My BookHub API",
            Version = "v1"
        };

        services.AddSwaggerGen(c => c.SwaggerDoc("v1", apiInfo));
        return services;
    }

    public static IServiceCollection AddServices(
        this IServiceCollection services)
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
            .Where(t => t.Service is not null)
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

    public static IServiceCollection AddApiControllers(
        this IServiceCollection services)
    {
        services
            .AddControllers(opt =>
            {
                opt.Filters.Add<ModelOrNotFoundActionFilter>();
            });

        return services;
    }

    public static IServiceCollection AddHealthcheck(
        this IServiceCollection services)
    {
        services
            .AddHealthChecks()
            .AddDbContextCheck<BookHubDbContext>("Database");

        return services;
    }

    public static IServiceCollection AddAutoMapper(
        this IServiceCollection services)
        => services.AddAutoMapper(Assembly.GetExecutingAssembly());
}
