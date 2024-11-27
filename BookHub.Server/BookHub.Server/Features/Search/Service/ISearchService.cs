namespace BookHub.Server.Features.Search.Service
{
    using Models;

    public interface ISearchService
    {
        Task<PaginatedModel<SearchBookServiceModel>> BooksAsync(string? searchTerm, int page, int pageSize);

        Task<PaginatedModel<SearchArticleServiceModel>> ArticlesAsync(string? searchTerm, int page, int pageSize);
    }
}
