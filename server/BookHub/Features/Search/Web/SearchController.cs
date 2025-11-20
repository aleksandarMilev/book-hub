namespace BookHub.Features.Search.Web
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Service;
    using Service.Models;

    using static Common.Constants.DefaultValues;

    [Authorize]
    public class SearchController(ISearchService service) : ApiController
    {
        private readonly ISearchService service = service;

        [HttpGet(ApiRoutes.Books)]
        public async Task<ActionResult<PaginatedModel<SearchBookServiceModel>>> Books(
            string? searchTerm,
            int page = DefaultPageIndex,
            int pageSize = DefaultPageSize)
            => this.Ok(await this.service.Books(searchTerm, page, pageSize));

        [HttpGet(ApiRoutes.Articles)]
        public async Task<ActionResult<PaginatedModel<SearchArticleServiceModel>>> Articles(
            string? searchTerm,
            int page = DefaultPageIndex,
            int pageSize = DefaultPageSize)
            => this.Ok(await this.service.Articles(searchTerm, page, pageSize));

        [HttpGet(ApiRoutes.Authors)]
        public async Task<ActionResult<PaginatedModel<SearchAuthorServiceModel>>> Authors(
           string? searchTerm,
           int page = DefaultPageIndex,
           int pageSize = DefaultPageSize)
           => this.Ok(await this.service.Authors(searchTerm, page, pageSize));

        [HttpGet(ApiRoutes.Profiles)]
        public async Task<ActionResult<PaginatedModel<SearchProfileServiceModel>>> Profiles(
           string? searchTerm,
           int page = DefaultPageIndex,
           int pageSize = DefaultPageSize)
           => this.Ok(await this.service.Profiles(searchTerm, page, pageSize));

        [HttpGet(ApiRoutes.Chats)]
        public async Task<ActionResult<PaginatedModel<SearchChatServiceModel>>> Chats(
            string? searchTerm,
            int page = DefaultPageIndex,
            int pageSize = DefaultPageSize)
            => this.Ok(await this.service.Chats(searchTerm, page, pageSize));
    }
}
