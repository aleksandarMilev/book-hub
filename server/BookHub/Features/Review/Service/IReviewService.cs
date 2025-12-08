namespace BookHub.Features.Review.Service;

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
        CancellationToken token = default);

    Task<ReviewServiceModel?> Details(
        Guid id,
        CancellationToken token = default);

    Task<ResultWith<ReviewServiceModel>> Create(
        CreateReviewServiceModel model,
        CancellationToken token = default);

    Task<Result> Edit(
        Guid id,
        CreateReviewServiceModel model,
        CancellationToken token = default);

    Task<Result> Delete(
        Guid id,
        CancellationToken token = default);
}
