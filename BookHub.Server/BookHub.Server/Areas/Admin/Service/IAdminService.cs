namespace BookHub.Server.Areas.Admin.Service
{
    using Infrastructure.Services.ServiceLifetimes;

    public interface IAdminService : IScopedService
    {
        Task<string> GetIdAsync();
    }
}
