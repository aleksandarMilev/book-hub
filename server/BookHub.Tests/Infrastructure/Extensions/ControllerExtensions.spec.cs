namespace BookHub.Tests.Infrastructure.Extensions;

using BookHub.Infrastructure.Extensions;
using BookHub.Infrastructure.Services.Result;
using Microsoft.AspNetCore.Mvc;
using Xunit;

public class ControllerExtensionsTests
{
    [Fact]
    public void NoContentOrBadRequest_ReturnsNoContent_WhenResultSucceeded()
    {
        var controller = new TestController();
        Result result = true;

        var actionResult = controller.NoContentOrBadRequest(result);

        Assert.IsType<NoContentResult>(actionResult);
    }

    [Fact]
    public void NoContentOrBadRequest_ReturnsBadRequest_WithErrorMessage_WhenResultFailed()
    {
        var controller = new TestController();
        const string errorMessage = "Something went wrong";
        Result result = errorMessage;

        var actionResult = controller.NoContentOrBadRequest(result);

        var badRequest = Assert.IsType<BadRequestObjectResult>(actionResult);
        var value = badRequest.Value;

        var errorMessageProperty = value?
            .GetType()
            .GetProperty("errorMessage")
            ?.GetValue(value, null);

        Assert.Equal(errorMessage, errorMessageProperty);
    }

    [Fact]
    public void OkOrBadRequest_ReturnsOk_WithMappedResponse_WhenResultSucceeded()
    {
        var controller = new TestController();
        var data = 5;
        var result = ResultWith<int>.Success(data);

        var actionResult = controller.OkOrBadRequest(result, x => new
        {
            Value = x,
            IsPositive = x > 0
        });

        var okResult = Assert.IsType<OkObjectResult>(actionResult);
        var value = okResult.Value;

        var valueProperty = value?
            .GetType()
            .GetProperty("Value")
            ?.GetValue(value, null);

        var isPositiveProperty = value?
            .GetType()
            .GetProperty("IsPositive")
            ?.GetValue(value, null);

        Assert.Equal(data, valueProperty);
        Assert.Equal(true, isPositiveProperty);
    }

    [Fact]
    public void OkOrBadRequest_ReturnsBadRequest_WithErrorMessage_WhenResultFailed()
    {
        var controller = new TestController();
        const string errorMessage = "Invalid data";
        ResultWith<int> result = errorMessage;

        var actionResult = controller.OkOrBadRequest(result, x => x);

        var badRequest = Assert.IsType<BadRequestObjectResult>(actionResult);
        var value = badRequest.Value;

        var errorMessageProperty = value?
            .GetType()
            .GetProperty("errorMessage")
            ?.GetValue(value, null);

        Assert.Equal(errorMessage, errorMessageProperty);
    }

    private sealed class TestController : ControllerBase { }
}
