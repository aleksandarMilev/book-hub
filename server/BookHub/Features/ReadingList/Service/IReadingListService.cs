namespace BookHub.Features.ReadingList.Service
{
    using Book.Service.Models;
    using Infrastructure.Services;
    using Infrastructure.Services.ServiceLifetimes;

    public interface IReadingListService : ITransientService
    {
        Task<PaginatedModel<BookServiceModel>> All(
           string userId,
           string status,
           int pageIndex,
           int pageSize);

        Task<Result> Add(int bookId, string status);

        Task<Result> Delete(int bookId, string status);
    }
}
