namespace BookHub.Server.Controllers
{
    using BookHub.Server.Controllers.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : ApiController
    {
        [AllowAnonymous]
        [HttpGet("/")]
        public ActionResult Get() => this.Ok("Hello, World!");
    }
}
