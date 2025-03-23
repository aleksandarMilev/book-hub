namespace BookHub.Features.Search.Service.Models
{
    public class SearchChatServiceModel
    {
        public int Id { get; init; }

        public string Name { get; init; } = null!;

        public string ImageUrl { get; set; } = null!;

        public string CreatorId { get; init; } = null!;
    }
}
