namespace BookHub.Server.Areas.Admin.Features.Article.Web
{
    using Microsoft.AspNetCore.Mvc;

    public class ArticleController : AdminController
    {
        [HttpGet]
        public ActionResult<string> Get() => "Hello from Admin!";
    }
}
