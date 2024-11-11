namespace BookHub.Server.Infrastructure.Extensions
{
    using Features;
    using Services;
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
