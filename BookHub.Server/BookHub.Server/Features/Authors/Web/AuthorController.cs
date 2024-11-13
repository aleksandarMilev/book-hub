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
        public ActionResult Nationalities()
        {
            var nationalities = this.authorService.GetNationalities();

            return this.Ok(nationalities);
        }


        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<ActionResult> TopThree()
        {
            var model = await this.authorService.GetTopThreeAsync();

            return this.Ok(model);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Details(int id)
        {
            var author = await this.authorService.GetDetailsAsync(id);

            return this.Ok(author);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateAuthorWebModel webModel)
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
