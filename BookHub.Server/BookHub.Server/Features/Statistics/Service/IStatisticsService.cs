namespace BookHub.Server.Features.Statistics.Service
{
    using Models;

    public interface IStatisticsService
    {
        Task<StatisticsServiceModel> GetAsync();
    }
}
