namespace BookHub.Tests.ErrorHandling;

using FluentAssertions;
using Infrastructure.ExceptionHandling;
using Infrastructure.Services.ImageWriter;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Npgsql;

public sealed class ExpectedFailuresUnit
{
    [Fact]
    public void MapPostgresError_ShouldReturnConflict_ForUniqueViolation()
    {
        var failure = ExpectedFailures.MapPostgresError(
            PostgresErrorCodes.UniqueViolation,
            deletesRows: false);

        failure.Should().Be(new ExpectedFailure(
            StatusCodes.Status409Conflict,
            ExpectedFailures.UniqueViolationDetail));
    }

    [Fact]
    public void MapPostgresError_ShouldReturnBadRequest_ForForeignKeyViolation_WhenInsertingOrUpdating()
    {
        var failure = ExpectedFailures.MapPostgresError(
            PostgresErrorCodes.ForeignKeyViolation,
            deletesRows: false);

        failure.Should().Be(new ExpectedFailure(
            StatusCodes.Status400BadRequest,
            ExpectedFailures.MissingReferenceDetail));
    }

    [Fact]
    public void MapPostgresError_ShouldReturnConflict_ForForeignKeyViolation_WhenDeleting()
    {
        var failure = ExpectedFailures.MapPostgresError(
            PostgresErrorCodes.ForeignKeyViolation,
            deletesRows: true);

        failure.Should().Be(new ExpectedFailure(
            StatusCodes.Status409Conflict,
            ExpectedFailures.StillReferencedDetail));
    }

    [Theory]
    [InlineData(PostgresErrorCodes.NotNullViolation)]
    [InlineData(PostgresErrorCodes.SerializationFailure)]
    [InlineData(PostgresErrorCodes.DeadlockDetected)]
    public void MapPostgresError_ShouldReturnNull_ForOtherErrors(string sqlState)
        => ExpectedFailures
            .MapPostgresError(sqlState, deletesRows: false)
            .Should()
            .BeNull();

    [Fact]
    public void Map_ShouldUseTheSqlState_OfTheInnerPostgresException()
    {
        var postgresException = new PostgresException(
            messageText: "duplicate key value violates unique constraint",
            severity: "ERROR",
            invariantSeverity: "ERROR",
            sqlState: PostgresErrorCodes.UniqueViolation);

        var failure = ExpectedFailures.Map(
            new DbUpdateException("Save failed.", postgresException));

        failure!.StatusCode.Should().Be(StatusCodes.Status409Conflict);
    }

    [Fact]
    public void Map_ShouldReturnBadRequest_WithTheMessage_ForImageValidationException()
    {
        var failure = ExpectedFailures.Map(
            new ImageValidationException("Image file is empty."));

        failure.Should().Be(new ExpectedFailure(
            StatusCodes.Status400BadRequest,
            "Image file is empty."));
    }

    [Fact]
    public void Map_ShouldReturnNull_ForUnexpectedExceptions()
    {
        ExpectedFailures.Map(new InvalidOperationException()).Should().BeNull();
        ExpectedFailures.Map(new DbUpdateException("No inner exception.")).Should().BeNull();
    }
}
