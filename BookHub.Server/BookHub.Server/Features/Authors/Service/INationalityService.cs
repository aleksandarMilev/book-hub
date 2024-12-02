namespace BookHub.Server.Features.Authors.Service
{
    using Infrastructure.Services.ServiceLifetimes;
    using Models;

    public interface INationalityService : ITransientService
    {
        Task<IEnumerable<NationalityServiceModel>> NamesAsync();
    }
}
