namespace BookHub.Features.Statistics.Data.Queries.AllStatistics;

using BookHub.Features.Statistics.Data.Models;
using Infrastructure.Services.ServiceLifetimes;

public interface IStatisticsQuery : ITransientService
{
    Task<StatisticsRow> All(
        CancellationToken cancellationToken);
}
