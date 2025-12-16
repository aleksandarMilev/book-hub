namespace BookHub.Features.Search.Service.Models;

public class SearchChatServiceModel
{
    public Guid Id { get; init; }

    public string Name { get; init; } = null!;

    public string ImagePath { get; set; } = null!;

    public string CreatorId { get; init; } = null!;
}
