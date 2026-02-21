namespace BookHub.Infrastructure.Validation;

using System.ComponentModel.DataAnnotations;
using Services.ImageValidator;

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

        var imageValidator = (IImageValidator)validationContext
            .GetService(typeof(IImageValidator))!;

        if (imageValidator is null) 
        {
            return new ValidationResult("Image validator not configured.");
        }

        var validationReuslt = imageValidator.ValidateImageFile(image);
        if (!validationReuslt.Succeeded)
        {
            return new ValidationResult(validationReuslt.ErrorMessage);
        }

        return ValidationResult.Success;
    }
}
