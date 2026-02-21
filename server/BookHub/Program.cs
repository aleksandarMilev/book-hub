using BookHub.Infrastructure.Extensions;

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

await app.RunAsync();
