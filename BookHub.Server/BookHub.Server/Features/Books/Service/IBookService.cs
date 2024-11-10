namespace BookHub.Server.Features.Books.Service
{
    using BookHub.Server.Features.Books.Service.Models;
    using BookHub.Server.Infrastructure.Services;

    public interface IBookService
    {
        Task<IEnumerable<BookListServiceModel>> GetAllAsync();

        Task<BookDetailsServiceModel?> GetDetailsAsync(int id);

        Task<int> CreateAsync(CreateBookServiceModel model, string userId);

        Task<Result> EditAsync(int id, CreateBookServiceModel model, string userId);

        Task<Result> DeleteAsync(int id, string userId);
    }
}
