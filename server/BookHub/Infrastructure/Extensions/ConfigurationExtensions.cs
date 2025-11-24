namespace BookHub.Infrastructure.Extensions
{
    public static class ConfigurationExtensions
    {
        public static string GetAppSettings(
            this IConfiguration configuration)
            => configuration
                .GetSection(nameof(ApplicationSettings))
                .GetValue<string>(nameof(ApplicationSettings.Secret))!;
    }
}
