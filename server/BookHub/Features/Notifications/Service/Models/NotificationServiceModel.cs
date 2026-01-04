namespace BookHub.Features.Notifications.Service.Models;

using Shared;

public class NotificationServiceModel
{
    public Guid Id { get; init; }

    public string Message { get; init; } = null!;

    public bool IsRead { get; init; }

    public DateTime CreatedOn { get; init; }

    public Guid ResourceId { get; init; }

    public ResourceType ResourceType { get; init; }
}
