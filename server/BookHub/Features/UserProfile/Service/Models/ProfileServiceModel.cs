namespace BookHub.Features.UserProfile.Service.Models
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

        public int ReadBooksCount { get; set; }

        public int ToReadBooksCount { get; set; }

        public int CurrentlyReadingBooksCount { get; set; }
    }
}
