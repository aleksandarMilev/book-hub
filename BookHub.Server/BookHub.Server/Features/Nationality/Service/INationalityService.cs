namespace BookHub.Server.Features.Nationality.Service
{
    using Models;

    public interface INationalityService
    {
        Task<IEnumerable<NationalityServiceModel>> NamesAsync();
    }
}
