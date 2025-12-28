namespace BookHub.Tests.Helpers;

using Microsoft.AspNetCore.Mvc;

public static class ActionResultHelpers
{
    private const int OkStatusCode = 200;

    public static int? GetStatusCode<T>(this ActionResult<T> result)
    {
        if (result.Result is StatusCodeResult statusCodeResult)
        {
            return statusCodeResult.StatusCode;
        }

        if (result.Result is ObjectResult objectResult)
        {
            return objectResult.StatusCode ?? OkStatusCode;
        }

        if (result.Value is not null)
        {
            return OkStatusCode;
        }

        return null;
    }

    public static T? GetValue<T>(
        this ActionResult<T> result) where T : class
    {
        if (result.Value is not null)
        {
            return result.Value;
        }

        if (result.Result is ObjectResult objectResult)
        {
            return objectResult.Value as T;
        }

        return default;
    }
}
