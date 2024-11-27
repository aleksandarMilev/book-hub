namespace BookHub.Server.Features.Article.Service
{
    using Models;

    public interface IArticleService
    {
        Task<ArticleDetailsServiceModel?> DetailsAsync(int id);

        Task<int> CreateAsync(CreateArticleServiceModel model);
    }
}
