namespace BookHub.Server.Features.Article.Web.User
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Service;
    using Service.Models;

    [Authorize]
    public class ArticleController(IArticleService service) : ApiController
    {
        private readonly IArticleService service = service;

        [HttpGet("{id}")]
        public async Task<ActionResult<ArticleDetailsServiceModel>> Details(int id)
            => this.Ok(await this.service.DetailsAsync(id));
    }
}
