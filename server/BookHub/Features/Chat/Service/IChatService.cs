namespace BookHub.Features.Chat.Service;

using Infrastructure.Services.Result;
using Infrastructure.Services.ServiceLifetimes;
using Models;
using UserProfile.Service.Models;

public interface IChatService : ITransientService
{
    Task<ChatDetailsServiceModel?> Details(
        Guid chatId,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<ChatServiceModel>> NotJoined(
        string userToJoinId,
        CancellationToken cancellationToken = default);

    Task<bool> CanAccessChat(
        Guid chatId,
        string userId,
        CancellationToken cancellationToken = default);

    Task<bool> IsInvited(
        Guid chatId,
        string userId,
        CancellationToken cancellationToken = default);

    Task<ResultWith<ChatDetailsServiceModel>> Create(
        CreateChatServiceModel serviceModel,
        CancellationToken cancellationToken = default);

    Task<Result> Edit(
        Guid chatId,
        CreateChatServiceModel serviceModel,
        CancellationToken cancellationToken = default);

    Task<Result> Delete(
        Guid chatId,
        CancellationToken cancellationToken = default);

    Task<ResultWith<PrivateProfileServiceModel>> Accept(
        ProcessChatInvitationServiceModel model,
        CancellationToken cancellationToken = default);

    Task<Result> Reject(
        ProcessChatInvitationServiceModel model,
        CancellationToken cancellationToken = default);

    Task<Result> InviteUserToChat(
       Guid chatId,
       AddUserToChatServiceModel model,
       CancellationToken cancellationToken = default);

    Task<Result> RemoveUserFromChat(
        Guid chatId,
        string userToRemoveId,
        CancellationToken cancellationToken = default);
}