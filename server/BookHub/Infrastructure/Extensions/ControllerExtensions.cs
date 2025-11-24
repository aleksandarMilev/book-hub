namespace BookHub.Infrastructure.Extensions;

using Microsoft.AspNetCore.Mvc;
using Services.Result;

public static class ControllerExtensions
{
    public static ActionResult NoContentOrBadRequest(
        this ControllerBase controller, 
        Result result)
    {
        if (result.Succeeded)
        {
            return controller.NoContent();
        }

        var errorObject = new
        {
            errorMessage = result.ErrorMessage
        };

        return controller.BadRequest(errorObject);
    }
}
