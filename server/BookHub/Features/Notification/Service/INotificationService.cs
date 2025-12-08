namespace BookHub.Features.Notification.Service
{
    using BookHub.Common;
    using BookHub.Infrastructure.Services.Result;
    using Infrastructure.Services;
    using Infrastructure.Services.ServiceLifetimes;
    using Models;

    public interface INotificationService : ITransientService
    {
        Task<IEnumerable<NotificationServiceModel>> LastThree();

        Task<PaginatedModel<NotificationServiceModel>> All(int pageIndex, int pageSize);

        Task<int> CreateOnEntityCreation(
           Guid resourceId,
           string resourceType,
           string nameProp,
           string receiverId);

        Task<int> CreateOnEntityApprovalStatusChange(
           Guid resourceId,
           string resourceType,
           string nameProp,
           string receiverId,
           bool isApproved);

        Task<int> CreateOnChatInvitation(
            Guid chatId,
            string chatName,
            string receiverId);

        Task<int> CreateOnChatInvitationStatusChanged(
            Guid chatId,
            string chatName,
            string receiverId,
            bool hasAccepted);

        Task<Result> Delete(int id);

        Task<Result> MarkAsRead(int id);
    }
}
