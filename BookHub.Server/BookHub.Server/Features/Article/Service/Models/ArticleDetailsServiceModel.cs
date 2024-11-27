namespace BookHub.Server.Features.Article.Service.Models
{
    public class ArticleDetailsServiceModel : CreateArticleServiceModel
    {
        public int Id { get; init; }

        public DateTime CreatedOn { get; init; }

        public int Views { get; init; }
    }
}
