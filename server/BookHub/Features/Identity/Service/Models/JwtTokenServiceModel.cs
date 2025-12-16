namespace BookHub.Features.Identity.Service.Models;

public class JwtTokenServiceModel(string token)
{
    public string Token { get; init; } = token;
}
