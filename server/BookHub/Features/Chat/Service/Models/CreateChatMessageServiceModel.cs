namespace BookHub.Features.Chat.Service.Models;

public class CreateChatMessageServiceModel 
{
    public string Message { get; init; } = default!;

    public Guid ChatId { get; init; }
}
