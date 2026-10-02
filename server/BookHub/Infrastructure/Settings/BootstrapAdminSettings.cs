namespace BookHub.Infrastructure.Settings;

using System.ComponentModel.DataAnnotations;

using static Common.Constants.Names;

public class BootstrapAdminSettings : IValidatableObject
{
    public bool Enabled { get; init; }

    public string? Email { get; init; }

    public string? Password { get; init; }

    public string? Role { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!this.Enabled)
        {
            yield break;
        }

        if (string.IsNullOrWhiteSpace(this.Email))
        {
            yield return new(
                "BootstrapAdmin:Email is required when BootstrapAdmin:Enabled is true.",
                [nameof(this.Email)]);
        }

        if (string.IsNullOrWhiteSpace(this.Password))
        {
            yield return new(
                "BootstrapAdmin:Password is required when BootstrapAdmin:Enabled is true.",
                [nameof(this.Password)]);
        }

        if (string.IsNullOrWhiteSpace(this.Role))
        {
            yield return new(
                "BootstrapAdmin:Role is required when BootstrapAdmin:Enabled is true.",
                [nameof(this.Role)]);
        }
        else if (this.Role != AdminRoleName)
        {
            // Any other role creates an admin that [Authorize(Roles = ...)] and IAdminService don't recognize.
            yield return new(
                $"BootstrapAdmin:Role must be '{AdminRoleName}'.",
                [nameof(this.Role)]);
        }
    }
}
