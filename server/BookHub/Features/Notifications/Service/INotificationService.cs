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

    /// <summary>
    /// Adds one notification per receiver to the change tracker without saving.
    /// The caller's SaveChangesAsync persists them together with its own changes.
    /// </summary>
    void AddOnBookCreation(
        Guid bookId,
        string bookTitle,
        IEnumerable<string> receiverIds);

    /// <inheritdoc cref="AddOnBookCreation"/>
    void AddOnBookEdition(
        Guid bookId,
        string bookTitle,
        IEnumerable<string> receiverIds);

    /// <inheritdoc cref="AddOnBookCreation"/>
    void AddOnAuthorCreation(
        Guid authorId,
        string authorName,
        IEnumerable<string> receiverIds);

    /// <inheritdoc cref="AddOnBookCreation"/>
    void AddOnAuthorEdition(
        Guid authorId,
        string authorName,
        IEnumerable<string> receiverIds);

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

    Task<Result> Delete(
        Guid notificationId,
        CancellationToken cancellationToken = default);

    Task<Result> MarkAsRead(
        Guid notificationId,
        CancellationToken cancellationToken = default);
}
