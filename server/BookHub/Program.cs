using BookHub.Infrastructure.Extensions;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

var builderEnvIsNotTesting = !builder.Environment.IsEnvironment("Testing");

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
    .AddSwagger()
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

// Fail fast on invalid settings before anything touches the database.
// ValidateOnStart alone would only run in RunAsync, after the startup tasks below.
app
    .Services
    .GetRequiredService<IStartupValidator>()
    .Validate();

var envIsDev = app.Environment.IsDevelopment();
var envIsNotTesting = !app.Environment.IsEnvironment("Testing");

// In every environment: exceptions become ProblemDetails (GlobalExceptionHandler, which logs
// them itself, so the middleware's own error log is suppressed), and empty-body error
// responses such as 401/403 get a ProblemDetails body.
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
        .UseHttpsRedirection();

    await app.UseCustomForwardedHeaders();
}

// Before authentication/authorization, so the Swagger UI and document stay reachable
// without a token.
if (envIsDev)
{
    app.UseSwaggerUI();
}

// UseStaticFiles serves wwwroot (book covers, author photos, avatars) and short-circuits
// before UseAuthorization, so uploaded images stay public under the fallback policy.
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
    await app.UseMigrations();
    await app.UseBuiltInUser();
    await app.UseDevAdminRole();
}
else if (envIsNotTesting)
{
    // No-op unless BootstrapAdmin:Enabled is true.
    await app.UseProductionAdminRole();
}

await app.RunAsync();
