namespace BookHub.Server.Features.Books.Service
{
    using BookHub.Server.Features.Books.Service.Models;

    public interface IBookService
    {
        Task<IEnumerable<BookListServiceModel>> GetAllAsync();

        Task<BookDetailsServiceModel?> GetDetailsAsync(int id);

        Task<int> CreateAsync(CreateBookServiceModel model, string userId);

        Task<bool> EditAsync(int id, CreateBookServiceModel model, string userId);

        Task<bool> DeleteAsync(int id, string userId);
    }
}
