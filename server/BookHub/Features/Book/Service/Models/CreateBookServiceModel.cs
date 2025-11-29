namespace BookHub.Features.Book.Service.Models
{
    public class CreateBookServiceModel
    {
        public string Title { get; init; } = null!;

        public Guid AuthorId { get; init; }

        public string? ImageUrl { get; set; }

        public string ShortDescription { get; init; } = null!;

        public string LongDescription { get; init; } = null!;

        public string? PublishedDate { get; init; }

        public IEnumerable<int> Genres { get; init; } = new HashSet<int>();
    }
}
