namespace BookHub.Features.Statistics.Service;

using Data.Queries.AllStatistics;
using Microsoft.Extensions.Caching.Memory;
using Models;

public class StatisticsService(
    IStatisticsQuery data,
    IMemoryCache cache) : IStatisticsService
{
    private const string CacheKey = "home_statistics";
    private static readonly SemaphoreSlim lockObject = new(1, 1);

    public async Task<StatisticsServiceModel> All(
        CancellationToken cancellationToken = default)
    {
        if (this.TryGetCached(out var cached))
        {
            return cached;
        }

        await lockObject.WaitAsync(cancellationToken);

        try
        {
            if (this.TryGetCached(out cached))
            {
                return cached;
            }

            var statistics = await data.All(cancellationToken);
            var serviceModel = new StatisticsServiceModel(
                statistics.Profiles,
                statistics.Books,
                statistics.Authors,
                statistics.Reviews,
                statistics.Genres,
                statistics.Articles);

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            };

            cache.Set(
                CacheKey,
                serviceModel,
                cacheOptions);

            return serviceModel;
        }
        finally
        {
            lockObject.Release();
        }
    }

    private bool TryGetCached(out StatisticsServiceModel value)
    {
        var isCached = cache.TryGetValue(
            CacheKey,
            out StatisticsServiceModel? cached);

        if (isCached && cached is not null)
        {
            value = cached;
            return true;
        }

        value = null!;
        return false;
    }
}
