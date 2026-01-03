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

    Task<Result> IncrementCreatedBooksCount(
        string userId,
        CancellationToken cancellationToken = default);

    Task<Result> IncrementCreatedAuthorsCount(
        string userId,
        CancellationToken cancellationToken = default);

    Task<Result> IncrementWrittenReviewsCount(
        string userId,
        CancellationToken cancellationToken = default);

    Task<Result> IncrementCurrentlyReadingBooksCount(
        string userId,
        CancellationToken cancellationToken = default);

    Task<Result> IncrementToReadBooksCount(
        string userId,
        CancellationToken cancellationToken = default);

    Task<Result> IncrementReadBooksCount(
        string userId,
        CancellationToken cancellationToken = default);

    Task<Result> DecrementCurrentlyReadingBooksCount(
        string userId,
        CancellationToken cancellationToken = default);

    Task<Result> DecrementToReadBooksCount(
        string userId,
        CancellationToken cancellationToken = default);

    Task<Result> DecrementReadBooksCount(
        string userId,
        CancellationToken cancellationToken = default);
}
