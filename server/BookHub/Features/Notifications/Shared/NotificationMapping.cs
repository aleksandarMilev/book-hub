namespace BookHub.Features.Notifications.Shared;

using Data.Models;
using Service.Models;

public static class NotificationMapping
{
    public static IQueryable<NotificationServiceModel> ToServiceModels(
        this IQueryable<NotificationDbModel> dbModels)
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
