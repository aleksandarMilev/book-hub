namespace BookHub.Server.Features.Nationality.Service
{
    public interface INationalityService
    {
        Task<IEnumerable<string>> GetNamesAsync();
    }
}
