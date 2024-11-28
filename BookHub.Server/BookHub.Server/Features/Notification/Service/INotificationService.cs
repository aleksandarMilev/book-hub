namespace BookHub.Server.Features.Notification.Service
{
    using Models;

    public interface INotificationService
    {
        Task<IEnumerable<NotificationServiceModel>> LastThreeAsync();

        Task<int> CreateAsync(int resourceId, string resourceType, string nameProp);
    }
}
