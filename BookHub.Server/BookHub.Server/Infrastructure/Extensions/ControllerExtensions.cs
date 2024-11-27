namespace BookHub.Server.Infrastructure.Extensions
{
    using Features;
    using Microsoft.AspNetCore.Mvc;
    using Services;

    public static class ControllerExtensions
    {
        public static ActionResult NoContentOrBadRequest(this ControllerBase controller, Result result)
        {
            if (result.Succeeded)
            {
                return controller.NoContent();
            }

            return controller.BadRequest(new { errorMessage = result.ErrorMessage });
        }
    }
}
