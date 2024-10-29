namespace BookHub.Server.Features.Identity
{
    public interface IIdentityService
    {
        string GenerateJwtToken(string appSettingsSecret, string userId, string username);
    }
}
