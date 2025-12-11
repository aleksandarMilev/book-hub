namespace BookHub.Features.Identity.Service.Models;

public class LoginServiceModel
{
    public string Credentials { get; init; } = null!;

    public bool RememberMe { get; init; }

    public string Password { get; init; } = null!;
}
