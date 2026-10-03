namespace BookHub.Features.Notifications.Service;

using BookHub.Data;
using Common;
using Data.Models;
using Infrastructure.Services.CurrentUser;
using Infrastructure.Services.PageClamper;
using Infrastructure.Services.Result;
using Microsoft.EntityFrameworkCore;
using Service.Models;
using Shared;

using static Common.Constants.ErrorMessages;
using static Shared.Constants;

public class NotificationService(
    BookHubDbContext data,
    ICurrentUserService userService,
    IPageClamper pageClamper,
    ILogger<NotificationService> logger) : INotificationService
{
    private const string ResourceName = "notification";

    public async Task<IEnumerable<NotificationServiceModel>> LastThree(
        CancellationToken cancellationToken = default)
        => await data
            .Notifications
            .AsNoTracking()
            .Where(n => n.ReceiverId == userService.GetId()!)
            .OrderByDescending(n => n.CreatedOn)
            .Take(3)
            .ToServiceModels()
            .ToListAsync(cancellationToken);

    public async Task<PaginatedModel<NotificationServiceModel>> All(
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        pageClamper.ClampPageSizeAndIndex(
            ref pageIndex,
            ref pageSize);

        var notifications = data
            .Notifications
            .AsNoTracking()
            .Where(n => n.ReceiverId == userService.GetId())
            .OrderBy(n => n.IsRead)
            .ThenByDescending(n => n.CreatedOn)
            .ToServiceModels();

        var total = await notifications.CountAsync(cancellationToken);
        var items = await notifications
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedModel<NotificationServiceModel>(
            items,
            total,
            pageIndex,
            pageSize);
    }

    public void AddOnBookCreation(
        Guid bookId,
        string bookTitle,
        IEnumerable<string> receiverIds)
    {
        var message = string.Format(
            Messages.Created,
            userService.GetUsername(),
            bookTitle);

        this.AddNotifications(
            bookId,
            ResourceType.Book,
            message,
            receiverIds);
    }

    public void AddOnBookEdition(
        Guid bookId,
        string bookTitle,
        IEnumerable<string> receiverIds)
    {
        var message = string.Format(
            Messages.Edited,
            userService.GetUsername(),
            bookTitle);

        this.AddNotifications(
            bookId,
            ResourceType.Book,
            message,
            receiverIds);
    }

    public void AddOnAuthorCreation(
        Guid authorId,
        string authorName,
        IEnumerable<string> receiverIds)
    {
        var message = string.Format(
            Messages.Created,
            userService.GetUsername(),
            authorName);

        this.AddNotifications(
            authorId,
            ResourceType.Author,
            message,
            receiverIds);
    }

    public void AddOnAuthorEdition(
        Guid authorId,
        string authorName,
        IEnumerable<string> receiverIds)
    {
        var message = string.Format(
            Messages.Edited,
            userService.GetUsername(),
            authorName);

        this.AddNotifications(
            authorId,
            ResourceType.Author,
            message,
            receiverIds);
    }

    public async Task<Guid> CreateOnBookApproved(
        Guid bookId,
        string bookTitle,
        string receiverId,
        CancellationToken cancellationToken = default)
    {
        var message = string.Format(
            Messages.Approved,
            bookTitle);

        return await this.CreateNewNotification(
            bookId,
            ResourceType.Book,
            message,
            receiverId,
            cancellationToken);
    }

    public async Task<Guid> CreateOnAuthorApproved(
        Guid authorId,
        string authorName,
        string receiverId,
        CancellationToken cancellationToken = default)
    {
        var message = string.Format(
            Messages.Approved,
            authorName);

        return await this.CreateNewNotification(
            authorId,
            ResourceType.Author,
            message,
            receiverId,
            cancellationToken);
    }

    public async Task<Guid> CreateOnBookRejected(
        Guid bookId,
        string bookTitle,
        string receiverId,
        CancellationToken cancellationToken = default)
    {
        var message = string.Format(
            Messages.Rejected,
            bookTitle);

        return await this.CreateNewNotification(
            bookId,
            ResourceType.Book,
            message,
            receiverId,
            cancellationToken);
    }

    public async Task<Guid> CreateOnAuthorRejected(
        Guid authorId,
        string authorName,
        string receiverId,
        CancellationToken cancellationToken = default)
    {
        var message = string.Format(
            Messages.Rejected,
            authorName);

        return await this.CreateNewNotification(
            authorId,
            ResourceType.Author,
            message,
            receiverId,
            cancellationToken);
    }

    public async Task<Result> Delete(
        Guid notificationId,
        CancellationToken cancellationToken = default)
    {
        var userId = userService.GetId()!;
        var notification = await data
             .Notifications
             .FindAsync(
                [notificationId],
                cancellationToken);

        if (notification is null)
        {
            return this.LogAndReturnNotFound(notificationId);
        }

        // Notifications are private, so someone else's is reported as not found:
        // a 403 would reveal that the notification exists.
        if (notification.ReceiverId != userId)
        {
            return this.LogAndReturnNotFoundForOtherUser(
                notificationId,
                userId);
        }

        data.Remove(notification);
        await data.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<Result> MarkAsRead(
        Guid notificationId,
        CancellationToken cancellationToken = default)
    {
        var rowsAffected = await data
            .Notifications
            .Where(n =>
                n.Id == notificationId &&
                n.ReceiverId == userService.GetId()!)
            .ExecuteUpdateAsync(
                setters => 
                    setters.SetProperty(
                        n => n.IsRead,
                        _ => true), 
                cancellationToken);

        if (rowsAffected == 0)
        {
            return this.LogAndReturnNotFound(notificationId);
        }

        return true;
    }

    private async Task<Guid> CreateNewNotification(
        Guid resourceId,
        ResourceType resourceType,
        string message,
        string receiverId,
        CancellationToken cancellationToken = default)
    {
        var notification = new NotificationDbModel
        {
            ResourceId = resourceId,
            ResourceType = resourceType,
            Message = message,
            ReceiverId = receiverId
        };

        data.Add(notification);
        await data.SaveChangesAsync(cancellationToken);

        return notification.Id;
    }

    private void AddNotifications(
        Guid resourceId,
        ResourceType resourceType,
        string message,
        IEnumerable<string> receiverIds)
    {
        var notifications = receiverIds
            .Distinct()
            .Select(receiverId => new NotificationDbModel
            {
                ResourceId = resourceId,
                ResourceType = resourceType,
                Message = message,
                ReceiverId = receiverId
            });

        data.AddRange(notifications);
    }

    private Result LogAndReturnNotFound(Guid notificationId)
    {
        logger.LogWarning(
            DbEntityNotFoundTemplate,
            nameof(NotificationDbModel),
            notificationId);

        return NotFound();
    }

    private Result LogAndReturnNotFoundForOtherUser(
        Guid notificationId,
        string userId)
    {
        logger.LogWarning(
            UnauthorizedMessageTemplate,
            userId,
            nameof(NotificationDbModel),
            notificationId);

        return NotFound();
    }

    private static Result NotFound()
        => Result.NotFound(string.Format(
            ResourceNotFound,
            ResourceName));
}
