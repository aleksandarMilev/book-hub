namespace BookHub.Features.Chat.Service.Models;

public class ProcessChatInvitationServiceModel
{
    public Guid ChatId { get; init; }

    public string ChatName { get; init; } = default!;

    public string ChatCreatorId { get; init; } = default!;
}
