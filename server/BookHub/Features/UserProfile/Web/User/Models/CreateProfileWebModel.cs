namespace BookHub.Features.UserProfile.Web.Models;

using BookHub.Infrastructure.Validation;
using System.ComponentModel.DataAnnotations;

using static Shared.Constants.Validation;

public class CreateProfileWebModel
{
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

    [MinAge]
    [MaxAge]
    [Required]
    public DateTime DateOfBirth { get; init; }

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
