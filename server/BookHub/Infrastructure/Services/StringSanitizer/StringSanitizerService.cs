namespace BookHub.Infrastructure.Services.StringSanitizer;

public class StringSanitizerService : IStringSanitizerService
{
    public string SanitizeStringForLog(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return "<null>";
        }

        return value
            .Replace("\r", "\\r")
            .Replace("\n", "\\n");
    }
}
