namespace BookHub.Server.Features.UserProfile.Service
{
    using Infrastructure.Services;
    using Models;

    public interface IProfileService
    {
        Task<ProfileServiceModel?> GetAsync();

        Task<string> CreateAsync(CreateProfileServiceModel model);

        Task<Result> EditAsync(CreateProfileServiceModel model);

        Task<Result> DeleteAsync();

        Task<bool> HasProfileAsync();
    }
}
