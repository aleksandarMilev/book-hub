namespace BookHub.Features.Identity.Web.Models;

using System.ComponentModel.DataAnnotations;

public class LoginWebModel
{
    [Required]
    public string Credentials { get; init; } = null!;

    public bool RememberMe { get; init; } 

    [Required]
    public string Password { get; init; } = null!;
}
