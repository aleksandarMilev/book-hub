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
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Services.ServiceLifetimes;
using Settings;

using static Features.Identity.Shared.Constants.Lockout;
using static Common.Constants.Cors;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCorsPolicy(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment env)
    {
        const string ConfigSectionName = "Cors:AllowedOrigins";
        var allowedOrigins = configuration[ConfigSectionName];

        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicyName, policy =>
            {
                if (env.IsDevelopment())
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(allowedOrigins))
                    {
                        var origins = allowedOrigins
                            .Split(
                                ';',
                                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                        policy
                            .WithOrigins(origins)
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    }
                    else
                    {
                        throw new InvalidOperationException(
                            "Cors:AllowedOrigins is not configured for the current environment.");
                    }
                }
            });
        });

        return services;
    }

    public static IServiceCollection AddAppSettings(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        AddJwtSettings(services, configuration);
        AddEmailSettings(services, configuration);

        return services;
    }

    public static IServiceCollection AddDatabase(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        const string DefaultConnectionSection = "ConnectionStrings__DefaultConnection";
        const string DefaultConnection = "DefaultConnection";

        var connectionString = Environment
            .GetEnvironmentVariable(DefaultConnectionSection)
            ?? configuration.GetConnectionString(DefaultConnection);

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
            .AddIdentityCore<UserDbModel>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(AccountLockoutTimeSpan);
                options.Lockout.MaxFailedAccessAttempts = MaxFailedLoginAttempts;

                if (env.IsDevelopment())
                {
                    const int RequiredDevLength = 6;

                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = RequiredDevLength;
                }
                else
                {
                    const int RequiredProdLength = 8;

                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = RequiredProdLength;
                }
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<BookHubDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }


    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment env)
    {
        var settings = configuration
            .GetSection(nameof(JwtSettings))
            .Get<JwtSettings>()
            ?? throw new InvalidOperationException("JwtSettings section is missing!");

        var key = Encoding.ASCII.GetBytes(settings.Secret);

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                if (env.IsDevelopment())
                {
                    options.RequireHttpsMetadata = false;
                    options.IncludeErrorDetails = true;

                    options.TokenValidationParameters = new TokenValidationParameters
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
                    const int ClockSkewMinutes = 2;

                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidIssuer = settings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = settings.Audience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(ClockSkewMinutes)
                    };
                }
            });

        return services;
    }

    public static IServiceCollection AddSwagger(
        this IServiceCollection services)
    {
        const string Title = "My BookHub API";
        const string Version = "v1";

        var apiInfo = new OpenApiInfo
        {
            Title = Title,
            Version = Version
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
        const string Name = "Database";

        services
            .AddHealthChecks()
            .AddDbContextCheck<BookHubDbContext>(Name);

        return services;
    }

    private static IServiceCollection AddJwtSettings(
        this IServiceCollection services,
        IConfiguration configuration)
        => services.Configure<JwtSettings>(
            configuration.GetSection(nameof(JwtSettings)));

    private static IServiceCollection AddEmailSettings(
        this IServiceCollection services,
        IConfiguration configuration)
        => services.Configure<EmailSettings>(
            configuration.GetSection(nameof(EmailSettings)));
}
