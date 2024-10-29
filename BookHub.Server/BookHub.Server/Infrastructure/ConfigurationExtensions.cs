namespace BookHub.Server.Infrastructure
{
    public static class ConfigurationExtensions
    {
        public static string GetDefaultConnectionString(this IConfiguration configuration)
        {
            return configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("The connection string 'DefaultConnection' is not found!");
        }

        public static AppSettings GetAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsConfiguration = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsConfiguration);

            return appSettingsConfiguration.Get<AppSettings>()
                ?? throw new InvalidOperationException("AppSettings not found.");
        }
    }
}
