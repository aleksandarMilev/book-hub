namespace BookHub.Server.Features.Chat.Service
{
    using Infrastructure.Services;
    using Infrastructure.Services.ServiceLifetimes;
    using Service.Models;

    public interface IChatMessageService : ITransientService
    {
        Task<ChatMessageServiceModel> CreateAsync(CreateChatMessageServiceModel model);

        Task<ResultWith<ChatMessageServiceModel>> EditAsync(int id, CreateChatMessageServiceModel model);

        Task<Result> DeleteAsync(int id);
    }
}
