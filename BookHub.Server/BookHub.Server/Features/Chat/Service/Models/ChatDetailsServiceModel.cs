namespace BookHub.Server.Features.Chat.Service.Models
{
    using UserProfile.Service.Models;

    public class ChatDetailsServiceModel : ChatServiceModel
    {
        public IEnumerable<PrivateProfileServiceModel> Participants { get; } = new HashSet<PrivateProfileServiceModel>();

        public IEnumerable<ChatMessageServiceModel> Messages { get; } = new HashSet<ChatMessageServiceModel>();
    }
}
