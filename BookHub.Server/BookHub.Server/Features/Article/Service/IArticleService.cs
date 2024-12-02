namespace BookHub.Server.Features.Article.Service
{
    using Infrastructure.Services;
    using Infrastructure.Services.ServiceLifetimes;
    using Models;

    public interface IArticleService : ITransientService
    {
        Task<ArticleDetailsServiceModel?> DetailsAsync(int id);

        Task<int> CreateAsync(CreateArticleServiceModel model);

        Task<Result> EditAsync(int id, CreateArticleServiceModel model);

        Task<Result> DeleteAsync(int id);
    }
}
