namespace BookHub.Server.Features.Books.Service
{
    using Models;
    using Infrastructure.Services;

    public interface IBookService
    {
        Task<IEnumerable<BookListServiceModel>> GetAllAsync();

        Task<IEnumerable<BookListServiceModel>> GetTopThreeAsync();

        Task<BookDetailsServiceModel?> GetDetailsAsync(int id);

        Task<int> CreateAsync(CreateBookServiceModel model, string userId);

        Task<Result> EditAsync(int id, CreateBookServiceModel model, string userId);

        Task<Result> DeleteAsync(int id, string userId);
    }
}
