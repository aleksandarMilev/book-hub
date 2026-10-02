namespace BookHub.Infrastructure.Settings;

using System.ComponentModel.DataAnnotations;
using System.Text;

public class JwtSettings : IValidatableObject
{
    // HS256 needs a key of at least 256 bits, or token creation throws IDX10720.
    public const int MinSecretBytes = 32;

    [Required]
    public string Secret { get; init; } = null!;

    public string Issuer { get; init; } = "BookHub";

    public string Audience { get; init; } = "BookHubClient";

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var secretBytes = Encoding.UTF8.GetByteCount(this.Secret ?? string.Empty);
        if (secretBytes < MinSecretBytes)
        {
            yield return new(
                $"JwtSettings:Secret must be at least {MinSecretBytes} bytes (UTF-8); got {secretBytes}.",
                [nameof(this.Secret)]);
        }
    }
}
