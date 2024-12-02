namespace BookHub.Server.Features.ReadingList.Service
{
    using Book.Service.Models;
    using Infrastructure.Services;
    using Infrastructure.Services.ServiceLifetimes;

    public interface IReadingListService : ITransientService
    {
        Task<PaginatedModel<BookServiceModel>> AllAsync(
           string userId,
           string status,
           int pageIndex,
           int pageSize);

        Task<Result> AddAsync(int bookId, string status);

        Task<Result> DeleteAsync(int bookId, string status);
    }
}
