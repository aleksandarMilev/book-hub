namespace BookHub.Features.Identity.Web.Models;

using System.ComponentModel.DataAnnotations;

using static Shared.Constants.Validation;

public class LoginWebModel
{
    [Required]
    [StringLength(
       CredentialsMaxLength,
       MinimumLength = CredentialsMinLength)]
    public string Credentials { get; init; } = null!;

    [Required]
    [StringLength(
       PasswordMaxLength,
       MinimumLength = PasswordMinLength)]
    public string Password { get; init; } = null!;

    public bool RememberMe { get; init; }
}
