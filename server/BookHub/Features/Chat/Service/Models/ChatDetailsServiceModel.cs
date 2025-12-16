namespace BookHub.Features.Chat.Service.Models;

using UserProfile.Service.Models;

public class ChatDetailsServiceModel : ChatServiceModel
{
    public ICollection<PrivateProfileServiceModel> Participants { get; init; } = new HashSet<PrivateProfileServiceModel>();

    public ICollection<ChatMessageServiceModel> Messages { get; set; } = new HashSet<ChatMessageServiceModel>();
}
