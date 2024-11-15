namespace BookHub.Server.Features.Books.Service
{
    using Models;
    using Infrastructure.Services;

    public interface IBookService
    {
        Task<IEnumerable<BookServiceModel>> GetAllAsync();

        Task<IEnumerable<BookServiceModel>> GetTopThreeAsync();

        Task<BookDetailsServiceModel?> GetDetailsAsync(int id);

        Task<int> CreateAsync(CreateBookServiceModel model);

        Task<Result> EditAsync(int id, CreateBookServiceModel model);

        Task<Result> DeleteAsync(int id, string userId);
    }
}
