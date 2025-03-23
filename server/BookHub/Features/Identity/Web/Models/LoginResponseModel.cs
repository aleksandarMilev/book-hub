namespace BookHub.Features.Identity.Web.Models
{
    public class LoginResponseModel(string token)
    {
        public string Token { get; init; } = token;
    }
}
