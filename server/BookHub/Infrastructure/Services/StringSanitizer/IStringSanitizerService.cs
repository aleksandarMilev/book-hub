namespace BookHub.Infrastructure.Services.StringSanitizer;

using ServiceLifetimes;

public interface IStringSanitizerService : ITransientService
{
    string SanitizeStringForLog(string value);
}
