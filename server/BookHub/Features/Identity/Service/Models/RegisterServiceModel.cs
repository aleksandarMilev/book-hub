namespace BookHub.Features.Identity.Service.Models;

public class RegisterServiceModel
{
    public string Username { get; init; } = default!;

    public string Email { get; init; } = default!;

    public string Password { get; init; } = default!;

    public string FirstName { get; init; } = default!;

    public string LastName { get; init; } = default!;

    public IFormFile? Image { get; init; }

    public DateTime DateOfBirth { get; init; }

    public string? SocialMediaUrl { get; init; }

    public string? Biography { get; init; }

    public bool IsPrivate { get; init; }
}
