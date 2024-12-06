namespace BookHub.Server.Features.Chat.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Common.Constants.Validation.Chat;

    public class CreateChatMessageWebModel
    {
        [Required]
        [StringLength(MessageMaxLength, MinimumLength = MessageMinLength)]
        public string Message { get; init; } = null!;

        public int ChatId { get; init; }
    }
}
