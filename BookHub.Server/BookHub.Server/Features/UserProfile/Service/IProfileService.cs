namespace BookHub.Server.Features.UserProfile.Service
{
    using Infrastructure.Services;
    using Infrastructure.Services.ServiceLifetimes;
    using Models;

    public interface IProfileService : ITransientService
    {
        Task<bool> HasProfileAsync();

        Task<IEnumerable<ProfileServiceModel>> TopThreeAsync();

        Task<ProfileServiceModel?> MineAsync();

        Task<IProfileServiceModel?> OtherUserAsync(string id);

        Task<string> CreateAsync(CreateProfileServiceModel model);

        Task<Result> EditAsync(CreateProfileServiceModel model);

        Task<Result> DeleteAsync(string userId = null!);

        Task UpdateCountAsync(
            string userId,
            string propName,
            Func<int, int> updateFunc);

        Task<bool> MoreThanFiveCurrentlyReadingAsync(string userId);
    }
}
