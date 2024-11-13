namespace BookHub.Server.Features.Books.Service.Models
{
    public class CreateBookServiceModel
    {
        public string Title { get; init; } = null!;

        public string ShortDescription { get; init; } = null!;

        public string LongDescription { get; init; } = null!;

        public string ImageUrl { get; init; } = null!;

        public string? AuthorName { get; init; }

        public string CreatorId { get; set; } = null!;
    }
}
