namespace BookHub.Server.Features.Notification.Service
{
    using Data;
    using Data.Models;
    using Infrastructure.Services;

    using static Common.Messages.Notifications;

    public class NotificationService(
        BookHubDbContext data,
        ICurrentUserService userService) : INotificationService
    {
        private readonly BookHubDbContext data = data;
        private readonly ICurrentUserService userService = userService;

        public async Task<int> CreateAuthorNotificationAsync(int authorId, string authorName)
        {
            var username = this.userService.GetUsername();
            var message = string.Format(NotificationCreated, username, authorName);
            var notification = new AuthorNotification()
            {
                AuthorId = authorId,
                Message = message
            };

            this.data.Add(notification);
            await this.data.SaveChangesAsync();

            return notification.Id;
        }

        public async Task<int> CreateBookNotificationAsync(int bookId, string title)
        {
            var username = this.userService.GetUsername();
            var message = string.Format(NotificationCreated, username, title);
            var notification = new BookNotification()
            {
                BookId = bookId,
                Message = message
            };

            this.data.Add(notification);
            await this.data.SaveChangesAsync();

            return notification.Id;
        }
    }
}
