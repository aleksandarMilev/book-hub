#pragma warning disable ASP0023 
namespace BookHub.Server.Features.Authors.Web
{
    using AutoMapper;
    using Infrastructure.Extensions;
    using Infrastructure.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Service;
    using Service.Models;

    [Authorize]
    public class AuthorController(
        IAuthorService authorService,
        ICurrentUserService userService,
        IMapper mapper) : ApiController
    {
        private readonly IAuthorService authorService = authorService;
        private readonly ICurrentUserService userService = userService;
        private readonly IMapper mapper = mapper;

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<AuthorNamesServiceModel>>> Names()
            => this.Ok(await this.authorService.GetNamesAsync());

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<AuthorServiceModel>>> TopThree()
            => this.Ok(await this.authorService.GetTopThreeAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDetailsServiceModel>> Details(int id) 
            => this.Ok(await this.authorService.GetDetailsAsync(id));

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateAuthorWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateAuthorServiceModel>(webModel);
            serviceModel.CreatorId = this.userService.GetId();

            var authorId = await this.authorService.CreateAsync(serviceModel);

            return this.Created(nameof(this.Create), authorId);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(int id, CreateAuthorWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateAuthorServiceModel>(webModel);
            serviceModel.CreatorId = this.userService.GetId();

            var result = await this.authorService.EditAsync(id, serviceModel);

            return this.NoContentOrBadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await this.authorService.DeleteAsync(id, this.userService.GetId()!);

            return this.NoContentOrBadRequest(result);
        }
    }
}
