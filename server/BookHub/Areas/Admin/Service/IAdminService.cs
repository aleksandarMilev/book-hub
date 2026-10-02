namespace BookHub.Areas.Admin.Service;

using Infrastructure.Services.ServiceLifetimes;

public interface IAdminService : IScopedService
{
    /// <summary>
    /// Returns the IDs of every user in the admin role. Empty when there are none.
    /// </summary>
    Task<IReadOnlyCollection<string>> GetIds(
        CancellationToken cancellationToken = default);
}
