namespace BookHub.Server.Controllers
{
    using BookHub.Server.Controllers.Base;
    using Microsoft.AspNetCore.Mvc;


    public class BookController : ApiController
    {

        [HttpPost]
        public async Task<ActionResult<int>> Create() 
        {
            return this.Ok();
        } 
    }
}
