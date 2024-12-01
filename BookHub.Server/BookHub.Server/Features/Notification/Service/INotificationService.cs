namespace BookHub.Server.Features.Notification.Service
{
    using Infrastructure.Services;
    using Models;

    public interface INotificationService
    {
        Task<IEnumerable<NotificationServiceModel>> LastThreeAsync();

        Task<PaginatedModel<NotificationServiceModel>> AllAsync(int pageIndex, int pageSize);

        Task<int> CreateOnEntityCreationAsync(
           int resourceId,
           string resourceType,
           string nameProp,
           string receiverId);

        Task<int> CreateOnEntityApprovalStatusChangeAsync(
           int resourceId,
           string resourceType,
           string nameProp,
           string receiverId,
           bool isApproved);

        Task<Result> MarkAsReadAsync(int id);
    }
}
