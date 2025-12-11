namespace BookHub.Features.Notification.Service;

using BookHub.Common;
using Infrastructure.Services.Result;
using Infrastructure.Services.ServiceLifetimes;
using Models;

public interface INotificationService : ITransientService
{
    Task<IEnumerable<NotificationServiceModel>> LastThree(
        CancellationToken token = default);

    Task<PaginatedModel<NotificationServiceModel>> All(
        int pageIndex,
        int pageSize,
        CancellationToken token = default);

    Task<Guid> CreateOnEntityCreation(
       Guid resourceId,
       string resourceType,
       string nameProp,
       string receiverId,
       CancellationToken token = default);

    Task<Guid> CreateOnEntityApprovalStatusChange(
       Guid resourceId,
       string resourceType,
       string nameProp,
       string receiverId,
       bool isApproved,
       CancellationToken token = default);

    Task<Guid> CreateOnChatInvitation(
        Guid chatId,
        string chatName,
        string receiverId,
        CancellationToken token = default);

    Task<Guid> CreateOnChatInvitationStatusChanged(
        Guid chatId,
        string chatName,
        string receiverId,
        bool hasAccepted,
        CancellationToken token = default);


    Task<Result> Delete(
        Guid id,
        CancellationToken token = default);

    Task<Result> MarkAsRead(
        Guid id,
        CancellationToken token = default);
}
