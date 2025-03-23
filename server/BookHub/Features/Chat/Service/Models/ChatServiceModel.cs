namespace BookHub.Features.Chat.Service.Models
{
    public class ChatServiceModel
    {
        public int Id { get; init; }

        public string Name { get; init; } = null!;

        public string ImageUrl { get; set; } = null!;

        public string CreatorId { get; init; } = null!;
    }
}
