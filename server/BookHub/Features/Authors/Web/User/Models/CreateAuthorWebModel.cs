namespace BookHub.Features.Authors.Web.User.Models;

using System.ComponentModel.DataAnnotations;
using Shared;

using static Shared.Constants.Validation;

public class CreateAuthorWebModel : IValidatableObject
{
    [Required]
    [StringLength(
        NameMaxLength,
        MinimumLength = NameMinLength)]
    public string Name { get; init; } = null!;

    public IFormFile? Image { get; init; }

    [Required]
    [StringLength(
        BiographyMaxLength,
        MinimumLength = BiographyMinLength)]
    public string Biography { get; init; } = null!;

    [StringLength(
        PenNameMaxLength,
        MinimumLength = PenNameMinLength)]
    public string? PenName { get; init; }

    public Nationality Nationality { get; init; } = Nationality.Unknown;

    public Gender Gender { get; init; } = Gender.Other;

    public string? BornAt { get; init; }

    public string? DiedAt { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!Enum.IsDefined(typeof(Nationality), this.Nationality))
        {
            yield return new ValidationResult(
                "Invalid nationality value.",
                [nameof(this.Nationality)]);
        }

        if (!Enum.IsDefined(typeof(Gender), this.Gender))
        {
            yield return new ValidationResult(
                "Invalid gender value.",
                [nameof(this.Gender)]);
        }
    }
}
