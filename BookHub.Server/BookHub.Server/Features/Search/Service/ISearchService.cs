namespace BookHub.Server.Features.Search.Service
{
    using Models;

    public interface ISearchService
    {
        Task<IEnumerable<SearchBookServiceModel>> GetBooksAsync(string? searchTerm);
    }
}
