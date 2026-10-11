namespace BookHub.Infrastructure.Extensions;

using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Settings;

public static class WebApplicationBuilderExtensions
{
    extension(WebApplicationBuilder builder)
    {
        public WebApplicationBuilder UseSerilog()
        {
            builder
                .Host
                .UseSerilog(static (context, loggerConfiguration) =>
                {
                    var seqSettings = context.Configuration
                        .GetSection(nameof(SeqSettings))
                        .Get<SeqSettings>()
                        ?? new SeqSettings();

                    if (!context.HostingEnvironment.IsDevelopment())
                    {
                        loggerConfiguration
                            .MinimumLevel.Information()
                            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                            .MinimumLevel.Override("System", LogEventLevel.Warning);
                    }

                    loggerConfiguration
                        .ReadFrom.Configuration(context.Configuration)
                        .Enrich.FromLogContext()
                        .Enrich.WithProperty("MachineName", Environment.MachineName)
                        .Enrich.WithProperty("EnvironmentName", context.HostingEnvironment.EnvironmentName)
                        .WriteTo.Console(new CompactJsonFormatter());

                    if (!string.IsNullOrWhiteSpace(seqSettings.ServerUrl))
                    {
                        loggerConfiguration.WriteTo.Seq(seqSettings.ServerUrl);
                    }
                });

            return builder;
        }
    }
}
