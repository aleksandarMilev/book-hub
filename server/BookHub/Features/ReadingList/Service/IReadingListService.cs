namespace BookHub.Features.ReadingList.Service;

using Book.Service.Models;
using BookHub.Common;
using Infrastructure.Services.Result;
using Infrastructure.Services.ServiceLifetimes;
using ReadingList.Data.Models;
using ReadingList.Service.Models;

public interface IReadingListService : ITransientService
{
    Task<ResultWith<PaginatedModel<BookServiceModel>>> All(
       string userId,
       ReadingListStatus status,
       int pageIndex,
       int pageSize,
       CancellationToken token);

    Task<BookServiceModel?> LastCurrentlyReading(
        string userId,
        CancellationToken token);

    Task<Result> Add(
        ReadingListServiceModel model,
        CancellationToken token);

    Task<Result> Delete(
        ReadingListServiceModel model,
        CancellationToken token);
}
