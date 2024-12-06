namespace BookHub.Server.Features.Chat.Service.Models
{
    public class CreateChatMessageServiceModel 
    {
        public string Message { get; init; } = null!;

        public int ChatId { get; init; }
    }
}
