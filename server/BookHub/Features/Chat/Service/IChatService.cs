namespace BookHub.Features.Chat.Service
{
    using Infrastructure.Services;
    using Infrastructure.Services.ServiceLifetimes;
    using Service.Models;
    using UserProfile.Service.Models;

    public interface IChatService : ITransientService
    {
        Task<IEnumerable<ChatServiceModel>> NotJoined(string userToJoinId);

        Task<ChatDetailsServiceModel?> Details(int id);

        Task<bool> CanAccessChat(int chatId, string userId);

        Task<bool> IsInvited(int chatId, string userId);

        Task<int> Create(CreateChatServiceModel model);

        Task<Result> Edit(int id, CreateChatServiceModel model);

        Task<Result> Delete(int id);

        Task<Result> InviteUserToChat(
            int chatId,
            string chatName,
            string userId);

        Task<Result> RemoveUserFromChat(int chatId, string userToRemoveId);

        Task<ResultWith<PrivateProfileServiceModel>> Accept(
            int chatId,
            string chatName,
            string chatCreatorId);

        Task<Result> Reject(
            int chatId,
            string chatName,
            string chatCreatorId);
    }
}
