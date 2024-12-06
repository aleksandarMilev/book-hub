namespace BookHub.Server.Features.Chat.Service
{
    using Infrastructure.Services;
    using Infrastructure.Services.ServiceLifetimes;
    using Service.Models;

    public interface IChatService : ITransientService
    {
        Task<IEnumerable<ChatServiceModel>> AllAsync();

        Task<IEnumerable<ChatServiceModel>> ChatsNotJoinedAsync(string userToJoinId);

        Task<ChatDetailsServiceModel?> DetailsAsync(int id);

        Task<int> CreateAsync(CreateChatServiceModel model);

        Task<(int, string)> AddUserToChatAsync(int chatId, string userId);

        Task<Result> EditAsync(int id);

        Task<Result> DeleteAsync(int id);
    }
}
