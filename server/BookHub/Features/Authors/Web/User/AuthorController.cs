namespace BookHub.Features.Authors.Web.User
{
    using AutoMapper;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Service;
    using Service.Models;

    using static ApiRoutes;
    using static Common.ApiRoutes;

    [Authorize]
    public class AuthorController(
        IAuthorService service,
        IMapper mapper) : ApiController
    {
        private readonly IAuthorService service = service;
        private readonly IMapper mapper = mapper;

        [AllowAnonymous]
        [HttpGet(Author.Top)]
        public async Task<ActionResult<IEnumerable<AuthorServiceModel>>> TopThree()
           => this.Ok(await this.service.TopThree());

        [HttpGet(Author.Names)]
        public async Task<ActionResult<IEnumerable<AuthorNamesServiceModel>>> Names()
            => this.Ok(await this.service.Names());

        [HttpGet(Id)]
        public async Task<ActionResult<AuthorDetailsServiceModel>> Details(int id)
            => this.Ok(await this.service.Details(id));

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateAuthorWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateAuthorServiceModel>(webModel);
            var authorId = await this.service.Create(serviceModel);

            return this.Created(nameof(this.Create), authorId);
        }

        [HttpPut(Id)]
        public async Task<ActionResult> Edit(int id, CreateAuthorWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateAuthorServiceModel>(webModel);
            var result = await this.service.Edit(id, serviceModel);

            return this.NoContentOrBadRequest(result);
        }

        [HttpDelete(Id)]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await this.service.Delete(id);

            return this.NoContentOrBadRequest(result);
        }
    }
}
