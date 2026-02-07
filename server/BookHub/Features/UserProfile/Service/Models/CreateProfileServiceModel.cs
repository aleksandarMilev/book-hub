namespace BookHub.Features.UserProfile.Service.Models;

using Infrastructure.Services.ImageWriter.Models;

public class CreateProfileServiceModel : IImageServiceModel
{
    public string FirstName { get; init; } = null!;

    public string LastName { get; init; } = null!;

    public IFormFile? Image { get; init; }

    public string? DateOfBirth { get; init; }

    public string? SocialMediaUrl { get; init; }

    public string? Biography { get; init; }

    public bool IsPrivate { get; init; }

    public bool RemoveImage { get; init; }
}
