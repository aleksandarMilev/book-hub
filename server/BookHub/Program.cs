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

if (builderEnvIsNotTesting)
{
    builder
        .Services
        .AddCorsPolicy(
            builder.Configuration,
            builder.Environment);

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
var envIsNotTesting = !app.Environment.IsEnvironment("Testing");

app
    .UseExceptionHandler(new ExceptionHandlerOptions
    {
        SuppressDiagnosticsCallback = _ => true,
    })
    .UseStatusCodePages();

if (envIsDev)
{
    app.UseDeveloperExceptionPage();
}
else
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

if (envIsNotTesting)
{
    app.UseAllowedCors();
}

app
    .UseAuthentication()
    .UseRateLimiter()
    .UseAuthorization()
    .UseAppEndpoints();

if (envIsDev)
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    await app.UseMigrations(cancellationToken);
    await app.UseBuiltInUser();
    await app.UseDevAdminRole();
}
else if (envIsNotTesting)
{
    await app.UseProductionAdminRole();
}

await app.RunAsync(cancellationToken);
