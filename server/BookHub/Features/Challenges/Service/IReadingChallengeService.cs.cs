namespace BookHub.Features.Challenges.Service;

using Infrastructure.Services.Result;
using Infrastructure.Services.ServiceLifetimes;
using Models;

public interface IReadingChallengeService : ITransientService
{
    Task<ReadingChallengeServiceModel?> Get(
        int year,
        CancellationToken cancellationToken = default);

    Task<Result> Upsert(
        UpsertReadingChallengeServiceModel serviceModel,
        CancellationToken cancellationToken = default);

    Task<ReadingChallengeProgressServiceModel?> Progress(
        int year,
        CancellationToken cancellationToken = default);

    Task<Result> CheckInToday(
        CancellationToken cancellationToken = default);

    Task<ReadingStreakServiceModel> Streak(
        CancellationToken cancellationToken = default);
}
