namespace BookHub.Server.Infrastructure.Extensions
{
    public static class ConfigurationExtensions
    {
        private const string ConnectionStringNotFound = "The connection string 'DefaultConnection' is not found!";
        private const string AdminPasswordNotFound = "The'AdminPassword' section is not found!";
        private const string DefaultConnectionString = "DefaultConnection";
        private const string AdminPassword = "AdminPassword";

        public static string GetDefaultConnectionString(this IConfiguration configuration) 
            => configuration
                  .GetConnectionString(DefaultConnectionString)
                  ?? throw new InvalidOperationException(ConnectionStringNotFound);

        public static string GetAdminPassword(this IConfiguration configuration)
            => configuration[AdminPassword]
                  ?? throw new InvalidOperationException(AdminPasswordNotFound);
    }
}
