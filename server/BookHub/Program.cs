using BookHub.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

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
    await app.UseBuiltInUser();
}

await app.RunAsync();
