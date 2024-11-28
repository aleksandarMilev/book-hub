namespace BookHub.Server.Features.Notification.Service.Models.Base
{
    public abstract class NotificationServiceModel
    {
        public int Id { get; init; }

        public string Message { get; init; } = null!;

        public bool IsRead { get; init; }
    }
}
