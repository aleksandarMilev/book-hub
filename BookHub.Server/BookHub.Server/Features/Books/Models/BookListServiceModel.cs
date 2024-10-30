namespace BookHub.Server.Features.Books.Models
{
    public class BookListServiceModel
    {
        public int Id { get; init; }

        public string Title { get; init; } = null!;

        public string Author { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;
    }
}
