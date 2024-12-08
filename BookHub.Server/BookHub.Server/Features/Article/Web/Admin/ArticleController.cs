namespace BookHub.Server.Features.Article.Web.Admin
{
    using Areas.Admin.Web;
    using AutoMapper;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Service;
    using Service.Models;

    using static BookHub.Server.Common.ApiRoutes;

    public class ArticleController(
        IArticleService service,
        IMapper mapper) : AdminApiController
    {
        private readonly IArticleService service = service;
        private readonly IMapper mapper = mapper;

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateArticleWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateArticleServiceModel>(webModel);
            var id = await this.service.CreateAsync(serviceModel);

            return this.Created(nameof(this.Create), id);
        }

        [HttpPut(Id)]
        public async Task<ActionResult> Edit(int id, CreateArticleWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateArticleServiceModel>(webModel);
            var result = await this.service.EditAsync(id, serviceModel);

            return this.NoContentOrBadRequest(result);
        }

        [HttpDelete(Id)]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await this.service.DeleteAsync(id);

            return this.NoContentOrBadRequest(result);
        }
    }
}
