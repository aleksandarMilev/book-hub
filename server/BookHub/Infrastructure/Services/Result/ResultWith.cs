namespace BookHub.Infrastructure.Services.Result;

public class ResultWith<T>
{
    private ResultWith(
        bool succeeded,
        T? data = default,
        string? errorMessage = null)
    {
        Succeeded = succeeded;
        Data = data;
        ErrorMessage = errorMessage;
    }

    public bool Succeeded { get; init; }

    public T? Data { get; init; }

    public string? ErrorMessage { get; init; }

    public static ResultWith<T> Success(T data)
        => new(true, data);

    public static ResultWith<T> Failure(string errorMessage)
        => new(false, default, errorMessage);

    public static implicit operator ResultWith<T>(string errorMessage)
        => new(false, default, errorMessage);
}
