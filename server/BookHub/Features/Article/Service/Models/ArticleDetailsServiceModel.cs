namespace BookHub.Features.Article.Service.Models
{
    public class ArticleDetailsServiceModel : CreateArticleServiceModel
    {
        public string Id { get; init; } = null!;

        public DateTime CreatedOn { get; init; }

        public int Views { get; init; }
    }
}
