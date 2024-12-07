namespace BookHub.Server.Features.Notification.Service
{
    using Infrastructure.Services;
    using Infrastructure.Services.ServiceLifetimes;
    using Models;

    public interface INotificationService : ITransientService
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

        Task<int> CreateOnChatInvitationAsync(int chatId, string chatName, string receiverId);

        Task<int> CreateOnChatInvitationStatusChangedAsync(
            int chatId,
            string chatName,
            string receiverId,
            bool hasAccepted);

        Task<Result> MarkAsReadAsync(int id);

        Task<Result> DeleteAsync(int id);
    }
}
