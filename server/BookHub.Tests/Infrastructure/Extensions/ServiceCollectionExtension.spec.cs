namespace BookHub.Tests.Infrastructure.Extensions;

using BookHub.Data;
using BookHub.Infrastructure.Extensions;
using BookHub.Infrastructure.Services.CurrentUser;
using BookHub.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Moq;
using Swashbuckle.AspNetCore.Swagger;
using Xunit;

using static BookHub.Common.Constants.Cors;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddCorsPolicy_InDevelopment_AllowsAnyOriginHeaderMethod()
    {
        var services = new ServiceCollection();
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Cors:AllowedOrigins"] = "http://example.com"
            })
            .Build();

        var env = new Mock<IWebHostEnvironment>();
        env.SetupAllProperties();
        env.Object.EnvironmentName = Environments.Development;

        services.AddCorsPolicy(config, env.Object);

        var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<CorsOptions>>();
        var policy = options.Value.GetPolicy(CorsPolicyName);

        Assert.NotNull(policy);
        Assert.True(policy!.AllowAnyOrigin);
        Assert.True(policy.AllowAnyHeader);
        Assert.True(policy.AllowAnyMethod);
    }

    [Fact]
    public void AddCorsPolicy_InProduction_UsesConfiguredOrigins()
    {
        var services = new ServiceCollection();

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Cors:AllowedOrigins"] = "http://a.com;http://b.com"
            })
            .Build();

        var env = new Mock<IWebHostEnvironment>();
        env.SetupAllProperties();
        env.Object.EnvironmentName = Environments.Production;

        services.AddCorsPolicy(config, env.Object);

        var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<CorsOptions>>();
        var policy = options.Value.GetPolicy(CorsPolicyName);

        Assert.NotNull(policy);
        Assert.False(policy!.AllowAnyOrigin);
        Assert.True(policy.AllowAnyHeader);
        Assert.True(policy.AllowAnyMethod);
        Assert.Equal(new[] { "http://a.com", "http://b.com" }, policy.Origins);
    }

    [Fact]
    public void AddCorsPolicy_InProduction_ThrowsWhenOriginsMissing()
    {
        var services = new ServiceCollection();

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>())
            .Build();

        var env = new Mock<IWebHostEnvironment>();
        env.SetupAllProperties();
        env.Object.EnvironmentName = Environments.Production;

        services.AddCorsPolicy(config, env.Object);

        var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<CorsOptions>>();

        Assert.Throws<InvalidOperationException>(
            () => options.Value.GetPolicy(CorsPolicyName));
    }

    [Fact]
    public void AddAppSettings_BindsJwtAndEmailSettings()
    {
        var services = new ServiceCollection();

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["JwtSettings:Secret"] = "test-secret",
                ["JwtSettings:Issuer"] = "issuer",
                ["JwtSettings:Audience"] = "audience",
                ["EmailSettings:Host"] = "smtp.host",
                ["EmailSettings:Port"] = "587",
                ["EmailSettings:Username"] = "user",
                ["EmailSettings:Password"] = "pass",
                ["EmailSettings:From"] = "BookHub <book@hub.com>",
                ["EmailSettings:UseSsl"] = "true"
            })
            .Build();

        services.AddAppSettings(config);

        var provider = services.BuildServiceProvider();

        var jwtOptions = provider.GetRequiredService<IOptions<JwtSettings>>().Value;
        var emailOptions = provider.GetRequiredService<IOptions<EmailSettings>>().Value;

        Assert.Equal("test-secret", jwtOptions.Secret);
        Assert.Equal("issuer", jwtOptions.Issuer);
        Assert.Equal("audience", jwtOptions.Audience);

        Assert.Equal("smtp.host", emailOptions.Host);
        Assert.Equal(587, emailOptions.Port);
        Assert.Equal("user", emailOptions.Username);
        Assert.Equal("pass", emailOptions.Password);
        Assert.Equal("BookHub <book@hub.com>", emailOptions.From);
        Assert.True(emailOptions.UseSsl);
    }

    [Fact]
    public void AddDatabase_RegistersBookHubDbContext()
    {
        var services = new ServiceCollection();

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = "Server=(localdb)\\mssqllocaldb;Database=BookHubTest;Trusted_Connection=True;Encrypt=False"
            })
            .Build();

        services.AddDatabase(config);
        services.AddScoped<ICurrentUserService, FakeCurrentUserService>();

        var provider = services.BuildServiceProvider();
        var context = provider.GetRequiredService<BookHubDbContext>();

        Assert.NotNull(context);
    }

    [Fact]
    public void AddIdentity_ConfiguresPasswordOptions_ForDevelopment()
    {
        var services = new ServiceCollection();
        services.AddDbContext<BookHubDbContext>(opt => opt.UseInMemoryDatabase("IdentityDev"));
        services.AddScoped<ICurrentUserService, FakeCurrentUserService>();

        var env = new Mock<IWebHostEnvironment>();
        env.SetupAllProperties();
        env.Object.EnvironmentName = Environments.Development;

        services.AddIdentity(env.Object);

        var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<IdentityOptions>>().Value;

        Assert.True(options.User.RequireUniqueEmail);
        Assert.Equal(TimeSpan.FromMinutes(Features.Identity.Shared.Constants.AccountLockoutTimeSpan), options.Lockout.DefaultLockoutTimeSpan);
        Assert.Equal(Features.Identity.Shared.Constants.MaxFailedLoginAttempts, options.Lockout.MaxFailedAccessAttempts);

        Assert.False(options.Password.RequireDigit);
        Assert.False(options.Password.RequireLowercase);
        Assert.False(options.Password.RequireUppercase);
        Assert.False(options.Password.RequireNonAlphanumeric);
        Assert.Equal(6, options.Password.RequiredLength);
    }

    [Fact]
    public void AddIdentity_ConfiguresPasswordOptions_ForProduction()
    {
        var services = new ServiceCollection();
        services.AddDbContext<BookHubDbContext>(opt => opt.UseInMemoryDatabase("IdentityProd"));
        services.AddScoped<ICurrentUserService, FakeCurrentUserService>();

        var env = new Mock<IWebHostEnvironment>();
        env.SetupAllProperties();
        env.Object.EnvironmentName = Environments.Production;

        services.AddIdentity(env.Object);

        var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<IdentityOptions>>().Value;

        Assert.True(options.User.RequireUniqueEmail);
        Assert.Equal(TimeSpan.FromMinutes(Features.Identity.Shared.Constants.AccountLockoutTimeSpan), options.Lockout.DefaultLockoutTimeSpan);
        Assert.Equal(Features.Identity.Shared.Constants.MaxFailedLoginAttempts, options.Lockout.MaxFailedAccessAttempts);

        Assert.True(options.Password.RequireDigit);
        Assert.True(options.Password.RequireLowercase);
        Assert.True(options.Password.RequireUppercase);
        Assert.False(options.Password.RequireNonAlphanumeric);
        Assert.Equal(8, options.Password.RequiredLength);
    }

    [Fact]
    public void AddJwtAuthentication_ConfiguresDevJwtBearerOptions()
    {
        var services = new ServiceCollection();

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["JwtSettings:Secret"] = "a_very_long_test_secret_key_123456",
                ["JwtSettings:Issuer"] = "issuer",
                ["JwtSettings:Audience"] = "audience"
            })
            .Build();

        var env = new Mock<IWebHostEnvironment>();
        env.SetupAllProperties();
        env.Object.EnvironmentName = Environments.Development;

        services.AddLogging();
        services.AddJwtAuthentication(config, env.Object);

        var provider = services.BuildServiceProvider();

        var authOptions = provider.GetRequiredService<IOptions<AuthenticationOptions>>().Value;
        Assert.Equal(JwtBearerDefaults.AuthenticationScheme, authOptions.DefaultAuthenticateScheme);
        Assert.Equal(JwtBearerDefaults.AuthenticationScheme, authOptions.DefaultChallengeScheme);
        Assert.Equal(JwtBearerDefaults.AuthenticationScheme, authOptions.DefaultScheme);

        var jwtOptionsMonitor = provider.GetRequiredService<IOptionsMonitor<JwtBearerOptions>>();
        var jwtOptions = jwtOptionsMonitor.Get(JwtBearerDefaults.AuthenticationScheme);

        Assert.False(jwtOptions.RequireHttpsMetadata);
        Assert.True(jwtOptions.IncludeErrorDetails);

        Assert.False(jwtOptions.TokenValidationParameters.ValidateIssuer);
        Assert.False(jwtOptions.TokenValidationParameters.ValidateAudience);
        Assert.True(jwtOptions.TokenValidationParameters.ValidateIssuerSigningKey);
        Assert.True(jwtOptions.TokenValidationParameters.ValidateLifetime);
    }

    [Fact]
    public void AddJwtAuthentication_ConfiguresProdJwtBearerOptions()
    {
        var services = new ServiceCollection();

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["JwtSettings:Secret"] = "a_very_long_test_secret_key_123456",
                ["JwtSettings:Issuer"] = "issuer-prod",
                ["JwtSettings:Audience"] = "audience-prod"
            })
            .Build();

        var env = new Mock<IWebHostEnvironment>();
        env.SetupAllProperties();
        env.Object.EnvironmentName = Environments.Production;

        services.AddLogging();
        services.AddJwtAuthentication(config, env.Object);

        var provider = services.BuildServiceProvider();
        var jwtOptionsMonitor = provider.GetRequiredService<IOptionsMonitor<JwtBearerOptions>>();
        var jwtOptions = jwtOptionsMonitor.Get(JwtBearerDefaults.AuthenticationScheme);

        Assert.True(jwtOptions.RequireHttpsMetadata);
        Assert.True(jwtOptions.TokenValidationParameters.ValidateIssuerSigningKey);
        Assert.True(jwtOptions.TokenValidationParameters.ValidateIssuer);
        Assert.True(jwtOptions.TokenValidationParameters.ValidateAudience);
        Assert.True(jwtOptions.TokenValidationParameters.ValidateLifetime);
        Assert.Equal("issuer-prod", jwtOptions.TokenValidationParameters.ValidIssuer);
        Assert.Equal("audience-prod", jwtOptions.TokenValidationParameters.ValidAudience);
        Assert.Equal(TimeSpan.FromMinutes(2), jwtOptions.TokenValidationParameters.ClockSkew);
    }

    [Fact]
    public void AddJwtAuthentication_ThrowsWhenJwtSettingsMissing()
    {
        var services = new ServiceCollection();

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>())
            .Build();

        var env = new Mock<IWebHostEnvironment>();
        env.SetupAllProperties();
        env.Object.EnvironmentName = Environments.Development;

        Assert.Throws<InvalidOperationException>(() => services.AddJwtAuthentication(config, env.Object));
    }

    [Fact]
    public void AddSwagger_RegistersSwaggerProvider()
    {
        var services = new ServiceCollection();

        services.AddSwagger();

        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ISwaggerProvider));

        Assert.NotNull(descriptor);
    }

    [Fact]
    public void AddServices_RegistersCurrentUserService_AsScoped()
    {
        var services = new ServiceCollection();

        services.AddServices();

        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ICurrentUserService));

        Assert.NotNull(descriptor);
        Assert.Equal(ServiceLifetime.Scoped, descriptor!.Lifetime);
        Assert.Equal(typeof(CurrentUserService), descriptor.ImplementationType);
    }

    private sealed class FakeCurrentUserService : ICurrentUserService
    {
        public string? GetUsername() => null;
        public string? GetId() => null;
        public bool IsAdmin() => false;
    }
}
