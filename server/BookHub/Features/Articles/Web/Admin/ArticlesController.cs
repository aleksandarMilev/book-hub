namespace BookHub.Features.Article.Web.Admin;

using Areas.Admin.Web;
using BookHub.Features.Article.Service.Models;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Models;
using Service;
using Shared;

using static Common.Constants.ApiRoutes;
using static Shared.Constants.RouteNames;

public class ArticlesController(IArticleService service) : AdminApiController
{
    [HttpGet(Id)]
    public async Task<ActionResult<ArticleDetailsServiceModel>> DetailsForEdit(
        Guid id,
        CancellationToken cancellationToken = default)
        => this.Ok(await service.Details(id, true, cancellationToken));

    [HttpPost]
    public async Task<ActionResult<ArticleDetailsServiceModel>> Create(
        CreateArticleWebModel webModel,
        CancellationToken cancellationToken = default)
    {
        var serviceModel = webModel.ToServiceModel();
        var createdArticle = await service.Create(
            serviceModel,
            cancellationToken);

        return this.CreatedAtRoute(
            routeName: DetailsRouteName,
            routeValues: new { id = createdArticle.Id },
            value: createdArticle);
    }

    [HttpPut(Id)]
    public async Task<ActionResult> Edit(
        Guid id,
        CreateArticleWebModel webModel,
        CancellationToken cancellationToken = default)
    {
        var serviceModel = webModel.ToServiceModel();
        var result = await service.Edit(
            id,
            serviceModel,
            cancellationToken);

        return this.NoContentOrBadRequest(result);
    }

    [HttpDelete(Id)]
    public async Task<ActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await service.Delete(
            id,
            cancellationToken);

        return this.NoContentOrBadRequest(result);
    }
}
