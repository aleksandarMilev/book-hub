namespace BookHub.Server.Features.ReadingList.Service
{
    using Book.Service.Models;
    using Infrastructure.Services;

    public interface IReadingListService
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
