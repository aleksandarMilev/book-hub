namespace BookHub.Server.Features.Books.Models
{
    public class BookDetailsServiceModel : BookListServiceModel
    {
        public string Description { get; init; } = null!;
    }
}
