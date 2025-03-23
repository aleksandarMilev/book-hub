namespace BookHub.Features.Chat.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Shared.ValidationConstants;

    public class ProcessChatInvitationWebModel
    {
        public int ChatId { get; init; }

        [Required]
        [StringLength(
            NameMaxLength,
            MinimumLength = NameMinLength)]
        public string ChatName { get; init; } = null!;

        [Required]
        public string ChatCreatorId { get; init; } = null!;
    }
}
