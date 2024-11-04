namespace BookHub.Server.Features.Books.Service.Models
{
    public class BookDetailsServiceModel : BookListServiceModel
    {
        public string Description { get; init; } = null!;
    }
}
