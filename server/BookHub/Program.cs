using BookHub.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddHttpContextAccessor()
    .AddCorsPolicy(
        builder.Configuration,
        builder.Environment)
    .AddAppSettings(builder.Configuration)
    .AddDatabase(builder.Configuration)
    .AddIdentity(builder.Environment)
    .AddJwtAuthentication(
        builder.Configuration,
        builder.Environment)
    .AddApiControllers()
    .AddServices()
    .AddSwagger()
    .AddHealthcheck();

var app = builder.Build();

var envIsDev = app.Environment.IsDevelopment();
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
    .UseStaticFiles()
    .UseAllowedCors()
    .UseAuthentication()
    .UseAuthorization()
    .UseAppEndpoints();

if (envIsDev)
{
    app.UseSwaggerUI();
    await app.UseMigrations();
    await app.UseAdminRole();
}

var logger = app.Logger;
var url = Environment.GetEnvironmentVariable("DOTNET_URLS")
    ?? Environment.GetEnvironmentVariable("ASPNETCORE_URLS");

logger.LogInformation(
    "Server is listening on: {Url}. Environment: {env}",
    url,
    app.Environment.ToString());

await app.RunAsync();