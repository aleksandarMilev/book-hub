namespace BookHub.Tests.Common.Exceptions;

using BookHub.Common.Exceptions;
using Xunit;

public class DbEntityNotFoundExceptionSpec
{
    [Fact]
    public void Constructor_SetsMessage_WithEntityTypeAndIntId()
    {
        var entityType = "Book";
        var id = 42;
        var expectedMessage = $"{entityType} with Id: {id} was not found!";

        var exception = new DbEntityNotFoundException<int>(entityType, id);

        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void Constructor_SetsMessage_WithEntityTypeAndGuidId()
    {
        var entityType = "User";
        var id = Guid.NewGuid();
        var expectedMessage = $"{entityType} with Id: {id} was not found!";

        var exception = new DbEntityNotFoundException<Guid>(entityType, id);

        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void Exception_IsAssignableToSystemException()
    {
        var exception = new DbEntityNotFoundException<int>("Book", 1);

        Assert.IsAssignableFrom<Exception>(exception);
    }
}
