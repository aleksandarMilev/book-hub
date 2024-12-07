namespace BookHub.Server.Features.Authors.Web.User
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Service;
    using Service.Models;

    using static ApiRoutes;

    [Authorize]
    public class NationalityController(INationalityService service) : ApiController
    {
        private readonly INationalityService service = service;

        [HttpGet(Nationality.Names)]
        public async Task<ActionResult<IEnumerable<NationalityServiceModel>>> Names()
           => this.Ok(await this.service.NamesAsync());
    }
}
