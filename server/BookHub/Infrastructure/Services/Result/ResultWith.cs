namespace BookHub.Infrastructure.Services.Result;

public class ResultWith<T>
{
    private ResultWith(
        bool succeeded,
        T? data = default,
        string? errorMessage = null,
        ErrorKind errorKind = ErrorKind.BadRequest)
    {
        this.Succeeded = succeeded;
        this.Data = data;
        this.ErrorMessage = errorMessage;
        this.ErrorKind = errorKind;
    }

    public bool Succeeded { get; init; }

    public T? Data { get; init; }

    public string? ErrorMessage { get; init; }

    public ErrorKind ErrorKind { get; init; }

    public static ResultWith<T> Success(T data)
        => new(true, data);

    public static ResultWith<T> Failure(string errorMessage)
        => new(false, default, errorMessage);

    public static ResultWith<T> NotFound(string errorMessage)
        => new(false, default, errorMessage, ErrorKind.NotFound);

    public static ResultWith<T> Forbidden(string errorMessage)
        => new(false, default, errorMessage, ErrorKind.Forbidden);

    public static ResultWith<T> Conflict(string errorMessage)
        => new(false, default, errorMessage, ErrorKind.Conflict);

    public static implicit operator ResultWith<T>(string errorMessage)
        => new(false, default, errorMessage);
}
