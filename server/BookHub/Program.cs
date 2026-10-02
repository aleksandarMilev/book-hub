using BookHub.Infrastructure.Extensions;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

var builderEnvIsNotTesting = !builder.Environment.IsEnvironment("Testing");

builder
    .Services
    .AddHttpContextAccessor()
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

if (envIsDev)
{
    app.UseDeveloperExceptionPage();
}
else 
{
    app
        .UseHsts()
        .UseHttpsRedirection();

    await app.UseCustomForwardedHeaders();
}

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
    app.UseSwaggerUI();

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
