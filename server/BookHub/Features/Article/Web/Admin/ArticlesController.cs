namespace BookHub.Features.Article.Web.Admin
{
    using Areas.Admin.Web;
    using BookHub.Features.Article.Service.Models;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Service;
    using Shared;

    using static Common.Constants.ApiRoutes;
    using static Shared.Constants.RouteNames;

    public class ArticlesController(IArticlesService service) : AdminApiController
    {
        [HttpGet(Id)]
        public async Task<ActionResult<ArticleDetailsServiceModel>> DetailsForEdit(
            Guid id,
            CancellationToken token = default)
            => this.Ok(await service.Details(id, true, token));

        [HttpPost]
        public async Task<ActionResult<ArticleDetailsServiceModel>> Create(
            CreateArticleWebModel webModel,
            CancellationToken token = default)
        {
            var serviceModel = webModel.ToServiceModel();
            var createdArticle = await service.Create(serviceModel, token);

            return CreatedAtRoute(
                routeName: DetailsRouteName,
                routeValues: new { id = createdArticle.Id },
                value: createdArticle);
        }

        [HttpPut(Id)]
        public async Task<ActionResult> Edit(
            Guid id,
            CreateArticleWebModel webModel,
            CancellationToken token = default)
        {
            var serviceModel = webModel.ToServiceModel();
            var result = await service.Edit(
                id,
                serviceModel,
                token);

            return this.NoContentOrBadRequest(result);
        }

        [HttpDelete(Id)]
        public async Task<ActionResult> Delete(
            Guid id,
            CancellationToken token = default)
        {
            var result = await service.Delete(id, token);

            return this.NoContentOrBadRequest(result);
        }
    }
}
