#pragma warning disable ASP0023 
namespace BookHub.Server.Features.Authors.Web
{
    using AutoMapper;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Service;
    using Service.Models;

    //[Authorize]
    public class AuthorController(
        IAuthorService service,
        IMapper mapper) : ApiController
    {
        private readonly IAuthorService service = service;
        private readonly IMapper mapper = mapper;

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<AuthorServiceModel>>> TopThree()
           => this.Ok(await this.service.TopThreeAsync());

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<AuthorNamesServiceModel>>> Names()
            => this.Ok(await this.service.NamesAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDetailsServiceModel>> Details(int id) 
            => this.Ok(await this.service.DetailsAsync(id));

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateAuthorWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateAuthorServiceModel>(webModel);
            var authorId = await this.service.CreateAsync(serviceModel);

            return this.Created(nameof(this.Create), authorId);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(int id, CreateAuthorWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateAuthorServiceModel>(webModel);
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
