namespace BookHub.Features.Article.Service.Models
{
    public class ArticleDetailsServiceModel : CreateArticleServiceModel
    {
        public Guid Id { get; init; }

        public DateTime CreatedOn { get; init; }

        public int Views { get; init; }
    }
}
