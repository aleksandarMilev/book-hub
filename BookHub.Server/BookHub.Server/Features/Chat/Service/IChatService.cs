namespace BookHub.Server.Features.Chat.Service
{
    using Infrastructure.Services;
    using Infrastructure.Services.ServiceLifetimes;
    using Service.Models;

    public interface IChatService : ITransientService
    {
        Task<IEnumerable<ChatServiceModel>> AllAsync();

        Task<IEnumerable<ChatServiceModel>> NotJoinedAsync(string userToJoinId);

        Task<bool> CanAccessChatAsync(int chatId, string userId);

        Task<bool> IsInvitedAsync(int chatId, string userId);

        Task<ChatDetailsServiceModel?> DetailsAsync(int id);

        Task<int> CreateAsync(CreateChatServiceModel model);

        Task<(int, string)> InviteUserToChatAsync(int chatId, string chatName, string userId);

        Task<Result> AcceptAsync(
            int chatId,
            string chatName,
            string chatCreatorId);

        Task<Result> RejectAsync(
            int chatId,
            string chatName,
            string chatCreatorId);

        Task<Result> RemoveUserAsync(int chatId, string userToRemoveId);

        Task<Result> EditAsync(int id, CreateChatServiceModel model);

        Task<Result> DeleteAsync(int id);
    }
}
