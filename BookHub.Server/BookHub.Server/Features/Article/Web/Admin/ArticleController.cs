namespace BookHub.Server.Features.Article.Web.Admin
{
    using Areas.Admin;
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Service;
    using Service.Models;

    public class ArticleController(
        IArticleService service,
        IMapper mapper) : AdminApiController
    {
        private readonly IArticleService service = service;
        private readonly IMapper mapper = mapper;

        [AllowAnonymous]
        [HttpGet]
        public ActionResult<string> Get() => "Hello from Admin!";

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateArticleWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateArticleServiceModel>(webModel);
            var id = await this.service.CreateAsync(serviceModel);

            return this.Created(nameof(this.Create), id);
        }
    }
}
