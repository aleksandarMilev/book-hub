namespace BookHub.Infrastructure.ExceptionHandling;

using Microsoft.EntityFrameworkCore;
using Npgsql;
using Services.ImageWriter;

// Exceptions that are the client's fault or a lost race, not a server bug. They map to a 4xx
// ProblemDetails with a generic detail. Anything not listed here is a 500.
public static class ExpectedFailures
{
    public const string UniqueViolationDetail =
        "The request conflicts with an existing resource.";

    public const string StillReferencedDetail =
        "The resource is still in use and cannot be removed.";

    public const string MissingReferenceDetail =
        "The request references a resource that does not exist.";

    public static ExpectedFailure? Map(Exception exception)
        => exception switch
        {
            ImageValidationException imageException => new(
                StatusCodes.Status400BadRequest,
                imageException.Message),

            DbUpdateException { InnerException: PostgresException postgresException } dbException
                => MapPostgresError(
                    postgresException.SqlState,
                    deletesRows: dbException
                        .Entries
                        .Any(e => e.State == EntityState.Deleted)),

            _ => null,
        };

    // Decided by SqlState (never by parsing the message):
    // - unique violation (23505): a duplicate or a lost race against the service's own
    //   duplicate check → 409;
    // - foreign key violation (23503) while deleting: the row is still referenced, so the
    //   request conflicts with the current state → 409;
    // - foreign key violation while inserting or updating: the request points at a row that
    //   doesn't exist, which is invalid input → 400.
    public static ExpectedFailure? MapPostgresError(
        string sqlState,
        bool deletesRows)
        => sqlState switch
        {
            PostgresErrorCodes.UniqueViolation => new(
                StatusCodes.Status409Conflict,
                UniqueViolationDetail),

            PostgresErrorCodes.ForeignKeyViolation when deletesRows => new(
                StatusCodes.Status409Conflict,
                StillReferencedDetail),

            PostgresErrorCodes.ForeignKeyViolation => new(
                StatusCodes.Status400BadRequest,
                MissingReferenceDetail),

            _ => null,
        };
}

public sealed record ExpectedFailure(
    int StatusCode,
    string Detail);
