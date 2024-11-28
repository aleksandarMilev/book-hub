﻿namespace BookHub.Server.Features.Article.Web.Admin
{
    using Areas.Admin;
    using AutoMapper;
    using BookHub.Server.Areas.Admin.Web;
    using Infrastructure.Extensions;
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

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateArticleWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateArticleServiceModel>(webModel);
            var id = await this.service.CreateAsync(serviceModel);

            return this.Created(nameof(this.Create), id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(int id, CreateArticleWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateArticleServiceModel>(webModel);
            var result = await this.service.EditAsync(id, serviceModel);

            return this.NoContentOrBadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await this.service.DeleteAsync(id);

            return this.NoContentOrBadRequest(result);
        }
    }
}
