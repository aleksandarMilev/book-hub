namespace BookHub.Features.Authors.Service.Models
{
    using Book.Service.Models;

    public class AuthorDetailsServiceModel : AuthorServiceModel
    {
        public string? PenName { get; init; }

        public NationalityServiceModel Nationality { get; init; } = null!;

        public string Gender { get; init; } = null!;

        public string? BornAt { get; init; }

        public string? DiedAt { get; init; }

        public string? CreatorId { get; set; }

        public bool IsApproved { get; init; }

        public ICollection<BookServiceModel> TopBooks { get; set; } = new List<BookServiceModel>();
    }
}
