namespace BookHub.Server.Features.Identity.Web.Models
{
    public class LoginResponseModel(
        string token,
        bool hasProfile = false)
    {
        public string Token { get; init; } = token;

        public bool HasProfile { get; init; } = hasProfile;
    }
}
