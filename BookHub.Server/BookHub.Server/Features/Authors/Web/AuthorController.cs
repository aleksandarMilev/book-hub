namespace BookHub.Server.Features.Authors.Web
{
    using AutoMapper;
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

        [HttpPost]
        public async Task<ActionResult> Create(CreateAuthorWebModel webModel)
        {
            var userId = this.userService.GetId();
            var serviceModel = this.mapper.Map<AuthorDetailsServiceModel>(webModel);
            serviceModel.CreatorId = userId;

            var authorId = await this.authorService.CreateAsync(serviceModel);

            return this.Created(nameof(this.Create), authorId);
        }
    }
}
