namespace BookHub.Server.Features.Chat.Service
{
    using Infrastructure.Services;
    using Infrastructure.Services.ServiceLifetimes;
    using Service.Models;

    public interface IChatMessageService : ITransientService
    {
        Task<IEnumerable<ChatMessageServiceModel>> AllForChatAsync(int chatId);

        Task<int> CreateAsync(CreateChatMessageServiceModel model);

        Task<Result> EditAsync(int id, CreateChatMessageServiceModel model);

        Task<Result> DeleteAsync(int id);
    }
}
