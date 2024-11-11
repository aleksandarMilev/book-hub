namespace BookHub.Server.Features.Books.Service.Models
{
    public class BookDetailsServiceModel : BookListServiceModel
    {
        public int RatingsCount { get; init; }
        public string LongDescription { get; init; } = null!;

        public int AuthorId { get; init; }

        public string? CreatorId { get; init; }


        //public ICollection<Review> Reviews { get; } = new HashSet<Review>();
    }
}
