namespace BookHub.Features.Chat.Service;

using Infrastructure.Services.Result;
using Infrastructure.Services.ServiceLifetimes;
using Models;
using UserProfile.Service.Models;

public interface IChatService : ITransientService
{
    Task<ChatDetailsServiceModel?> Details(
        Guid id,
        CancellationToken token = default);

    Task<IEnumerable<ChatServiceModel>> NotJoined(
        string userToJoinId,
        CancellationToken token = default);

    Task<bool> CanAccessChat(
        Guid chatId,
        string userId,
        CancellationToken token = default);

    Task<bool> IsInvited(
        Guid chatId,
        string userId,
        CancellationToken token = default);

    Task<ResultWith<ChatDetailsServiceModel>> Create(
        CreateChatServiceModel serviceModel,
        CancellationToken token = default);

    Task<Result> Edit(
        Guid chatId,
        CreateChatServiceModel serviceModel,
        CancellationToken token = default);

    Task<Result> Delete(
        Guid chatId,
        CancellationToken token = default);

    Task<ResultWith<PrivateProfileServiceModel>> Accept(
        ProcessChatInvitationServiceModel model,
        CancellationToken token = default);

    Task<Result> Reject(
        ProcessChatInvitationServiceModel model,
        CancellationToken token = default);

    Task<Result> InviteUserToChat(
       Guid chatId,
       AddUserToChatServiceModel model,
       CancellationToken token = default);

    Task<Result> RemoveUserFromChat(
        Guid chatId,
        string userToRemoveId,
        CancellationToken token = default);
}