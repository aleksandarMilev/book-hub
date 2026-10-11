namespace BookHub.Common;

public static class Utils
{
    extension(DateTime? dateTime)
    {
        public string? ToIso8601String()
            => dateTime.HasValue
                ? dateTime.Value.ToString("O")
                : null;
    }
}
