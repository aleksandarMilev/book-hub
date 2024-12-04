namespace BookHub.Server.Features.Identity.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class LoginRequestModel
    {
        [Required]
        public string Credentials { get; init; } = null!;

        [Required]
        public string Password { get; init; } = null!;
    }
}
