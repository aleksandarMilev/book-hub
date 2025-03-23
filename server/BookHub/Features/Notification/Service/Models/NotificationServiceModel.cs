namespace BookHub.Features.Notification.Service.Models
{
    public class NotificationServiceModel
    {
        public int Id { get; init; }

        public string Message { get; init; } = null!;

        public bool IsRead { get; init; }

        public DateTime CreatedOn { get; init; }

        public int ResourceId { get; init; }

        public string ResourceType { get; init; } = null!;
    }
}
