namespace BookHub.Server.Infrastructure.Extensions
{
    public static class ConfigurationExtensions
    {
        public static string GetDefaultConnectionString(this IConfiguration configuration)
        {
            return configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("The connection string 'DefaultConnection' is not found!");
        }
    }
}
