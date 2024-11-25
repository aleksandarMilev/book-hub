namespace BookHub.Server.Features.UserProfile.Service.Models
{
    public class ProfileServiceModel
    {
        public string Id { get; init; } = null!;

        public string FirstName { get; init; } = null!;

        public string LastName { get; init; } = null!;

        public string ImageUrl { get; init; } = null!;

        public string PhoneNumber { get; init; } = null!;

        public DateTime DateOfBirth { get; init; }

        public string? SocialMediaUrl { get; init; }

        public string? Biography { get; init; }

        public bool IsPrivate { get; init; }
    }
}
