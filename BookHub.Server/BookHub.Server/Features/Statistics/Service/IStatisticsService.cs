namespace BookHub.Server.Features.Statistics.Service
{
    using Infrastructure.Services.ServiceLifetimes;
    using Models;

    public interface IStatisticsService : ITransientService
    {
        Task<StatisticsServiceModel> GetAsync();
    }
}
