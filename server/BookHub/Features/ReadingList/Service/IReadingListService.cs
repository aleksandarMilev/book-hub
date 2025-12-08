namespace BookHub.Features.ReadingList.Service
{
    using Book.Service.Models;
    using BookHub.Common;
    using BookHub.Infrastructure.Services.Result;
    using Infrastructure.Services;
    using Infrastructure.Services.ServiceLifetimes;

    public interface IReadingListService : ITransientService
    {
        Task<PaginatedModel<BookServiceModel>> All(
           string userId,
           string status,
           int pageIndex,
           int pageSize);

        Task<Result> Add(Guid bookId, string status);

        Task<Result> Delete(Guid bookId, string status);
    }
}
