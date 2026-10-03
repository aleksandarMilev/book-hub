namespace BookHub.Features.ReadingLists.Service;

using BookHub.Common;
using Books.Service.Models;
using Infrastructure.Services.Result;
using Infrastructure.Services.ServiceLifetimes;
using Service.Models;
using Shared;

public interface IReadingListService : ITransientService
{
    /// <summary>
    /// Returns null when the caller may not see the user's lists (private or missing profile).
    /// </summary>
    Task<ResultWith<PaginatedModel<BookServiceModel>>> All(
       string userId,
       ReadingListStatus status,
       int pageIndex,
       int pageSize,
       CancellationToken cancellationToken);

    /// <summary>
    /// Returns null when there is no such book, or when the caller may not see the user's lists.
    /// </summary>
    Task<BookServiceModel?> LastCurrentlyReading(
        string userId,
        CancellationToken cancellationToken);

    Task<Result> Add(
        ReadingListServiceModel model,
        CancellationToken cancellationToken);

    Task<Result> Delete(
        ReadingListServiceModel model,
        CancellationToken cancellationToken);
}
