namespace BookHub.Features.Identity.Service.Models;

public class RegisterServiceModel
{
    public string Username { get; init; } = null!;

    public string Email { get; init; } = null!;

    public string Password { get; init; } = null!;

    public string FirstName { get; init; } = null!;

    public string LastName { get; init; } = null!;

    public IFormFile? Image { get; init; }

    public string? DateOfBirth { get; init; }

    public string? SocialMediaUrl { get; init; }

    public string? Biography { get; init; }

    public bool IsPrivate { get; init; }
}
