namespace BookHub.Features.Notification.Service;

using BookHub.Common;
using BookHub.Data;
using Chat.Data.Models;
using Data.Models;
using Infrastructure.Services.CurrentUser;
using Infrastructure.Services.Result;
using Microsoft.EntityFrameworkCore;
using Service.Models;
using Shared;

using static Common.Constants.ErrorMessages;
using static Shared.Constants.Messages;
using static Shared.Constants.Statuses;

public class NotificationService(
    BookHubDbContext data,
    ICurrentUserService userService,
    ILogger<NotificationService> logger) : INotificationService
{
    public async Task<IEnumerable<NotificationServiceModel>> LastThree(
        CancellationToken token = default)
        => await data
            .Notifications
            .Where(n => n.ReceiverId == userService.GetId())
            .OrderByDescending(n => n.CreatedOn)
            .Take(3)
            .ToServiceModels()
            .ToListAsync(token);

    public async Task<PaginatedModel<NotificationServiceModel>> All(
        int pageIndex,
        int pageSize,
        CancellationToken token = default)
    {
        var notifications = data
            .Notifications
            .Where(n => n.ReceiverId == userService.GetId())
            .OrderBy(n => n.IsRead)
            .ThenByDescending(n => n.CreatedOn)
            .ToServiceModels();

        var total = await notifications.CountAsync(token);
        var paginatedNotifications = await notifications
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(token);

        return new PaginatedModel<NotificationServiceModel>(
            paginatedNotifications,
            total,
            pageIndex,
            pageSize);
    }

    public async Task<Guid> CreateOnEntityCreation(
        Guid resourceId,
        string resourceType,
        string nameProp,
        string receiverId,
        CancellationToken token = default)
    {
        var username = userService.GetUsername()!;
        var message = string.Format(Created, username, nameProp);

        var notification = new Notification
        {
            ResourceId = resourceId,
            ResourceType = resourceType,
            Message = message,
            ReceiverId = receiverId
        };

        data.Add(notification);
        await data.SaveChangesAsync(token);

        return notification.Id;
    }

    public async Task<Guid> CreateOnEntityApprovalStatusChange(
        Guid resourceId,
        string resourceType,
        string nameProp,
        string receiverId,
        bool isApproved,
        CancellationToken token = default)
    {
        var message = string.Format(
            ApprovalStatusChange,
            nameProp,
            isApproved ? Approved : Rejected);

        var notification = new Notification
        {
            ResourceId = resourceId,
            ResourceType = resourceType,
            Message = message,
            ReceiverId = receiverId
        };

        data.Add(notification);
        await data.SaveChangesAsync(token);

        return notification.Id;
    }

    public async Task<Guid> CreateOnChatInvitation(
        Guid chatId,
        string chatName,
        string receiverId,
        CancellationToken token = default)
    {
        var username = userService.GetUsername()!;
        var message = string.Format(ChatInvitation, username, chatName);

        var notification = new Notification
        {
            ResourceId = chatId,
            ResourceType = nameof(ChatDbModel),
            Message = message,
            ReceiverId = receiverId
        };

        data.Add(notification);
        await data.SaveChangesAsync(token);

        return notification.Id;
    }

    public async Task<Guid> CreateOnChatInvitationStatusChanged(
        Guid chatId,
        string chatName,
        string receiverId,
        bool hasAccepted,
        CancellationToken token = default)
    {
        var message = string.Format(
            ChatInvitationStatusChange,
            userService.GetUsername()!,
            hasAccepted
                ? Accepted
                : Rejected,
            chatName);

        var notification = new Notification
        {
            ResourceId = chatId,
            ResourceType = nameof(ChatDbModel),
            Message = message,
            ReceiverId = receiverId
        };

        data.Add(notification);
        await data.SaveChangesAsync(token);

        return notification.Id;
    }

    public async Task<Result> Delete(
        Guid id,
        CancellationToken token = default)
    {
        var userId = userService.GetId()!;
        var notification = await data
             .Notifications
             .FindAsync([id], token);

        if (notification is null)
        {
            return this.LogAndReturnNotFoundMessage(id);
        }

        if (notification.ReceiverId != userId)
        {
            return this.LogAndReturnUnauthorizedMessage(id, userId);
        }

        data.Remove(notification);
        await data.SaveChangesAsync(token);

        return true;
    }

    public async Task<Result> MarkAsRead(
        Guid id,
        CancellationToken token = default)
    {
        var userId = userService.GetId()!;
        var notification = await data
             .Notifications
             .FindAsync([id], token);

        if (notification is null)
        {
            return this.LogAndReturnNotFoundMessage(id);
        }

        if (notification.ReceiverId != userId)
        {
            return this.LogAndReturnUnauthorizedMessage(id, userId);
        }

        notification.IsRead = true;
        await data.SaveChangesAsync(token);

        return true;
    }

    private string LogAndReturnNotFoundMessage(Guid id)
    {
        logger.LogWarning(
            DbEntityNotFoundTemplate,
            nameof(Notification),
            id);

        return string.Format(
            DbEntityNotFound,
            nameof(Notification),
            id);
    }

    private string LogAndReturnUnauthorizedMessage(
        Guid notificationId,
        string userId)
    {
        logger.LogWarning(
            UnauthorizedMessageTemplate,
            userId,
            nameof(Notification),
            notificationId);

        return string.Format(
            UnauthorizedMessage,
            userId,
            nameof(Notification),
            notificationId);
    }
}
