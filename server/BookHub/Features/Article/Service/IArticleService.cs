namespace BookHub.Features.Article.Service
{
    using Infrastructure.Services;
    using Infrastructure.Services.ServiceLifetimes;
    using Models;

    public interface IArticleService : ITransientService
    {
        Task<ArticleDetailsServiceModel?> Details(int id);

        Task<int> Create(CreateArticleServiceModel model);

        Task<Result> Edit(int id, CreateArticleServiceModel model);

        Task<Result> Delete(int id);
    }
}
