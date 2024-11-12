namespace BookHub.Server.Features.Books.Service.Models
{
    using Authors.Service.Models;

    public class BookDetailsServiceModel : BookListServiceModel
    {
        public int RatingsCount { get; init; }

        public string LongDescription { get; init; } = null!;

        public string? CreatorId { get; init; }

        public AuthorServiceModel Author { get; init; } = null!;
    }
}
