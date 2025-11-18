namespace BookHub.Features.Article.Web.User
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Service;
    using Service.Models;

    using static Common.ApiRoutes;
    using static Shared.Constants.RouteNames;

    [Authorize]
    public class ArticleController(IArticleService service) : ApiController
    {
        // We should provide name here so we can construct the location header easier in the admin controller
        [HttpGet(Id, Name = DetailsRouteName)]
        public async Task<ActionResult<ArticleDetailsServiceModel>> Details(
            string id,
            CancellationToken token = default)
            => this.Ok(await service.Details(id, token));
    }
}
