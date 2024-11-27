namespace BookHub.Server.Features.Search.Service.Models
{
    public class SearchArticleServiceModel
    {
        public int Id { get; init; }

        public string Title { get; init; } = null!;

        public string Introduction { get; init; } = null!;

        public string? ImageUrl { get; init; }

        public DateTime CreatedOn { get; init; }
    }
}
