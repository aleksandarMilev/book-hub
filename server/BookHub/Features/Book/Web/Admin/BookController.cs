﻿namespace BookHub.Features.Book.Web.Admin
{
    using Areas.Admin.Web;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Service;
    using Service.Models;

    using static Common.ApiRoutes;

    [Authorize]
    public class BookController(IBookService service) : AdminApiController
    {
        private readonly IBookService service = service;

        [HttpGet(Id)]
        public async Task<ActionResult<BookDetailsServiceModel>> Details(int id)
            => this.Ok(await this.service.AdminDetails(id));

        [HttpPatch(Id + ApiRoutes.Approve)]
        public async Task<ActionResult> Approve(int id)
        {
            var result = await this.service.Approve(id);

            return this.NoContentOrBadRequest(result);
        }

        [HttpPatch(Id + ApiRoutes.Reject)]
        public async Task<ActionResult> Reject(int id)
        {
            var result = await this.service.Reject(id);

            return this.NoContentOrBadRequest(result);
        }
    }
}
