namespace BookHub.Server.Features.Profile.Web
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Service;

    [Authorize]
    public class ProfileController(
        IProfileService service,
        IMapper mapper) : ApiController
    {
        private readonly IProfileService service = service;
        private readonly IMapper mapper = mapper;

        [HttpGet]
        public ActionResult<string> Get() => this.Ok("Hello from profile!");
    }
}
