namespace BookHub.Features.Identity.Web.Models;

using System.ComponentModel.DataAnnotations;
using Infrastructure.Validation;

using static Features.UserProfile.Shared.Constants.Validation;
using static Shared.Constants.Validation;

public class RegisterWebModel
{
    [Required]
    [StringLength(
        UsernameMaxLength,
        MinimumLength = UsernameMinLength)]
    public string Username { get; init; } = null!;

    [Required]
    [EmailAddress]
    [StringLength(
        EmailMaxLength,
        MinimumLength = EmailMinLength)]
    public string Email { get; init; } = null!;

    [Required]
    [StringLength(
        PasswordMaxLength,
        MinimumLength = PasswordMinLength)]
    public string Password { get; init; } = null!;

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

    [DateOfBirth(MaxAgeYears)]
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
}
