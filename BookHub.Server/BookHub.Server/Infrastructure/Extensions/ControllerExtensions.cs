namespace BookHub.Server.Infrastructure.Extensions
{
    using BookHub.Server.Features;
    using BookHub.Server.Infrastructure.Services;
    using Microsoft.AspNetCore.Mvc;

    public static class ControllerExtensions
    {
        public static ActionResult NoContentOrBadRequest(this ApiController controller, Result result)
        {
            if (result.Succeeded)
            {
                return controller.NoContent();
            }

            return controller.BadRequest(new { errorMessage = result.ErrorMessage });
        }
    }
}
