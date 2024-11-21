namespace BookHub.Server.Features.Books.Service
{
    using Models;
    using Infrastructure.Services;

    public interface IBookService
    {
        Task<IEnumerable<BookServiceModel>> GetAllAsync();

        Task<IEnumerable<BookServiceModel>> GetTopThreeAsync();

        Task<BookDetailsServiceModel?> GetDetailsAsync(int id, string userId);

        Task<int> CreateAsync(CreateBookServiceModel model, string userId);

        Task<Result> EditAsync(int id, CreateBookServiceModel model, string userId);

        Task<Result> DeleteAsync(int id, string userId);
    }
}
