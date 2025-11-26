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

    public static ActionResult OkOrBadRequest<TData, TResponse>(
            this ControllerBase controller,
            ResultWith<TData> result,
            Func<TData, TResponse> selector)
    {
        if (result.Succeeded)
        {
            var response = selector(result.Data!);
            return controller.Ok(response);
        }

        var errorObject = new
        {
            errorMessage = result.ErrorMessage
        };

        return controller.BadRequest(errorObject);
    }
}
