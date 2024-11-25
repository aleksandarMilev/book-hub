namespace BookHub.Server.Features.UserProfile.Service
{
    using AutoMapper;
    using Data;

    public class ProfileService(
        BookHubDbContext data,
        IMapper mapper) : IProfileService
    {
        private readonly BookHubDbContext data = data;
        private readonly IMapper mapper;
    }
}
