namespace BookHub.Server.Features.Identity.Service
{
    public interface IIdentityService
    {
        string GenerateJwtToken(
            string appSettingsSecret,
            string userId,
            string username,
            string email,
            bool isAdmin = false);
    }
}
