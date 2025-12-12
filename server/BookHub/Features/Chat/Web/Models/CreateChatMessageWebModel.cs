namespace BookHub.Features.Chat.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Shared.Constants.Validation;

    public class CreateChatMessageWebModel
    {
        [Required]
        [StringLength(
            MessageMaxLength,
            MinimumLength = MessageMinLength)]
        public string Message { get; init; } = null!;

        public Guid ChatId { get; init; }
    }
}
