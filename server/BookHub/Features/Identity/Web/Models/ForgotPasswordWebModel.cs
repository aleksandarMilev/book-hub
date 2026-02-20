namespace BookHub.Features.Identity.Web.Models;

using System.ComponentModel.DataAnnotations;

using static Shared.Constants.Validation;

public class ForgotPasswordWebModel
{
    [Required]
    [EmailAddress]
    [StringLength(
        EmailMaxLength,
        MinimumLength = EmailMinLength)]
    public string Email { get; init; } = default!;
}