namespace BookHub.Features.Chat.Service;

using Infrastructure.Services.Result;
using Infrastructure.Services.ServiceLifetimes;
using Service.Models;

public interface IChatMessageService : ITransientService
{
    Task<ResultWith<IEnumerable<ChatMessageServiceModel>>> GetForChat(
        Guid chatId,
        int? before = null,
        int take = 50,
        CancellationToken token = default);

    Task<ResultWith<ChatMessageServiceModel>> Create(
        CreateChatMessageServiceModel model,
        CancellationToken token = default);

    Task<ResultWith<ChatMessageServiceModel>> Edit(
        int id,
        CreateChatMessageServiceModel model,
        CancellationToken token = default);

    Task<Result> Delete(
        int id,
        CancellationToken token = default);
}
