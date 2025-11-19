namespace BookHub.Features.Article.Service
{
    using Infrastructure.Services;
    using Infrastructure.Services.ServiceLifetimes;
    using Models;

    public interface IArticleService : ITransientService
    {
        Task<ArticleDetailsServiceModel?> Details(
            Guid id,
            CancellationToken token);

        Task<ArticleDetailsServiceModel> Create(
            CreateArticleServiceModel model,
            CancellationToken token);

        Task<Result> Edit(
            Guid id,
            CreateArticleServiceModel model,
            CancellationToken token);

        Task<Result> Delete(
            Guid id,
            CancellationToken token);
    }
}
