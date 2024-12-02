namespace BookHub.Server.Features.Identity.Service
{
    using Infrastructure.Services.ServiceLifetimes;

    public interface IIdentityService : ITransientService
    {
        string GenerateJwtToken(
            string appSettingsSecret,
            string userId,
            string username,
            string email,
            bool isAdmin = false);
    }
}
