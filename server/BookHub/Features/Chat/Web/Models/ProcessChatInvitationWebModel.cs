namespace BookHub.Features.Chat.Web.Models;

using System.ComponentModel.DataAnnotations;

using static Shared.Constants.Validation;

public class ProcessChatInvitationWebModel
{
    public Guid ChatId { get; init; }

    [Required]
    [StringLength(
        NameMaxLength,
        MinimumLength = NameMinLength)]
    public string ChatName { get; init; } = default!;

    [Required]
    public string ChatCreatorId { get; init; } = default!;
}
