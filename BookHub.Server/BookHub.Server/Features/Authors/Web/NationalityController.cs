namespace BookHub.Server.Features.Authors.Web
{
    using BookHub.Server.Features.Authors.Service.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Service;

    [Authorize]
    public class NationalityController(INationalityService service) : ApiController
    {
        private readonly INationalityService service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NationalityServiceModel>>> Names()
           => this.Ok(await this.service.NamesAsync());
    }
}
