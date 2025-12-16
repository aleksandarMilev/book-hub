namespace BookHub.Infrastructure.Validation;

using System.ComponentModel.DataAnnotations;
using System.Globalization;

using static Common.Constants.DateFormats;

public sealed class DateOfBirthAttribute : ValidationAttribute
{
    private readonly int maxAgeYears;

    public DateOfBirthAttribute(int maxAgeYears)
    {
        this.maxAgeYears = maxAgeYears;
        this.ErrorMessage = $"Date of birth must be a valid date between today and {this.maxAgeYears} years ago.";
    }

    protected override ValidationResult? IsValid(
        object? value,
        ValidationContext validationContext)
    {
        if (value is null)
        { 
            return ValidationResult.Success;
        }

        var valueAsString = value as string;
        if (string.IsNullOrWhiteSpace(valueAsString))
        {
            return ValidationResult.Success;
        }

        if (!DateOnly.TryParseExact(
            valueAsString,
            ISO8601,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out var dateOfBirth))
        {
            return new ValidationResult(this.ErrorMessage);
        }

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var oldestAllowed = today.AddYears(-this.maxAgeYears);

        if (dateOfBirth > today)
        {
            return new ValidationResult(this.ErrorMessage);
        }

        if (dateOfBirth < oldestAllowed)
        {
            return new ValidationResult(this.ErrorMessage);
        }

        return ValidationResult.Success;
    }
}