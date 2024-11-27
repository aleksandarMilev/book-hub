#pragma warning disable ASP0023 
namespace BookHub.Server.Features.Search.Web
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

        [HttpGet("[action]")]
        public async Task<ActionResult<PaginatedModel<SearchBookServiceModel>>> Books(
            string? searchTerm,
            int page = DefaultPageIndex,
            int pageSize = DefaultPageSize)
                => this.Ok(await this.service.BooksAsync(searchTerm, page, pageSize));


        [HttpGet("[action]")]
        public async Task<ActionResult<PaginatedModel<SearchArticleServiceModel>>> Articles(
            string? searchTerm,
            int page = DefaultPageIndex,
            int pageSize = DefaultPageSize)
                => this.Ok(await this.service.ArticlesAsync(searchTerm, page, pageSize));
    }
}
