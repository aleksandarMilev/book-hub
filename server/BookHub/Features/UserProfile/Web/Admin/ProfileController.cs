﻿namespace BookHub.Features.UserProfile.Web.Admin
{
    using Areas.Admin.Web;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Service;

    using static Common.ApiRoutes;

    public class ProfileController(IProfileService service) : AdminApiController
    {
        private readonly IProfileService service = service;

        [HttpDelete(Id)]
        public async Task<ActionResult> Delete(string id)
        {
            var result = await this.service.Delete(id);

            return this.NoContentOrBadRequest(result);
        }
    }
}
