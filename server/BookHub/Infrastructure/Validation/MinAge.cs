namespace BookHub.Infrastructure.Validation;

using System.ComponentModel.DataAnnotations;
using System.Globalization;

using static Common.Constants.DateFormats;

public sealed class MinAgeAttribute : ValidationAttribute
{
    private readonly int minAgeYears;

    public MinAgeAttribute(int minAgeYears = 13)
    {
        this.minAgeYears = minAgeYears;
        this.ErrorMessage = $"Date of birth must be a valid date and you must be at least {this.minAgeYears} years old.";
    }

    protected override ValidationResult? IsValid(
        object? value,
        ValidationContext validationContext)
    {
        if (value is null)
        {
            return ValidationResult.Success;
        }

        if (!TryGetDateOnly(value, out var dateOfBirth))
        {
            return new ValidationResult(this.ErrorMessage);
        }

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var latestAllowedDateOfBirth = today.AddYears(-this.minAgeYears);

        if (dateOfBirth > today || dateOfBirth > latestAllowedDateOfBirth)
        {
            return new ValidationResult(this.ErrorMessage);
        }

        return ValidationResult.Success;
    }

    private static bool TryGetDateOnly(
        object value,
        out DateOnly date)
    {
        switch (value)
        {
            case DateOnly dateOnly:
                date = dateOnly;
                return true;

            case DateTime dateTime:
                date = DateOnly.FromDateTime(dateTime.ToUniversalTime());
                return true;

            case string asString when !string.IsNullOrWhiteSpace(asString):
                return DateOnly.TryParseExact(
                    asString,
                    ISO8601,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out date);

            default:
                date = default;
                return false;
        }
    }
}
