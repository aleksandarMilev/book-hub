namespace BookHub.Server.Features.Search.Web
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Service;
    using Service.Models;

    using static Server.Common.DefaultValues;

    [Authorize]
    public class SearchController(ISearchService service) : ApiController
    {
        private readonly ISearchService service = service;

        [HttpGet(ApiRoutes.Books)]
        public async Task<ActionResult<PaginatedModel<SearchBookServiceModel>>> Books(
            string? searchTerm,
            int page = DefaultPageIndex,
            int pageSize = DefaultPageSize) => this.Ok(await this.service.BooksAsync(searchTerm, page, pageSize));

        [HttpGet(ApiRoutes.Articles)]
        public async Task<ActionResult<PaginatedModel<SearchArticleServiceModel>>> Articles(
            string? searchTerm,
            int page = DefaultPageIndex,
            int pageSize = DefaultPageSize) => this.Ok(await this.service.ArticlesAsync(searchTerm, page, pageSize));

        [HttpGet(ApiRoutes.Authors)]
        public async Task<ActionResult<PaginatedModel<SearchAuthorServiceModel>>> Authors(
           string? searchTerm,
           int page = DefaultPageIndex,
           int pageSize = DefaultPageSize) => this.Ok(await this.service.AuthorsAsync(searchTerm, page, pageSize));

        [HttpGet(ApiRoutes.Profiles)]
        public async Task<ActionResult<PaginatedModel<SearchProfileServiceModel>>> Profiles(
           string? searchTerm,
           int page = DefaultPageIndex,
           int pageSize = DefaultPageSize) => this.Ok(await this.service.ProfilesAsync(searchTerm, page, pageSize));

        [HttpGet(ApiRoutes.Chats)]
        public async Task<ActionResult<PaginatedModel<SearchChatServiceModel>>> Chats(
            string? searchTerm,
            int page = DefaultPageIndex,
            int pageSize = DefaultPageSize) => this.Ok(await this.service.ChatsAsync(searchTerm, page, pageSize));
    }
}
