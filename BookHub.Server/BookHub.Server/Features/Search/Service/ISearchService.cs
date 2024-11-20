namespace BookHub.Server.Features.Search.Service
{
    using Models;

    public interface ISearchService
    {
        Task<PaginatedModel<SearchBookServiceModel>> GetBooksAsync(string? searchTerm, int page, int pageSize);
    }
}
