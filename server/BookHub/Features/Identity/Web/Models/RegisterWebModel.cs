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
    public string Username { get; init; } = default!;

    [Required]
    [EmailAddress]
    [StringLength(
        EmailMaxLength,
        MinimumLength = EmailMinLength)]
    public string Email { get; init; } = default!;

    [Required]
    [StringLength(
        PasswordMaxLength,
        MinimumLength = PasswordMinLength)]
    public string Password { get; init; } = default!;

    [Required]
    [StringLength(
        NameMaxLength,
        MinimumLength = NameMinLength)]
    public string FirstName { get; init; } = default!;

    [Required]
    [StringLength(
        NameMaxLength,
        MinimumLength = NameMinLength)]
    public string LastName { get; init; } = default!;

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
