namespace BookHub.Features.Chat.Service
{
    using Infrastructure.Services;
    using Infrastructure.Services.ServiceLifetimes;
    using Service.Models;

    public interface IChatMessageService : ITransientService
    {
        Task<ChatMessageServiceModel> Create(CreateChatMessageServiceModel model);

        Task<ResultWith<ChatMessageServiceModel>> Edit(
            int id,
            CreateChatMessageServiceModel model);

        Task<Result> Delete(int id);
    }
}
