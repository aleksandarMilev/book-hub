namespace BookHub.Server.Features.UserProfile.Service
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using BookHub.Server.Common;
    using Data.Models;
    using Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Server.Data;

    using static Common.ErrorMessage;
    using static Shared.ValidationConstants;

    public class ProfileService(
        BookHubDbContext data,
        ICurrentUserService userService,
        IMapper mapper) : IProfileService
    {
        private readonly BookHubDbContext data = data;
        private readonly ICurrentUserService userService = userService;
        private readonly IMapper mapper = mapper;

        public async Task<bool> HasProfileAsync()
            => await this.data
                .Profiles
                .AnyAsync(p => p.UserId == this.userService.GetId());

        public async Task<IEnumerable<ProfileServiceModel>> TopThreeAsync()
            => await this.data
                .Profiles
                .OrderByDescending(p => 
                    p.CreatedBooksCount + 
                    p.CreatedAuthorsCount + 
                    p.ReviewsCount)
                .Take(3)
                .ProjectTo<ProfileServiceModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<ProfileServiceModel?> MineAsync()
            => await this.data
                .Profiles
                .ProjectTo<ProfileServiceModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(p => p.Id == this.userService.GetId());

        public async Task<IProfileServiceModel?> OtherUserAsync(string id)
        {
            var model = await this.data
                .Profiles
                .ProjectTo<ProfileServiceModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (model is null)
            {
                return null;
            }

            if (model.IsPrivate)
            {
                return this.mapper.Map<PrivateProfileServiceModel>(model);
            }

            return model;
        }
         
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
            var userId = this.userService.GetId();

            var profile = await this.data
                 .Profiles
                 .FindAsync(userId);

            if (profile is null)
            {
                return string.Format(DbEntityNotFound, nameof(UserProfile), userId);
            }

            this.mapper.Map(model, profile);

            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<Result> DeleteAsync()
        {
            var userId = this.userService.GetId();

            var profile = await this.data
                .Profiles
                .FindAsync(userId);

            if (profile is null)
            {
                return string.Format(DbEntityNotFound, nameof(UserProfile), userId);
            }

            this.data.Remove(profile);
            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task UpdateCountAsync(
            string userId,
            string propName,
            Func<int, int> updateFunc)
        {
            var profile = await this.data
               .Profiles
               .FindAsync(userId)
               ?? throw new DbEntityNotFoundException<string>(nameof(UserProfile), userId);

            var property = typeof(UserProfile)
                .GetProperty(propName)
                ?? throw new ArgumentException(propName);

            var currentValue = (int)property.GetValue(profile)!;
            var updatedValue = updateFunc(currentValue);
            property.SetValue(profile, updatedValue);

            await this.data.SaveChangesAsync();
        }

        public async Task<bool> MoreThanFiveCurrentlyReadingAsync(string userId)
            => await this.data
                .Profiles
                .Where(p => p.UserId == userId)
                .Select(p => p.CurrentlyReadingBooksCount)
                .FirstOrDefaultAsync() == CurrentlyReadingBooksMaxCount;
    }
}
