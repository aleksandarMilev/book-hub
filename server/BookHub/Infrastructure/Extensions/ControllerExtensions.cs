namespace BookHub.Infrastructure.Extensions;

using Microsoft.AspNetCore.Mvc;
using Services.Result;

// Maps service Results to responses. Every failure is a ProblemDetails (RFC 9457) body
// with the error message in `detail` and the status code taken from the Result's ErrorKind.
public static class ControllerExtensions
{
    public static ActionResult NoContentOrProblem(
        this ControllerBase controller,
        Result result)
    {
        if (result.Succeeded)
        {
            return controller.NoContent();
        }

        return controller.ProblemFor(
            result.ErrorMessage,
            result.ErrorKind);
    }

    public static ActionResult OkOrProblem<TData, TResponse>(
        this ControllerBase controller,
        ResultWith<TData> result,
        Func<TData, TResponse> selector)
    {
        if (result.Succeeded)
        {
            var response = selector(result.Data!);
            return controller.Ok(response);
        }

        return controller.ProblemFor(
            result.ErrorMessage,
            result.ErrorKind);
    }

    public static ActionResult CreatedAtRouteOrProblem<TData>(
        this ControllerBase controller,
        ResultWith<TData> result,
        string routeName,
        Func<TData, object> routeValues)
    {
        if (result.Succeeded)
        {
            return controller.CreatedAtRoute(
                routeName,
                routeValues(result.Data!),
                result.Data);
        }

        return controller.ProblemFor(
            result.ErrorMessage,
            result.ErrorKind);
    }

    public static int ToStatusCode(this ErrorKind errorKind)
        => errorKind switch
        {
            ErrorKind.NotFound => StatusCodes.Status404NotFound,
            ErrorKind.Forbidden => StatusCodes.Status403Forbidden,
            ErrorKind.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status400BadRequest,
        };

    private static ObjectResult ProblemFor(
        this ControllerBase controller,
        string? detail,
        ErrorKind errorKind)
        => controller.Problem(
            detail: detail,
            statusCode: errorKind.ToStatusCode());
}
