namespace BookHub.Common;

public static class Utils
{
    public static string? ToIso8601String(this DateTime? dateTime)
        => dateTime.HasValue
            ? dateTime.Value.ToString("O")
            : null;
}
