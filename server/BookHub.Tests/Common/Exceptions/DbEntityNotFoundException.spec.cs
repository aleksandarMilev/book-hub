namespace BookHub.Common.Exceptions.Tests;

using Xunit;

public class DbEntityNotFoundExceptionSpecs
{
    [Fact]
    public void Constructor_ShouldSetTheCorrectMessage()
    {
        var id = 42;
        var entityType = "Book";

        var exception = new DbEntityNotFoundException<int>(entityType, id);

        Assert.Equal("Book with Id: 42 was not found!", exception.Message);
    }

    [Fact]
    public void ShouldBeOfTypeException()
    {
        var exception = new DbEntityNotFoundException<Guid>("Author", Guid.Empty);
        Assert.IsType<Exception>(exception);
    }
}
