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

    public async Task<Guid> CreateOnBookCreation(
        Guid bookId,
        string bookTitle,
        string receiverId,
        CancellationToken cancellationToken = default)
    {
        var message = string.Format(
            Messages.Created,
            userService.GetUsername(),
            bookTitle);

        return await this.CreateNewNotification(
            bookId,
            ResourceType.Book,
            message,
            receiverId,
            cancellationToken);
    }

    public async Task<Guid> CreateOnBookEdition(
       Guid bookId,
       string bookTitle,
       string receiverId,
       CancellationToken cancellationToken = default)
    {
        var message = string.Format(
            Messages.Edited,
            userService.GetUsername(),
            bookTitle);

        return await this.CreateNewNotification(
            bookId,
            ResourceType.Book,
            message,
            receiverId,
            cancellationToken);
    }

    public async Task<Guid> CreateOnAuthorCreation(
        Guid authorId,
        string authorName,
        string receiverId,
        CancellationToken cancellationToken = default)
    {
        var message = string.Format(
            Messages.Created,
            userService.GetUsername(),
            authorName);

        return await this.CreateNewNotification(
            authorId,
            ResourceType.Author,
            message,
            receiverId,
            cancellationToken);
    }

    public async Task<Guid> CreateOnAuthorEdition(
        Guid authorId,
        string authorName,
        string receiverId,
        CancellationToken cancellationToken = default)
    {
        var message = string.Format(
            Messages.Edited,
            userService.GetUsername(),
            authorName);

        return await this.CreateNewNotification(
            authorId,
            ResourceType.Author,
            message,
            receiverId,
            cancellationToken);
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
            ResourceType.Book,
            message,
            receiverId,
            cancellationToken);
    }

    public async Task<Guid> CreateOnChatInvitation(
        Guid chatId,
        string chatName,
        string receiverId,
        CancellationToken cancellationToken = default)
    {
        var username = userService.GetUsername()!;
        var message = string.Format(
            Messages.ChatInvitation,
            username,
            chatName);

        return await this.CreateNewNotification(
            chatId,
            ResourceType.Chat,
            message,
            receiverId,
            cancellationToken);
    }

    public async Task<Guid> CreateOnChatInvitationAccepted(
        Guid chatId,
        string chatName,
        string receiverId,
        CancellationToken cancellationToken = default)
    {
        var message = string.Format(
            Messages.ChatInvitationAccepted,
            userService.GetUsername(),
            chatName);

        return await this.CreateNewNotification(
           chatId,
           ResourceType.Chat,
           message,
           receiverId,
           cancellationToken);
    }

    public async Task<Guid> CreateOnChatInvitationRejected(
        Guid chatId,
        string chatName,
        string receiverId,
        CancellationToken cancellationToken = default)
    {
        var message = string.Format(
            Messages.ChatInvitationRejected,
            userService.GetUsername(),
            chatName);

        return await this.CreateNewNotification(
           chatId,
           ResourceType.Chat,
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
            return this.LogAndReturnNotFoundMessage(notificationId);
        }

        if (notification.ReceiverId != userId)
        {
            return this.LogAndReturnUnauthorizedMessage(
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
            return this.LogAndReturnNotFoundMessage(notificationId);
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

    private string LogAndReturnNotFoundMessage(Guid notificationId)
    {
        logger.LogWarning(
            DbEntityNotFoundTemplate,
            nameof(NotificationDbModel),
            notificationId);

        return string.Format(
            DbEntityNotFound,
            nameof(NotificationDbModel),
            notificationId);
    }

    private string LogAndReturnUnauthorizedMessage(
        Guid notificationId,
        string userId)
    {
        logger.LogWarning(
            UnauthorizedMessageTemplate,
            userId,
            nameof(NotificationDbModel),
            notificationId);

        return string.Format(
            UnauthorizedMessage,
            userId,
            nameof(NotificationDbModel),
            notificationId);
    }
}
