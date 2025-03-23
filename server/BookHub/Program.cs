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

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app
    .UseRouting()
    .UseAllowedCors()
    .UseAuthentication()
    .UseAuthorization()
    .UseAppEndpoints()
    .UseSwaggerUI()
    .UseMigrations()
    .UseAdminRole(builder.Configuration);

await app.RunAsync();
