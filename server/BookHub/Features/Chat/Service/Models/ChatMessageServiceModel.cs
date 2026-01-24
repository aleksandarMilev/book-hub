namespace BookHub.Features.Chat.Service.Models;

public class ChatMessageServiceModel
{
    public int Id { get; init; }

    public string Message { get; init; } = default!;

    public string SenderId { get; init; } = default!;

    public string SenderName { get; init; } = default!;

    public string SenderImagePath { get; init; } = default!;

    public DateTime CreatedOn { get; init; }

    public DateTime? ModifiedOn { get; init; }
}
