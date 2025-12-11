namespace BookHub.Features.Notification.Service.Models;

public class NotificationServiceModel
{
    public Guid Id { get; init; }

    public string Message { get; init; } = null!;

    public bool IsRead { get; init; }

    public DateTime CreatedOn { get; init; }

    public Guid ResourceId { get; init; }

    public string ResourceType { get; init; } = null!;
}
