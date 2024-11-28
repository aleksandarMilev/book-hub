namespace BookHub.Server.Features.Authors.Web.User
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
           => Ok(await service.NamesAsync());
    }
}
