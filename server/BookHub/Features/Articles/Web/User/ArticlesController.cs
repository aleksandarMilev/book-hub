namespace BookHub.Features.Article.Web.User;

using BookHub.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Models;

using static Common.Constants.ApiRoutes;
using static Shared.Constants.RouteNames;

public class ArticlesController(IArticleService service) : ApiController
{
    [AllowAnonymous]
    [HttpGet(Id, Name = DetailsRouteName)]
    public async Task<ActionResult<ArticleDetailsServiceModel>> Details(
        Guid id,
        CancellationToken cancellationToken = default)
        => this.Ok(await service.Details(id, false, cancellationToken));
}
