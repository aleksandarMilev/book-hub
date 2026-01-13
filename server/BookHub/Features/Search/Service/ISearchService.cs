namespace BookHub.Features.Search.Service;

using Common;
using Infrastructure.Services.ServiceLifetimes;
using Models;

public interface ISearchService : ITransientService
{
    Task<PaginatedModel<SearchGenreServiceModel>> Genres(
        string? searchTerm,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<PaginatedModel<SearchBookServiceModel>> Books(
        string? searchTerm,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<PaginatedModel<SearchArticleServiceModel>> Articles(
        string? searchTerm,
        int pageIndex,
        int pageSize,
        CancellationToken token = default);

    Task<PaginatedModel<SearchAuthorServiceModel>> Authors(
        string? searchTerm,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<PaginatedModel<SearchProfileServiceModel>> Profiles(
        string? searchTerm,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<PaginatedModel<SearchChatServiceModel>> Chats(
        string? searchTerm,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default);
}
