namespace BookHub.Server.Features.Article.Service
{
    using Models;

    public interface IArticleService
    {
        Task<int> CreateAsync(CreateArticleServiceModel model);
    }
}
