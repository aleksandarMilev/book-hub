namespace BookHub.Server.Features.Notification.Service
{
    public interface INotificationService
    {
        Task<int> CreateAuthorNotificationAsync(int authorId, string authorName);

        Task<int> CreateBookNotificationAsync(int bookId, string title);
    }
}
