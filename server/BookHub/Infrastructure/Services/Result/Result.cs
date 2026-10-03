namespace BookHub.Infrastructure.Services.Result;

public class Result
{
    public Result(bool succeeded)
        => this.Succeeded = succeeded;

    public Result(
        string errorMessage,
        ErrorKind errorKind = ErrorKind.BadRequest)
    {
        this.Succeeded = false;
        this.ErrorMessage = errorMessage;
        this.ErrorKind = errorKind;
    }

    public bool Succeeded { get; init; }

    public string? ErrorMessage { get; init; }

    public ErrorKind ErrorKind { get; init; }

    public static Result NotFound(string errorMessage)
        => new(errorMessage, ErrorKind.NotFound);

    public static Result Forbidden(string errorMessage)
        => new(errorMessage, ErrorKind.Forbidden);

    public static Result Conflict(string errorMessage)
        => new(errorMessage, ErrorKind.Conflict);

    public static implicit operator Result(bool succeeded)
        => new(succeeded);

    public static implicit operator Result(string errorMessage)
        => new(errorMessage);
}
