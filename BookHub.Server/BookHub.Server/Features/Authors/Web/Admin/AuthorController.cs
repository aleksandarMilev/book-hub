#pragma warning disable ASP0023 
namespace BookHub.Server.Features.Authors.Web.Admin
{
    using Areas.Admin;
    using BookHub.Server.Areas.Admin.Web;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Service;

    [Authorize]
    public class AuthorController(IAuthorService service) : AdminApiController
    {
        private readonly IAuthorService service = service;

        [HttpPut("[action]/{id}")]
        public async Task<ActionResult> Approve(int id)
        {
            var result = await this.service.ApproveAsync(id);

            return this.NoContentOrBadRequest(result);
        }

        [HttpPut("[action]/{id}")]
        public async Task<ActionResult> Reject(int id)
        {
            var result = await this.service.DeleteAsync(id);

            return this.NoContentOrBadRequest(result);
        }
    }
}
