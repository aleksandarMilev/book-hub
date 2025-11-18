namespace BookHub.Features.Article.Service
{
    using Infrastructure.Services;
    using Infrastructure.Services.ServiceLifetimes;
    using Models;

    public interface IArticleService : ITransientService
    {
        Task<ArticleDetailsServiceModel?> Details(
            string id,
            CancellationToken token);

        Task<ArticleDetailsServiceModel> Create(
            CreateArticleServiceModel model,
            CancellationToken token);

        Task<Result> Edit(
            string id,
            CreateArticleServiceModel model,
            CancellationToken token);

        Task<Result> Delete(
            string id,
            CancellationToken token);
    }
}
