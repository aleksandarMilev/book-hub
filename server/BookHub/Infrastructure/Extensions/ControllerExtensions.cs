namespace BookHub.Infrastructure.Extensions;

using Microsoft.AspNetCore.Mvc;
using Services.Result;

public static class ControllerExtensions
{
    extension(ControllerBase controller)
    {
        public ActionResult NoContentOrBadRequest(Result result)
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

        public ActionResult OkOrBadRequest<TData, TResponse>(
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
}
