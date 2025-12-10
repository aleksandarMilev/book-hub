namespace BookHub.Features.UserProfile.Service.Models;

using Infrastructure.Services.ImageWriter.Models.Image;

public class CreateProfileServiceModel : IImageServiceModel
{
    public string FirstName { get; init; } = null!;

    public string LastName { get; init; } = null!;

    public IFormFile? Image { get; init; }

    public string PhoneNumber { get; init; } = null!;

    public string? DateOfBirth { get; init; } = null!;

    public string? SocialMediaUrl { get; init; }

    public string? Biography { get; init; }

    public bool IsPrivate { get; init; }
}
