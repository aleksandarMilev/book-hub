namespace BookHub.Features.Identity.Web.Models;

using System.ComponentModel.DataAnnotations;

using static Identity.Shared.Constants.Validation;

public class ResetPasswordWebModel
{
    [Required]
    [EmailAddress]
    [StringLength(
        EmailMaxLength,
        MinimumLength = EmailMinLength)]
    public string Email { get; init; } = default!;

    [Required]
    public string Token { get; init; } = default!;

    [Required]
    [StringLength(
        PasswordMaxLength,
        MinimumLength = PasswordMinLength)]
    public string NewPassword { get; init; } = default!;
}