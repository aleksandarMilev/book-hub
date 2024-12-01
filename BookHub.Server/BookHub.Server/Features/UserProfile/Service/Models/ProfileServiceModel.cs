namespace BookHub.Server.Features.UserProfile.Service.Models
{
    public class ProfileServiceModel : PrivateProfileServiceModel, IProfileServiceModel
    {
        public string PhoneNumber { get; init; } = null!;

        public DateTime DateOfBirth { get; init; }

        public string? SocialMediaUrl { get; init; }

        public string? Biography { get; init; }

        public int CreatedBooksCount { get; set; }

        public int CreatedAuthorsCount { get; set; }

        public int ReviewsCount { get; set; }
    }
}
