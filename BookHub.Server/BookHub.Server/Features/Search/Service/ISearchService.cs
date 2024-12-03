namespace BookHub.Server.Features.Search.Service
{
    using Infrastructure.Services.ServiceLifetimes;
    using Models;

    public interface ISearchService : ITransientService
    {
        Task<PaginatedModel<SearchBookServiceModel>> BooksAsync(string? searchTerm, int page, int pageSize);

        Task<PaginatedModel<SearchArticleServiceModel>> ArticlesAsync(string? searchTerm, int page, int pageSize);

        Task<PaginatedModel<SearchAuthorServiceModel>> AuthorsAsync(string? searchTerm, int page, int pageSize);

        Task<PaginatedModel<SearchProfileServiceModel>> ProfilesAsync(string? searchTerm, int page, int pageSize);

    }
}
