namespace BookHub.Areas.Admin.Service;

using Features.Identity.Data.Models;
using Microsoft.AspNetCore.Identity;

using static Common.Constants.Names;

public class AdminService(
    UserManager<UserDbModel> userManager,
    ILogger<AdminService> logger) : IAdminService
{
    public async Task<IReadOnlyCollection<string>> GetIds(
        CancellationToken cancellationToken = default)
    {
        var admins = await userManager.GetUsersInRoleAsync(AdminRoleName);
        if (admins.Count == 0)
        {
            logger.LogWarning(
                "No users in role {Role}; admin notifications will be skipped.",
                AdminRoleName);

            return [];
        }

        return admins
            .Select(a => a.Id)
            .ToList();
    }
}
