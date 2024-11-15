namespace BookHub.Server.Features.Search.Web
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Service;
    using Service.Models;

    [Authorize]
    public class SearchController(ISearchService service) : ApiController
    {
        private readonly ISearchService service = service;

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<SearchBookServiceModel>>> Books(string searchTerm)
            => this.Ok(await this.service.GetBooksAsync(searchTerm));
    }
}
