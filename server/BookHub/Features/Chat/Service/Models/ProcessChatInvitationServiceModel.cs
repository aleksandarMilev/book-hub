namespace BookHub.Features.Chat.Service.Models;

public class ProcessChatInvitationServiceModel
{
    public Guid ChatId { get; init; }

    public string ChatName { get; init; } = null!;

    public string ChatCreatorId { get; init; } = null!;
}
