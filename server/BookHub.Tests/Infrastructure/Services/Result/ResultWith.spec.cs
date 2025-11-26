namespace BookHub.Infrastructure.Services.Result.Tests;

using Xunit;

public class ResultWithSpecs
{
    [Fact]
    public void SuccessFactory_ShouldSetData_And_Succeeded()
    {
        var data = 123;

        var result = ResultWith<int>.Success(data);

        Assert.True(result.Succeeded);
        Assert.Equal(data, result.Data);
        Assert.Null(result.ErrorMessage);
    }

    [Fact]
    public void FailureFactory_ShouldSetError_And_NotSucceeded()
    {
        var error = "Something failed";

        var result = ResultWith<int>.Failure(error);

        Assert.False(result.Succeeded);
        Assert.Equal(error, result.ErrorMessage);
    }

    [Fact]
    public void ImplicitConversion_FromString_ShouldCreateFailure()
    {
        ResultWith<int> result = "Bad stuff happened";

        Assert.False(result.Succeeded);
        Assert.Equal("Bad stuff happened", result.ErrorMessage);
    }
}
