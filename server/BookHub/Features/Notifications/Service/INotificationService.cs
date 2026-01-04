namespace BookHub.Features.Notifications.Service;

using Common;
using Infrastructure.Services.Result;
using Infrastructure.Services.ServiceLifetimes;
using Models;

public interface INotificationService : ITransientService
{
    Task<IEnumerable<NotificationServiceModel>> LastThree(
        CancellationToken cancellationToken = default);

    Task<PaginatedModel<NotificationServiceModel>> All(
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<Guid> CreateOnBookCreation(
        Guid bookId,
        string bookTitle,
        string receiverId,
        CancellationToken cancellationToken = default);

    Task<Guid> CreateOnAuthorCreation(
        Guid authorId,
        string authorName,
        string receiverId,
        CancellationToken cancellationToken = default);

    Task<Guid> CreateOnBookApproved(
        Guid bookId,
        string bookTitle,
        string receiverId,
        CancellationToken cancellationToken = default);

    Task<Guid> CreateOnAuthorApproved(
        Guid authorId,
        string authorName,
        string receiverId,
        CancellationToken cancellationToken = default);

    Task<Guid> CreateOnBookRejected(
        Guid bookId,
        string bookTitle,
        string receiverId,
        CancellationToken cancellationToken = default);

    Task<Guid> CreateOnAuthorRejected(
        Guid authorId,
        string authorName,
        string receiverId,
        CancellationToken cancellationToken = default);

    Task<Guid> CreateOnChatInvitation(
        Guid chatId,
        string chatName,
        string receiverId,
        CancellationToken cancellationToken = default);

    Task<Guid> CreateOnChatInvitationAccepted(
        Guid chatId,
        string chatName,
        string receiverId,
        CancellationToken cancellationToken = default);

    Task<Guid> CreateOnChatInvitationRejected(
        Guid chatId,
        string chatName,
        string receiverId,
        CancellationToken cancellationToken = default);

    Task<Result> Delete(
        Guid notificationId,
        CancellationToken cancellationToken = default);

    Task<Result> MarkAsRead(
        Guid notificationId,
        CancellationToken cancellationToken = default);
}
