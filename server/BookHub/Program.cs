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

await app.RunAsync();
