using BookHub.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

var builderEnvIsNotTesting = !builder
    .Environment
    .IsEnvironment("Testing");

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
    .AddRateLimiting(builder.Environment);

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
var canceltationToken = app.Lifetime.ApplicationStopping;

var appEnvIsDev = app.Environment.IsDevelopment();
var appEnvIsNotTesting = !app
    .Environment
    .IsEnvironment("Testing");

if (appEnvIsDev)
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

app
    .UseRouting()
    .UseStaticFiles();

if (appEnvIsNotTesting)
{
    app.UseAllowedCors();
}

app
    .UseAuthentication()
    .UseRateLimiter()
    .UseAuthorization()
    .UseAppEndpoints();

if (appEnvIsDev)
{
    app.UseSwaggerUI();

    await app.UseMigrations(canceltationToken);
    await app.UseBuiltInUser(canceltationToken);
    await app.UseDevAdminRole();
}

await app.RunAsync(canceltationToken);
