namespace BookHub.Features.Identity.Service.Models;

public class LoginResponseModel(string token)
{
    public string Token { get; init; } = token;
}
