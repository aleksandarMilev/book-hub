namespace BookHub.Features.UserProfile.Service
{
    using Infrastructure.Services;
    using Infrastructure.Services.ServiceLifetimes;
    using Models;

    public interface IProfileService : ITransientService
    {
        Task<IEnumerable<ProfileServiceModel>> TopThree();

        Task<ProfileServiceModel?> Mine();

        Task<IProfileServiceModel?> OtherUser(string id);

        Task<bool> HasProfile();

        Task<bool> HasMoreThanFiveCurrentlyReading(string userId);

        Task<string> Create(CreateProfileServiceModel model);

        Task<Result> Edit(CreateProfileServiceModel model);

        Task<Result> Delete(string userId = null!);

        Task UpdateCount(
            string userId,
            string propName,
            Func<int, int> updateFunc);
    }
}
