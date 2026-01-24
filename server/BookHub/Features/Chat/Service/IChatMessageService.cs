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
        CancellationToken cancellationToken = default);

    Task<ResultWith<ChatMessageServiceModel>> Create(
        CreateChatMessageServiceModel model,
        CancellationToken cancellationToken = default);

    Task<ResultWith<ChatMessageServiceModel>> Edit(
        int chatMessageId,
        CreateChatMessageServiceModel model,
        CancellationToken cancellationToken = default);

    Task<Result> Delete(
        int chatMessageId,
        CancellationToken cancellationToken = default);
}
