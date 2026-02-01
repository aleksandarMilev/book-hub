namespace BookHub.Infrastructure.Validation;

using System.ComponentModel.DataAnnotations;

using static Common.Utils;

public sealed class ImageUploadAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(
        object? value,
        ValidationContext validationContext)
    {
        if (value is null) 
        {
            return ValidationResult.Success;
        }

        if (value is not IFormFile image)
        {
             return new ValidationResult("Invalid file.");
        }

        var validationReuslt = ValidateImageFile(image);
        if (!validationReuslt.Succeeded)
        {
            return new ValidationResult(validationReuslt.ErrorMessage);
        }

        return ValidationResult.Success;
    }
}
