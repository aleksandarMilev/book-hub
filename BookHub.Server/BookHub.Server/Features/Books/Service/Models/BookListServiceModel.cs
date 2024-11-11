namespace BookHub.Server.Features.Books.Service.Models
{
    public class BookListServiceModel
    {
        public int Id { get; init; }

        public string Title { get; init; } = null!;

        public string AuthorName { get; init; } = null!;

        public string ImageUrl { get; init; } = null!;

        public string ShortDescription { get; init; } = null!;

        public double AverageRating { get; init; } 

        public ICollection<string> Genres { get; init; } = new HashSet<string>();
    }
}
