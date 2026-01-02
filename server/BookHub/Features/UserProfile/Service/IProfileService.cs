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

    Task IncrementCreatedBooksCount(
        string userId,
        CancellationToken cancellationToken = default);

    Task IncrementCreatedAuthorsCount(
        string userId,
        CancellationToken cancellationToken = default);

    Task IncrementWrittenReviewsCount(
        string userId,
        CancellationToken cancellationToken = default);

    Task IncrementCurrentlyReadingBooksCount(
        string userId,
        CancellationToken cancellationToken = default);

    Task IncrementToReadBooksCount(
        string userId,
        CancellationToken cancellationToken = default);

    Task IncrementReadBooksCount(
        string userId,
        CancellationToken cancellationToken = default);

    Task DecrementCurrentlyReadingBooksCount(
        string userId,
        CancellationToken cancellationToken = default);

    Task DecrementToReadBooksCount(
        string userId,
        CancellationToken cancellationToken = default);

    Task DecrementReadBooksCount(
        string userId,
        CancellationToken cancellationToken = default);
}
