namespace BookHub.Server.Features.UserProfile.Service
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;
    using Models;

    using static Common.Messages.Error.Profile;

    public class ProfileService(
        BookHubDbContext data,
        ICurrentUserService userService,
        IMapper mapper) : IProfileService
    {
        private readonly BookHubDbContext data = data;
        private readonly ICurrentUserService userService = userService;
        private readonly IMapper mapper = mapper;

        public async Task<ProfileServiceModel?> GetAsync()
            => await this.data
                .Profiles
                .ProjectTo<ProfileServiceModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(p => p.Id == this.userService.GetId()!);

        public async Task<string> CreateAsync(CreateProfileServiceModel model)
        {
            var profile = this.mapper.Map<UserProfile>(model);
            profile.UserId = this.userService.GetId()!;

            this.data.Add(profile);
            await this.data.SaveChangesAsync();

            return profile.UserId;
        }

        public async Task<Result> EditAsync(CreateProfileServiceModel model)
        {
            var profile = await this.data
                 .Profiles
                 .FindAsync(this.userService.GetId()!);

            if (profile is null)
            {
                return ProfileNotFound;
            }

            this.mapper.Map(model, profile);

            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<Result> DeleteAsync()
        {
            var profile = await this.data
                .Profiles
                .FindAsync(this.userService.GetId()!);

            if (profile is null)
            {
                return ProfileNotFound;
            }

            this.data.Remove(profile);
            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<bool> HasProfileAsync() 
            => await this.data
                .Profiles
                .AnyAsync(p => p.UserId == this.userService.GetId()!);

    }
}
