namespace BookHub.Features.Chat.Service;

using Infrastructure.Services.Result;
using Infrastructure.Services.ServiceLifetimes;
using Models;
using UserProfile.Service.Models;
using Web.Models;

public interface IChatService : ITransientService
{
    Task<ChatDetailsServiceModel> Details(
        Guid id,
        CancellationToken token = default);

    IEnumerable<ChatServiceModel> NotJoined(
        string userId,
        CancellationToken token = default);

    Task<bool> CanAccessChat(
        Guid chatId,
        string userId,
        CancellationToken token = default);

    Task<bool> IsInvited(
        Guid id,
        string userId,
        CancellationToken token = default);

    Task<int> Create(
        CreateChatWebModel webModel,
        CancellationToken token = default);

    Task<Result> Edit(
        Guid id,
        CreateChatWebModel webModel,
        CancellationToken token = default);

    Task<Result> Delete(
        Guid id,
        CancellationToken token = default);

    Task<ResultWith<PrivateProfileServiceModel>> Accept(
        ProcessChatInvitationWebModel webModel,
        CancellationToken token = default);

    Task<Result> Reject(
        ProcessChatInvitationWebModel webModel,
        CancellationToken token = default);

    Task<int> InviteUser(
       Guid id,
       AddUserToChatWebModel webModel,
       CancellationToken token = default);

    Task<Result> RemoveUser(
        Guid chatId,
        string userId,
        CancellationToken token = default);
}