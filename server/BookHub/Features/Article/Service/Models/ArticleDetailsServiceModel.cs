namespace BookHub.Features.Article.Service.Models
{
    public class ArticleDetailsServiceModel : ArticleServiceModel
    {
        public Guid Id { get; init; }

        public int Views { get; init; }

        public string ImagePath { get; init; } = null!;

        public DateTime CreatedOn { get; init; }

        public DateTime? ModifiedOn { get; init; }
    }
}
