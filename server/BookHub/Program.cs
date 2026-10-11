using BookHub.Infrastructure.Extensions;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication
    .CreateBuilder(args)
    .UseSerilog();

var builderEnvIsNotTesting = !builder
    .Environment
    .IsEnvironment("Testing");

builder
    .Services
    .AddHttpContextAccessor()
    .AddErrorHandling()
    .AddAppSettings(builder.Configuration)
    .AddIdentity(builder.Environment)
    .AddJwtAuthentication(
        builder.Configuration,
        builder.Environment)
    .AddApiControllers()
    .AddServices()
    .AddOpenApi()
    .AddHealthcheck()
    .AddMemoryCache()
    .AddRateLimiting(builder.Environment)
    .AddBackgroundServices();

builder
    .Services
    .AddCorsPolicy(
        builder.Configuration,
        builder.Environment);

// The test host registers the same DbContext through AddDatabase(Func<string?>), pointed at
// its own test database.
if (builderEnvIsNotTesting)
{
    builder
        .Services
        .AddDatabase(builder.Configuration);
}

var app = builder.Build();

var cancellationToken = app
    .Lifetime
    .ApplicationStopping;

app
    .Services
    .GetRequiredService<IStartupValidator>()
    .Validate();

var envIsDev = app.Environment.IsDevelopment();

app
    .UseExceptionHandler(new ExceptionHandlerOptions
    {
        SuppressDiagnosticsCallback = _ => true,
    })
    .UseStatusCodePages();

if (!envIsDev)
{
    app
        .UseHsts()
        .UseHttpsRedirection()
        .UseCustomForwardedHeaders();
}

app.UseSerilogRequestLogging();

app
    .UseRouting()
    .UseStaticFiles();

app.UseAllowedCors();

app
    .UseAuthentication()
    .UseRateLimiter()
    .UseAuthorization()
    .UseAppEndpoints();

if (envIsDev)
{
    // Public like the Swagger UI was: the fallback policy would otherwise require a token.
    app.MapOpenApi().AllowAnonymous();
    app.MapScalarApiReference().AllowAnonymous();

    await app.UseMigrations(cancellationToken);
    await app.UseBuiltInUser();
    await app.UseDevAdminRole();
}
else
{
    await app.UseProductionAdminRole();
}

await app.RunAsync(cancellationToken);
