namespace BookHub.Features.Articles.Service;

using Infrastructure.Services.Result;
using Infrastructure.Services.ServiceLifetimes;
using Models;

public interface IArticleService : ITransientService
{
    Task<ArticleDetailsServiceModel?> Details(
        Guid articleId,
        bool isEditMode = false,
        CancellationToken cancelationToken = default);

    Task<ArticleDetailsServiceModel> Create(
        CreateArticleServiceModel model,
        CancellationToken cancelationToken = default);

    Task<Result> Edit(
        Guid articleId,
        CreateArticleServiceModel model,
        CancellationToken cancelationToken = default);

    Task<Result> Delete(
        Guid articleId,
        CancellationToken cancelationToken = default);
}
