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
    .AddMemoryCache()
    .AddRateLimiting(builder.Environment);

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
    .UseStaticFiles()
    .Use(async (ctx, next) =>
    {
        var rip = ctx.Connection.RemoteIpAddress?.ToString() ?? "null";
        var xff = ctx.Request.Headers["X-Forwarded-For"].ToString();
        var xfp = ctx.Request.Headers["X-Forwarded-Proto"].ToString();

        app.Logger.LogInformation("IP check: RemoteIp={RemoteIp} XFF={XFF} XFP={XFP}", rip, xff, xfp);
        await next();
    });

if (!app.Environment.IsEnvironment("Testing"))
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
    await app.UseDevAdminRole();
    await app.UseBuiltInUser();
}

await app.RunAsync();
