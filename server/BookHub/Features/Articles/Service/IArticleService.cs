namespace BookHub.Features.Article.Service;

using BookHub.Infrastructure.Services.Result;
using Infrastructure.Services;
using Infrastructure.Services.ServiceLifetimes;
using Models;

public interface IArticleService : ITransientService
{
    Task<ArticleDetailsServiceModel?> Details(
        Guid id,
        bool isEditMode = false,
        CancellationToken token = default);

    Task<ArticleDetailsServiceModel> Create(
        CreateArticleServiceModel model,
        CancellationToken token = default);

    Task<Result> Edit(
        Guid id,
        CreateArticleServiceModel model,
        CancellationToken token = default);

    Task<Result> Delete(
        Guid id,
        CancellationToken token = default);
}
