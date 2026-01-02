namespace BookHub.Features.UserProfile.Service;

using Infrastructure.Services.Result;
using Infrastructure.Services.ServiceLifetimes;
using Models;

public interface IProfileService : ITransientService
{
    Task<IEnumerable<ProfileServiceModel>> TopThree(
       CancellationToken token = default);

    Task<ProfileServiceModel?> Mine(
        CancellationToken token = default);

    Task<IProfileServiceModel?> OtherUser(
        string id,
        CancellationToken token = default);

    Task<ProfileServiceModel> Create(
        CreateProfileServiceModel model,
        string userId,
        CancellationToken token = default);

    Task<Result> Edit(
        CreateProfileServiceModel model,
        CancellationToken token = default);

    Task<Result> Delete(
        string? userId = null,
        CancellationToken token = default);

    Task<Result> UpdateCount(
        string userId,
        string propName,
        Func<int, int> updateFunc,
        CancellationToken token = default);
}
