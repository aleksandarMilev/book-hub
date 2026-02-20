namespace BookHub.Features.Identity.Service.Models;

public class ResetPasswordServiceModel
{
    public string Email { get; init; } = default!;

    public string Token { get; init; } = default!;

    public string NewPassword { get; init; } = default!;
}