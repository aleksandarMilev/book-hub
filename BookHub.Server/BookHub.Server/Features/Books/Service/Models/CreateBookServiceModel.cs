namespace BookHub.Server.Features.Books.Service.Models
{
    public class CreateBookServiceModel
    {
        public string Title { get; init; } = null!;

        public string Author { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

        public string Description { get; set; } = null!;
    }
}
