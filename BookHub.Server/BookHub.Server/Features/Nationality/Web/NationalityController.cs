namespace BookHub.Server.Features.Nationality.Web
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Service;

    [Authorize]
    public class NationalityController(INationalityService service) : ApiController
    {
        private readonly INationalityService service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Names()
           => this.Ok(await this.service.GetNamesAsync());
    }
}
