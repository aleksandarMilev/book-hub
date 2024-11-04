namespace BookHub.Server.Features.Identity.Web.Models
{
    public class LoginResponseModel
    {
        public LoginResponseModel() { }

        public LoginResponseModel(string username, string email, string userId, string token)
        {
            this.Username = username;
            this.Email = email;
            this.UserId = userId;
            this.Token = token;
        }

        public string Username { get; init; } = null!;

        public string Email { get; init; } = null!;

        public string UserId { get; init; } = null!;

        public string Token { get; init; } = null!;
    }
}
