namespace BookHub.Features.Notification.Shared;

using Service.Models;
using Data.Models;

public static class NotificationMapping
{
    public static IQueryable<NotificationServiceModel> ToServiceModels(
        this IQueryable<Notification> dbModels)
        => dbModels.Select(n => new NotificationServiceModel
        {
            Id = n.Id,
            Message = n.Message,
            IsRead = n.IsRead,
            CreatedOn = n.CreatedOn,
            ResourceId = n.ResourceId,
            ResourceType = n.ResourceType,
        });
}
