namespace BookHub.Features.Chat.Service.Models;

public class ChatServiceModel
{
    public Guid Id { get; init; }

    public string Name { get; init; } = default!;

    public string ImagePath { get; init; } = default!;

    public string CreatorId { get; init; } = default!;
}
