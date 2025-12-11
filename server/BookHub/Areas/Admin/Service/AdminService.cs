namespace BookHub.Areas.Admin.Service;

using Features.Identity.Data.Models;
using Microsoft.AspNetCore.Identity;

using static Common.Constants.Names;

public class AdminService(UserManager<User> userManager) : IAdminService
{
    public async Task<string> GetId()
    {
        var admins = await userManager.GetUsersInRoleAsync(AdminRoleName);
        var admin = admins.SingleOrDefault()
            ?? throw new InvalidOperationException("Admin user not found!");

        return admin.Id;
    }
}
