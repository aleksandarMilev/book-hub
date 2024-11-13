namespace BookHub.Server.Features.Authors.Service.Models
{
    using Books.Service.Models;

    public class AuthorDetailsServiceModel : AuthorServiceModel
    {
        public string? PenName { get; init; }

        public string Nationality { get; init; } = null!;

        public string Gender { get; init; } = null!;

        public string? BornAt { get; init; }

        public string? DiedAt { get; init; }

        public string? CreatorId { get; set; }

        public ICollection<BookListServiceModel> TopBooks { get; set; } = new List<BookListServiceModel>();
    }
}
