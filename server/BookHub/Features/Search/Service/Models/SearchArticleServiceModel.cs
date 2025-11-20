namespace BookHub.Features.Search.Service.Models
{
    public class SearchArticleServiceModel
    {
        public string Id { get; init; } = null!;

        public string Title { get; init; } = null!;

        public string Introduction { get; init; } = null!;

        public string? ImagePath { get; init; }

        public DateTime CreatedOn { get; init; }
    }
}
