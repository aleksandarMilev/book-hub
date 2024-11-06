namespace BookHub.Server.Infrastructure.Extensions
{
    using BookHub.Server.Features;
    using Microsoft.AspNetCore.Mvc;

    public static class ControllerExtensions
    {
        public static ActionResult NoContentOrBadRequest(this ApiController controller, bool succeed)
        {
            if (succeed)
            {
                return controller.NoContent();
            }

            return controller.BadRequest();
        }
    }
}
