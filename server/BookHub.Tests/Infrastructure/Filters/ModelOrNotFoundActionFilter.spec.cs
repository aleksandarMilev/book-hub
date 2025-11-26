namespace BookHub.Tests.Infrastructure.Filters;

using BookHub.Infrastructure.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Xunit;

public class ModelOrNotFoundActionFilterTests
{
    [Fact]
    public void OnActionExecuted_NonNullObjectResult_KeepsOriginalResult()
    {
        var filter = new ModelOrNotFoundActionFilter();
        var httpContext = new DefaultHttpContext();
        var actionContext = new ActionContext(
            httpContext,
            new RouteData(),
            new ActionDescriptor());

        var originalResult = new ObjectResult(new { Id = 1, Name = "Test" });
        var context = new ActionExecutedContext(
            actionContext,
            new List<IFilterMetadata>(),
            controller: null!)
        {
            Result = originalResult
        };

        filter.OnActionExecuted(context);

        Assert.Same(originalResult, context.Result);
    }

    [Fact]
    public void OnActionExecuted_NullObjectResultValue_SetsNotFoundResult()
    {
        var filter = new ModelOrNotFoundActionFilter();
        var httpContext = new DefaultHttpContext();
        var actionContext = new ActionContext(
            httpContext,
            new RouteData(),
            new ActionDescriptor());

        var context = new ActionExecutedContext(
            actionContext,
            new List<IFilterMetadata>(),
            controller: null!)
        {
            Result = new ObjectResult(null)
        };

        filter.OnActionExecuted(context);

        Assert.IsType<NotFoundResult>(context.Result);
    }

    [Fact]
    public void OnActionExecuted_NonObjectResult_DoesNothing()
    {
        var filter = new ModelOrNotFoundActionFilter();
        var httpContext = new DefaultHttpContext();
        var actionContext = new ActionContext(
            httpContext,
            new RouteData(),
            new ActionDescriptor());

        var originalResult = new StatusCodeResult(StatusCodes.Status204NoContent);
        var context = new ActionExecutedContext(
            actionContext,
            new List<IFilterMetadata>(),
            controller: null!)
        {
            Result = originalResult
        };

        filter.OnActionExecuted(context);

        Assert.Same(originalResult, context.Result);
    }
}
