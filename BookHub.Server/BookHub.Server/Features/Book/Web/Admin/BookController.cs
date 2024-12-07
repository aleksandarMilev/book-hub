namespace BookHub.Server.Features.Book.Web.Admin
{
    using Areas.Admin.Web;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Service;
    using Service.Models;

    using static Common.Constants.ApiRoutes.CommonRoutes;

    [Authorize]
    public class BookController(IBookService service) : AdminApiController
    {
        private readonly IBookService service = service;

        [HttpGet(Id)]
        public async Task<ActionResult<BookDetailsServiceModel>> Details(int id)
            => this.Ok(await this.service.AdminDetailsAsync(id));

        [HttpPatch(Id + ApiRoutes.Approve)]
        public async Task<ActionResult> Approve(int id)
        {
            var result = await this.service.ApproveAsync(id);

            return this.NoContentOrBadRequest(result);
        }

        [HttpPatch(Id + ApiRoutes.Reject)]
        public async Task<ActionResult> Reject(int id)
        {
            var result = await this.service.RejectAsync(id);

            return this.NoContentOrBadRequest(result);
        }
    }
}
