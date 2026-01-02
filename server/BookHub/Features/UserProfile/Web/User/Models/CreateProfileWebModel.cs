namespace BookHub.Features.UserProfile.Web.Models;

using System.ComponentModel.DataAnnotations;

using static Shared.Constants.Validation;

public class CreateProfileWebModel
{
    [Required]
    [StringLength(
        NameMaxLength,
        MinimumLength = NameMinLength)]
    public string FirstName { get; init; } = null!;

    [Required]
    [StringLength(
        NameMaxLength,
        MinimumLength = NameMinLength)]
    public string LastName { get; init; } = null!;

    public IFormFile? Image { get; init; }

    public string? DateOfBirth { get; init; }

    [StringLength(
        UrlMaxLength,
        MinimumLength = UrlMinLength)]
    public string? SocialMediaUrl { get; init; }

    [StringLength(
        BiographyMaxLength,
        MinimumLength = BiographyMinLength)]
    public string? Biography { get; init; }

    public bool IsPrivate { get; init; }

    public bool RemoveImage { get; init; }
}
