namespace BookHub.Server.Features.Chat.Service.Models
{
    public class ChatMessageServiceModel
    {
        public int Id { get; init; }

        public string Message { get; init; } = null!;

        public string SenderId { get; init; } = null!;

        public string SenderName { get; init; } = null!;

        public string SenderImageUrl { get; init; } = null!;

        public DateTime CreatedOn { get; init; }
    }
}
