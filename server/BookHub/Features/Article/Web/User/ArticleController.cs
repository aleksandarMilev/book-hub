namespace BookHub.Features.Article.Web.User
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Service;
    using Service.Models;

    using static Common.ApiRoutes;

    [Authorize]
    public class ArticleController(IArticleService service) : ApiController
    {
        private readonly IArticleService service = service;

        [HttpGet(Id)]
        public async Task<ActionResult<ArticleDetailsServiceModel>> Details(int id)
            => this.Ok(await this.service.Details(id));
    }
}
