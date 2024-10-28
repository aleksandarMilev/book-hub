namespace BookHub.Server.Controllers.Base
{
    using Microsoft.AspNetCore.Mvc;

    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public abstract class ApiController : ControllerBase
    {
    }
}
