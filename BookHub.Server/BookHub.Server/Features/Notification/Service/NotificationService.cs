namespace BookHub.Server.Features.Notification.Service
{
    using Areas.Admin.Service;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;
    using Service.Models;

    using static Common.Messages.Notifications;

    public class NotificationService(
        BookHubDbContext data,
        ICurrentUserService userService,
        IAdminService adminService,
        IMapper mapper) : INotificationService
    {
        private readonly BookHubDbContext data = data;
        private readonly ICurrentUserService userService = userService;
        private readonly IAdminService adminService = adminService;
        private readonly IMapper mapper = mapper;

        public async Task<int> CreateAsync(int resourceId, string resourceType, string nameProp)
        {
            var username = this.userService.GetUsername();
            var message = string.Format(NotificationCreated, username, nameProp);

            var notification = new Notification()
            {
                ResourceId = resourceId,
                ResourceType = resourceType,
                Message = message,
                ReceiverId = await this.adminService.GetIdAsync()
            };

            this.data.Add(notification);
            await this.data.SaveChangesAsync();

            return notification.Id;
        }

        public async Task<IEnumerable<NotificationServiceModel>> LastThreeAsync()
            => await this.data
                .Notifications
                .Where(n => n.ReceiverId == this.userService.GetId())
                .ProjectTo<NotificationServiceModel>(this.mapper.ConfigurationProvider)
                .OrderByDescending(n => n.CreatedOn)
                .Take(3)
                .ToListAsync();
    }
}
