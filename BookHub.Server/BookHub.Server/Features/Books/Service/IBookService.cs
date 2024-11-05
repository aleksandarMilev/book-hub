namespace BookHub.Server.Features.Books.Service
{
    using BookHub.Server.Features.Books.Service.Models;

    public interface IBookService
    {
        Task<IEnumerable<BookListServiceModel>> GetAllAsync();

        Task<BookDetailsServiceModel?> GetDetailsAsync(int id);

        Task<int> CreateAsync(string author, string description, string imageUrl, string title, string userId);

        Task<bool> DeleteAsync(int id, string userId);
    }
}
