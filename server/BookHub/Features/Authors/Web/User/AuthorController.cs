namespace BookHub.Features.Authors.Web.User
{
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Service;
    using Service.Models;
    using Shared;

    using static ApiRoutes;
    using static Common.Constants.ApiRoutes;

    [Authorize]
    public class AuthorController(IAuthorService service) : ApiController
    {
        [AllowAnonymous]
        [HttpGet(Author.Top)]
        public async Task<ActionResult<IEnumerable<AuthorServiceModel>>> TopThree()
            => this.Ok(await service.TopThree());

        [HttpGet(Author.Names)]
        public async Task<ActionResult<IEnumerable<AuthorNamesServiceModel>>> Names()
            => this.Ok(await service.Names());

        [HttpGet(Id)]
        public async Task<ActionResult<AuthorDetailsServiceModel>> Details(int id)
            => this.Ok(await this.Details(id));

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateAuthorWebModel webModel)
        {
            var serviceModel = webModel.ToCreateServiceModel();
            var authorId = await service.Create(serviceModel);

            return this.Created(nameof(this.Create), authorId);
        }

        [HttpPut(Id)]
        public async Task<ActionResult> Edit(int id, CreateAuthorWebModel webModel)
        {
            var serviceModel = webModel.ToCreateServiceModel();
            var result = await service.Edit(id, serviceModel);

            return this.NoContentOrBadRequest(result);
        }

        [HttpDelete(Id)]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await service.Delete(id);

            return this.NoContentOrBadRequest(result);
        }
    }
}
