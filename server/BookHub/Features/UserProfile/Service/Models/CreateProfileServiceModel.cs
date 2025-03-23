namespace BookHub.Features.UserProfile.Service.Models
{
    public class CreateProfileServiceModel
    {
        public string FirstName { get; init; } = null!;

        public string LastName { get; init; } = null!;

        public string ImageUrl { get; init; } = null!;

        public string PhoneNumber { get; init; } = null!;

        public string DateOfBirth { get; init; } = null!;

        public string? SocialMediaUrl { get; init; }

        public string? Biography { get; init; }

        public bool IsPrivate { get; init; }
    }
}
