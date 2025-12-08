namespace BookHub.Features.Search.Service;

using Infrastructure.Services.ServiceLifetimes;
using Models;

public interface ISearchService : ITransientService
{
    Task<PaginatedModel<SearchGenreServiceModel>> Genres(
        string? searchTerm,
        int page,
        int pageSize,
        CancellationToken token = default);

    Task<PaginatedModel<SearchBookServiceModel>> Books(
        string? searchTerm,
        int page,
        int pageSize,
        CancellationToken token = default);

    Task<PaginatedModel<SearchArticleServiceModel>> Articles(
        string? searchTerm,
        int page,
        int pageSize,
        CancellationToken token = default);

    Task<PaginatedModel<SearchAuthorServiceModel>> Authors(
        string? searchTerm,
        int page,
        int pageSize,
        CancellationToken token = default);

    Task<PaginatedModel<SearchProfileServiceModel>> Profiles(
        string? searchTerm,
        int page,
        int pageSize,
        CancellationToken token = default);

    Task<PaginatedModel<SearchChatServiceModel>> Chats(
        string? searchTerm,
        int page,
        int pageSize,
        CancellationToken token = default);
}
