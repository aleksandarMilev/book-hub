namespace BookHub.Server.Features.Authors.Service
{
    using Models;

    public interface INationalityService
    {
        Task<IEnumerable<NationalityServiceModel>> NamesAsync();
    }
}
