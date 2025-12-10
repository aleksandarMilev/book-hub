namespace BookHub.Features.UserProfile.Web.User.Models;

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

    [Required]
    [StringLength(
        PhoneMaxLength,
        MinimumLength = PhoneMinLength)]
    public string PhoneNumber { get; init; } = null!;

    [Required]
    public string? DateOfBirth { get; init; } = null!;

    [StringLength(
        UrlMaxLength,
        MinimumLength = UrlMinLength)]
    public string? SocialMediaUrl { get; init; }

    [StringLength(
        BiographyMaxLength,
        MinimumLength = BiographyMinLength)]
    public string? Biography { get; init; }

    public bool IsPrivate { get; init; }
}
