namespace BookHub.Infrastructure.Settings;

using System.ComponentModel.DataAnnotations;
using System.Text;

public class JwtSettings : IValidatableObject
{
    // HS256 needs a key of at least 256 bits, or token creation throws IDX10720.
    public const int MinSecretBytes = 32;

    [Required]
    public string Secret { get; init; } = null!;

    // Required in every environment (B-21): with an empty issuer or audience, tokens would be
    // issued without them, and outside Development every authenticated request would get a 401.
    // [Required] also rejects whitespace-only values.
    [Required(ErrorMessage = "JwtSettings:Issuer is required (JwtSettings__Issuer, or ISSUER in Compose).")]
    public string Issuer { get; init; } = null!;

    [Required(ErrorMessage = "JwtSettings:Audience is required (JwtSettings__Audience, or AUDIENCE in Compose).")]
    public string Audience { get; init; } = null!;

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
