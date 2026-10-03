namespace BookHub.Infrastructure.Extensions;

using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading.RateLimiting;
using Data;
using ExceptionHandling;
using Features.Emails;
using Features.Identity.Data.Models;
using Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Services.ServiceLifetimes;
using Settings;

using static Common.Constants.Cors;
using static Features.Identity.Shared.Constants.Lockout;

public static class ServiceCollectionExtensions
{
    // Every error response is a ProblemDetails (RFC 9457) with a traceId: controller
    // failures (ControllerExtensions), unhandled exceptions (GlobalExceptionHandler) and
    // empty-body status codes such as 401/403 (UseStatusCodePages).
    public static IServiceCollection AddErrorHandling(
        this IServiceCollection services)
    {
        services.AddProblemDetails(options =>
            options.CustomizeProblemDetails = context =>
                context
                    .ProblemDetails
                    .Extensions
                    .TryAdd(
                        "traceId",
                        Activity.Current?.Id ?? context.HttpContext.TraceIdentifier));

        // IExceptionHandler can't follow the I{ClassName} convention used by AddServices.
        services.AddExceptionHandler<GlobalExceptionHandler>();

        return services;
    }

    public static IServiceCollection AddRateLimiting(this IServiceCollection services, IWebHostEnvironment env)
    {
        services.AddRateLimiter(options =>
        {
            options.OnRejected = async (context, token) =>
            {
                context
                    .HttpContext
                    .Response
                    .StatusCode = StatusCodes.Status429TooManyRequests;

                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                {
                    context
                        .HttpContext
                        .Response
                        .Headers
                        .RetryAfter =
                        ((int)retryAfter.TotalSeconds).ToString(CultureInfo.InvariantCulture);
                }

                await context
                    .HttpContext
                    .RequestServices
                    .GetRequiredService<IProblemDetailsService>()
                    .WriteAsync(new()
                    {
                        HttpContext = context.HttpContext,
                        ProblemDetails = new()
                        {
                            Status = StatusCodes.Status429TooManyRequests,
                            Title = "Too Many Requests",
                            Detail = "Too many requests. Try again later.",
                        },
                    });
            };

            options.GlobalLimiter = PartitionedRateLimiter
                .Create<HttpContext, string>(httpContext =>
                {
                    var ip = httpContext
                        .Connection
                        .RemoteIpAddress?
                        .ToString()
                        ?? "unknown";

                    return RateLimitPartition
                        .GetFixedWindowLimiter(ip, _ => new()
                        {
                            PermitLimit = env.IsDevelopment()
                                ? 480
                                : 240,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0,
                            AutoReplenishment = true
                        });
                });
        });

        return services;
    }

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
        AddAppUrlsSettings(services, configuration);
        AddBootstrapAdminSettings(services, configuration);

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
                 .UseNpgsql(connectionString, npgsqlOptions =>
                 {
                     npgsqlOptions.MigrationsAssembly(
                         typeof(BookHubDbContext).Assembly.FullName);

                     npgsqlOptions.EnableRetryOnFailure();
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

                options.Lockout.AllowedForNewUsers = true;
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

        services
            .Configure<DataProtectionTokenProviderOptions>(options =>
            {
                const int LifeSpan = 2;
                options.TokenLifespan = TimeSpan.FromHours(LifeSpan);
            });

        return services;
    }


    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment env)
    {
        // The secret's presence and length are validated on startup (JwtSettings + ValidateOnStart).
        var settings = configuration
            .GetSection(nameof(JwtSettings))
            .Get<JwtSettings>()
            ?? new JwtSettings { Secret = string.Empty };

        var key = Encoding.UTF8.GetBytes(settings.Secret ?? string.Empty);

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
            .AddControllers(options =>
            {
                options.Filters.Add<ModelOrNotFoundActionFilter>();
            });

        return services;
    }

    public static IServiceCollection AddBackgroundServices(
        this IServiceCollection services)
        // Hosted services can't follow the I{ClassName} convention used by AddServices.
        => services.AddHostedService<WelcomeEmailBackgroundService>();

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
        => services.AddValidatedSettings<JwtSettings>(configuration);

    private static IServiceCollection AddEmailSettings(
        this IServiceCollection services,
        IConfiguration configuration)
        => services.Configure<EmailSettings>(
            configuration.GetSection(nameof(EmailSettings)));

    private static IServiceCollection AddAppUrlsSettings(
        this IServiceCollection services,
        IConfiguration configuration)
        => services.AddValidatedSettings<AppUrlsSettings>(configuration);

    private static IServiceCollection AddBootstrapAdminSettings(
        this IServiceCollection services,
        IConfiguration configuration)
        => services.AddValidatedSettings<BootstrapAdminSettings>(
            configuration,
            sectionName: "BootstrapAdmin");

    private static IServiceCollection AddValidatedSettings<TSettings>(
        this IServiceCollection services,
        IConfiguration configuration,
        string? sectionName = null)
        where TSettings : class
    {
        services
            .AddOptions<TSettings>()
            .Bind(configuration.GetSection(sectionName ?? typeof(TSettings).Name))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }
}
