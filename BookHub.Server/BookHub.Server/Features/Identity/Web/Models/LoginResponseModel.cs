namespace BookHub.Server.Features.Identity.Web.Models
{
    public class LoginResponseModel(string username, string email, string userId, string token)
    {
        public string Username { get; init; } = username;

        public string Email { get; init; } = email;

        public string UserId { get; init; } = userId;

        public string Token { get; init; } = token;
    }
}
