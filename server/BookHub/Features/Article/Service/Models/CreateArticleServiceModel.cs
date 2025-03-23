namespace BookHub.Features.Article.Service.Models
{
    public class CreateArticleServiceModel
    {
        public string Title { get; init; } = null!;

        public string Introduction { get; init; } = null!;

        public string Content { get; init; } = null!;

        public string? ImageUrl { get; set; }
    }
}
