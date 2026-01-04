namespace BookHub.Features.Statistics.Service;

using Infrastructure.Services.ServiceLifetimes;
using Models;

public interface IStatisticsService : ITransientService
{
    Task<StatisticsServiceModel> All(
        CancellationToken cancellationToken = default);
}
