namespace BookHub.Infrastructure.Settings;

using System.ComponentModel.DataAnnotations;

public class AppUrlsSettings
{
    [Required(ErrorMessage = "AppUrlsSettings:ClientBaseUrl is required.")]
    [Url(ErrorMessage = "AppUrlsSettings:ClientBaseUrl must be an absolute http(s) URL.")]
    public string ClientBaseUrl { get; init; } = default!;
}
