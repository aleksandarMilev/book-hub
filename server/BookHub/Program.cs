using BookHub.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAppSettings(builder.Configuration)
    .AddDatabase(builder.Configuration)
    .AddIdentity()
    .AddJwtAuthentication(builder.Configuration)
    .AddApiControllers()
    .AddServices()
    .AddAutoMapper()
    .AddSwagger();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}

var app = builder.Build();

var envIsDev = app.Environment.IsDevelopment();
if (envIsDev)
{
    app.UseDeveloperExceptionPage();
}

app
    .UseRouting()
    .UseStaticFiles()
    .UseAllowedCors()
    .UseAuthentication()
    .UseAuthorization()
    .UseAppEndpoints()
    .UseSwaggerUI();

if (envIsDev)
{
    await app.UseMigrations();
    await app.UseAdminRole();
}

var logger = app.Logger;
var url = Environment.GetEnvironmentVariable("DOTNET_URLS")
    ?? Environment.GetEnvironmentVariable("ASPNETCORE_URLS");

logger.LogInformation("Server is listening on: {Url}", url);

await app.RunAsync();