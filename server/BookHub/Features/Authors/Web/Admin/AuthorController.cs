namespace BookHub.Features.Authors.Web.Admin
{
    using Areas.Admin.Web;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Service;
    using Service.Models;

    using static ApiRoutes;
    using static Common.Constants.ApiRoutes;

    [Authorize]
    public class AuthorController(IAuthorService service) : AdminApiController
    {
        [HttpGet(Id)]
        public async Task<ActionResult<AuthorDetailsServiceModel>> Details(int id)
          => this.Ok(await service.AdminDetails(id));

        [HttpPatch(Id + Author.Approve)]
        public async Task<ActionResult> Approve(int id)
        {
            var result = await service.Approve(id);

            return this.NoContentOrBadRequest(result);
        }

        [HttpPatch((Id + Author.Reject))]
        public async Task<ActionResult> Reject(int id)
        {
            var result = await service.Reject(id);

            return this.NoContentOrBadRequest(result);
        }
    }
}
