namespace BookHub.Features.UserProfile.Service
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using BookHub.Data;
    using Common.Exceptions;
    using Data.Models;
    using Infrastructure.Services.CurrentUser;
    using Infrastructure.Services.Result;
    using Microsoft.EntityFrameworkCore;
    using Models;

    using static Common.Constants.ErrorMessages;
    using static Shared.ValidationConstants;

    public class ProfileService(
        BookHubDbContext data,
        ICurrentUserService userService,
        IMapper mapper) : IProfileService
    {
        private readonly BookHubDbContext data = data;
        private readonly ICurrentUserService userService = userService;
        private readonly IMapper mapper = mapper;

        public async Task<IEnumerable<ProfileServiceModel>> TopThree()
            => await this.data
                .Profiles
                .OrderByDescending(p =>
                    p.CreatedBooksCount +
                    p.CreatedAuthorsCount +
                    p.ReviewsCount)
                .Take(3)
                .ProjectTo<ProfileServiceModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<ProfileServiceModel?> Mine()
            => await this.data
                .Profiles
                .ProjectTo<ProfileServiceModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(p => p.Id == this.userService.GetId());

        public async Task<IProfileServiceModel?> OtherUser(string id)
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

        public async Task<bool> HasProfile()
             => await this.data
                .Profiles
                .AnyAsync(p => p.UserId == this.userService.GetId());

        public async Task<bool> HasMoreThanFiveCurrentlyReading(string userId)
            => await this.data
                .Profiles
                .Where(p => p.UserId == userId)
                .Select(p => p.CurrentlyReadingBooksCount)
                .FirstOrDefaultAsync() == CurrentlyReadingBooksMaxCount;
         
        public async Task<string> Create(CreateProfileServiceModel model)
        {
            var profile = this.mapper.Map<UserProfile>(model);
            profile.UserId = this.userService.GetId()!;

            this.data.Add(profile);
            await this.data.SaveChangesAsync();

            return profile.UserId;
        }

        public async Task<Result> Edit(CreateProfileServiceModel model)
        {
            var userId = this.userService.GetId();

            var profile = await this.data
                 .Profiles
                 .FindAsync(userId);

            if (profile is null)
            {
                return string.Format(
                    DbEntityNotFound,
                    nameof(UserProfile),
                    userId);
            }

            this.mapper.Map(model, profile);

            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<Result> Delete(string userToDeleteId = null!)
        {
            var currentUserId = this.userService.GetId()!;
            userToDeleteId ??= currentUserId;

            var profile = await this.data
                .Profiles
                .FindAsync(userToDeleteId);

            if (profile is null)
            {
                return string.Format(
                    DbEntityNotFound,
                    nameof(UserProfile),
                    userToDeleteId);
            }

            var isNotCurrentUserProfile = profile.UserId != currentUserId;
            var userIsNotAdmin = !this.userService.IsAdmin();

            if (isNotCurrentUserProfile && userIsNotAdmin)
            { 
                return string.Format(
                    UnauthorizedDbEntityAction,
                    this.userService.GetUsername(),
                    nameof(UserProfile),
                    userToDeleteId);
            }

            this.data.Remove(profile);
            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task UpdateCount(
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
    }
}
