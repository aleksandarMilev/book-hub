namespace BookHub.Features.Notification.Mapper
{
    using AutoMapper;
    using Data.Models;
    using Service.Models;

    public class NotificationMapper : Profile
    {
        public NotificationMapper()
            => this.CreateMap<Notification, NotificationServiceModel>();
    }
}
