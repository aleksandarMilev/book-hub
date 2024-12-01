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

        Task IncrementCountAsync(string userId, string propName);
    }
}
