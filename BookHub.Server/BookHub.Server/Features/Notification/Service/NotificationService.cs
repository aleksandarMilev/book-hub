namespace BookHub.Server.Features.Notification.Service
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data.Models;
    using Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;
    using Server.Data;
    using Service.Models;

    using static Messages;
    using static Common.ErrorMessage;

    public class NotificationService(
        BookHubDbContext data,
        ICurrentUserService userService,
        IMapper mapper) : INotificationService
    {
        private readonly BookHubDbContext data = data;
        private readonly ICurrentUserService userService = userService;
        private readonly IMapper mapper = mapper;

        public async Task<IEnumerable<NotificationServiceModel>> LastThreeAsync()
            => await this.data
                .Notifications
                .Where(n => n.ReceiverId == this.userService.GetId())
                .OrderByDescending(n => n.CreatedOn)
                .Take(3)
                .ProjectTo<NotificationServiceModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<PaginatedModel<NotificationServiceModel>> AllAsync(int pageIndex, int pageSize)
        {
            var notifications = this.data
                .Notifications
                .Where(n => n.ReceiverId == this.userService.GetId())
                .OrderBy(n => n.IsRead)
                .ThenByDescending(n => n.CreatedOn)
                .ProjectTo<NotificationServiceModel>(this.mapper.ConfigurationProvider);

            var total = await notifications.CountAsync();

            var paginatedNotifications = await notifications
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedModel<NotificationServiceModel>(paginatedNotifications, total, pageIndex, pageSize);
        }

        public async Task<int> CreateOnEntityCreationAsync(
            int resourceId,
            string resourceType,
            string nameProp,
            string receiverId)
        {
            var username = this.userService.GetUsername();
            var message = string.Format(Created, username, nameProp);

            var notification = new Notification()
            {
                ResourceId = resourceId,
                ResourceType = resourceType,
                Message = message,
                ReceiverId = receiverId
            };

            this.data.Add(notification);
            await this.data.SaveChangesAsync();

            return notification.Id;
        }

        public async Task<int> CreateOnEntityApprovalStatusChangeAsync(
            int resourceId,
            string resourceType,
            string nameProp,
            string receiverId,
            bool isApproved)
        {
            var message = string.Format(ApprovalStatusChange, nameProp, isApproved ? "approved" : "rejected");

            var notification = new Notification()
            {
                ResourceId = resourceId,
                ResourceType = resourceType,
                Message = message,
                ReceiverId = receiverId
            };

            this.data.Add(notification);
            await this.data.SaveChangesAsync();

            return notification.Id;
        }

        public async Task<int> CreateOnChatInvitationAsync(int chatId, string chatName, string receiverId)
        {
            var username = this.userService.GetUsername();
            var message = string.Format(ChatInvitation, username, chatName);

            var notification = new Notification()
            {
                ResourceId = chatId,
                ResourceType = nameof(Chat),
                Message = message,
                ReceiverId = receiverId
            };

            this.data.Add(notification);
            await this.data.SaveChangesAsync();

            return notification.Id;
        }

        public async Task<int> CreateOnChatInvitationStatusChangedAsync(
            int chatId,
            string chatName,
            string receiverId,
            bool hasAccepted)
        {
            var message = string.Format(
                ChatInvitationStatusChange,
                this.userService.GetUsername(),
                hasAccepted ? "accepted" : "rejected",
                chatName);

            var notification = new Notification()
            {
                ResourceId = chatId,
                ResourceType = nameof(Chat),
                Message = message,
                ReceiverId = receiverId
            };

            this.data.Add(notification);
            await this.data.SaveChangesAsync();

            return notification.Id;
        }

        public async Task<Result> MarkAsReadAsync(int id)
        {
            var notification = await this.data
                 .Notifications
                 .FindAsync(id);

            if (notification is null)
            {
                return string.Format(DbEntityNotFound, nameof(Notification), id);
            }

            notification.IsRead = true;
            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var notification = await this.data
                 .Notifications
                 .FindAsync(id);

            if (notification is null)
            {
                return string.Format(DbEntityNotFound, nameof(Notification), id);
            }

            this.data.Remove(notification);
            await this.data.SaveChangesAsync();

            return true;
        }
    }
}
