namespace BookHub.Server.Features.Books
{
    public interface IBookService
    {
        Task<int> CreateAsync(string author, string description, string imageUrl, string userId);
    }
}
