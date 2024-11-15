namespace BookHub.Server.Features.Nationality.Web
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Service;
    using Service.Models;

    [Authorize]
    public class NationalityController(INationalityService service) : ApiController
    {
        private readonly INationalityService service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NationalityServiceModel>>> Names()
           => this.Ok(await this.service.GetNamesAsync());
    }
}
