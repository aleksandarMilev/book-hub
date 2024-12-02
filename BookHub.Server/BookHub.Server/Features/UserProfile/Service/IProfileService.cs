namespace BookHub.Server.Features.UserProfile.Service
{
    using Infrastructure.Services;
    using Models;

    public interface IProfileService
    {
        Task<ProfileServiceModel?> MineAsync();

        Task<IEnumerable<ProfileServiceModel>> TopThreeAsync();

        Task<IProfileServiceModel?> OtherUserAsync(string id);

        Task<string> CreateAsync(CreateProfileServiceModel model);

        Task<Result> EditAsync(CreateProfileServiceModel model);

        Task<Result> DeleteAsync();

        Task UpdateCountAsync(
            string userId,
            string propName,
            Func<int, int> updateFunc);

        Task<bool> MoreThanFiveCurrentlyReadingAsync(string userId);
    }
}
