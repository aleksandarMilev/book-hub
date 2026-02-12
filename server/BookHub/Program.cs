using BookHub.Infrastructure.Extensions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

var builder = WebApplication.CreateBuilder(args);

builder.Services
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
    .AddMemoryCache();

if (!builder.Environment.IsEnvironment("Testing"))
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
var envIsProd = app.Environment.IsProduction();
if (envIsDev)
{
    app.UseDeveloperExceptionPage();
}
else 
{
    app
        .UseHsts()
        .UseHttpsRedirection();
}

app
    .UseRouting()
    .UseStaticFiles();

if (!app.Environment.IsEnvironment("Testing"))
{
    app.UseAllowedCors();
}

app
    .UseAuthentication()
    .UseAuthorization()
    .UseAppEndpoints();

if (envIsDev)
{
    app.UseSwaggerUI();
    await app.UseMigrations();
    await app.UseDevAdminRole();
}
else if (envIsProd)
{
    await app.useProductionAdminRole();
}

var logger = app.Logger;
var url = Environment.GetEnvironmentVariable("DOTNET_URLS")
    ?? Environment.GetEnvironmentVariable("ASPNETCORE_URLS");

logger.LogInformation(
    "Server is listening on: {Url}. Environment: {env}",
    url,
    app.Environment.ToString());

await app.RunAsync();
