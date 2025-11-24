namespace BookHub.Infrastructure.Services.Result.Tests;

using Xunit;

public class ResultSpecs
{
    [Fact]
    public void Constructor_WithBool_ShouldSetSucceeded()
    {
        var result = new Result(true);

        Assert.True(result.Succeeded);
        Assert.Null(result.ErrorMessage);
    }

    [Fact]
    public void Constructor_WithErrorMessage_ShouldSetProperties()
    {
        var error = "Something went wrong";
        var result = new Result(error);

        Assert.False(result.Succeeded);
        Assert.Equal(error, result.ErrorMessage);
    }

    [Fact]
    public void ImplicitConversionFromBool_ShouldWork()
    {
        Result result = true;

        Assert.True(result.Succeeded);
        Assert.Null(result.ErrorMessage);
    }

    [Fact]
    public void ImplicitConversionFromString_ShouldWork()
    {
        Result result = "Error!";

        Assert.False(result.Succeeded);
        Assert.Equal("Error!", result.ErrorMessage);
    }
}
