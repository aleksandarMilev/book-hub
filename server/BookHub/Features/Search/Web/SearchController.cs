namespace BookHub.Features.Search.Web;

using BookHub.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Models;

using static Common.Constants.DefaultValues;

[Authorize]
public class SearchController(ISearchService service) : ApiController
{
    [HttpGet(ApiRoutes.Books)]
    public async Task<ActionResult<PaginatedModel<SearchBookServiceModel>>> Books(
        string? searchTerm,
        int page = DefaultPageIndex,
        int pageSize = DefaultPageSize,
        CancellationToken cancellationToken = default)
        => this.Ok(await service.Books(
            searchTerm,
            page,
            pageSize,
            cancellationToken));

    [HttpGet(ApiRoutes.Genres)]
    public async Task<ActionResult<PaginatedModel<SearchBookServiceModel>>> Genres(
        string? searchTerm,
        int page = DefaultPageIndex,
        int pageSize = DefaultPageSize,
        CancellationToken cancellationToken = default)
        => this.Ok(await service.Genres(
            searchTerm,
            page,
            pageSize,
            cancellationToken));

    [AllowAnonymous]
    [HttpGet(ApiRoutes.Articles)]
    public async Task<ActionResult<PaginatedModel<SearchArticleServiceModel>>> Articles(
        string? searchTerm,
        int page = DefaultPageIndex,
        int pageSize = DefaultPageSize,
        CancellationToken cancellationToken = default)
        => this.Ok(await service.Articles(
            searchTerm,
            page,
            pageSize,
            cancellationToken));

    [HttpGet(ApiRoutes.Authors)]
    public async Task<ActionResult<PaginatedModel<SearchAuthorServiceModel>>> Authors(
       string? searchTerm,
       int page = DefaultPageIndex,
       int pageSize = DefaultPageSize,
        CancellationToken cancellationToken = default)
       => this.Ok(await service.Authors(
           searchTerm,
           page,
           pageSize,
           cancellationToken));

    [HttpGet(ApiRoutes.Profiles)]
    public async Task<ActionResult<PaginatedModel<SearchProfileServiceModel>>> Profiles(
       string? searchTerm,
       int page = DefaultPageIndex,
       int pageSize = DefaultPageSize,
        CancellationToken cancellationToken = default)
       => this.Ok(await service.Profiles(
           searchTerm,
           page,
           pageSize,
           cancellationToken));

    [HttpGet(ApiRoutes.Chats)]
    public async Task<ActionResult<PaginatedModel<SearchChatServiceModel>>> Chats(
        string? searchTerm,
        int page = DefaultPageIndex,
        int pageSize = DefaultPageSize,
        CancellationToken cancellationToken = default)
        => this.Ok(await service.Chats(
            searchTerm,
            page,
            pageSize,
            cancellationToken));
}
