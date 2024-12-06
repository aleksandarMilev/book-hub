namespace BookHub.Server.Features.Chat.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Common.Constants.Validation.Chat;
    public class CreateChatWebModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        [StringLength(UrlMaxLength, MinimumLength = UrlMinLength)]
        public string? ImageUrl { get; set; }
    }
}
