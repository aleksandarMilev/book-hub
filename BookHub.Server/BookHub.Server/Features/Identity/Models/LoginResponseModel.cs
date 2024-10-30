namespace BookHub.Server.Features.Identity.Models
{
    public class LoginResponseModel
    {
        public LoginResponseModel(string token) => Token = token;

        public string Token { get; init; } = null!;
    }
}
