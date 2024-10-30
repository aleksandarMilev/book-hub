namespace BookHub.Server.Features.Books
{
    using BookHub.Server.Features.Books.Models;

    public interface IBookService
    {
        Task<IEnumerable<BookListServiceModel>> GetAllAsync();

        Task<BookDetailsServiceModel?> GetDetailsAsync(int id);

        Task<int> CreateAsync(string author, string description, string imageUrl, string title, string userId);
    }
}
