namespace BookHub.Features.Reviews.Service;

using BookHub.Common;
using Infrastructure.Services.Result;
using Infrastructure.Services.ServiceLifetimes;
using Models;

public interface IReviewService : ITransientService
{
    Task<PaginatedModel<ReviewServiceModel>> AllForBook(
        Guid bookId,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<ReviewServiceModel?> Details(
        Guid reviewId,
        CancellationToken cancellationToken = default);

    Task<ResultWith<ReviewServiceModel>> Create(
        CreateReviewServiceModel model,
        CancellationToken cancellationToken = default);

    Task<Result> Edit(
        Guid reviewId,
        CreateReviewServiceModel model,
        CancellationToken cancellationToken = default);

    Task<Result> Delete(
        Guid reviewId,
        CancellationToken cancellationToken = default);
}
