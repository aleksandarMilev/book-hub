namespace BookHub.Server.Features.Books
{
    public interface IBookService
    {
        Task<IEnumerable<BookListResponseModel>> GetAllAsync();
        
        Task<int> CreateAsync(string author, string description, string imageUrl, string title, string userId);
    }
}
