namespace BookHub.Features.Notification.Service
{
    using BookHub.Infrastructure.Services.Result;
    using Infrastructure.Services;
    using Infrastructure.Services.ServiceLifetimes;
    using Models;

    public interface INotificationService : ITransientService
    {
        Task<IEnumerable<NotificationServiceModel>> LastThree();

        Task<PaginatedModel<NotificationServiceModel>> All(int pageIndex, int pageSize);

        Task<int> CreateOnEntityCreation(
           int resourceId,
           string resourceType,
           string nameProp,
           string receiverId);

        Task<int> CreateOnEntityApprovalStatusChange(
           int resourceId,
           string resourceType,
           string nameProp,
           string receiverId,
           bool isApproved);

        Task<int> CreateOnChatInvitation(
            int chatId,
            string chatName,
            string receiverId);

        Task<int> CreateOnChatInvitationStatusChanged(
            int chatId,
            string chatName,
            string receiverId,
            bool hasAccepted);

        Task<Result> Delete(int id);

        Task<Result> MarkAsRead(int id);
    }
}
