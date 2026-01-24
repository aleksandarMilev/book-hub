namespace BookHub.Features.Chat.Service.Models;

public class AddUserToChatServiceModel
{
    public string UserId { get; init; } = default!;

    public string ChatName { get; init; } = default!;
}
