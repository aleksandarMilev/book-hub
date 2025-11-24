namespace BookHub.Areas.Admin.Service;

using Features.Identity.Data.Models;
using Microsoft.AspNetCore.Identity;

using static Common.Constants.Names;

public class AdminService : IAdminService
{
    public async Task<string> GetId()
    {
        //var admin = await roleManager.FindByNameAsync(AdminRoleName);

        //return admin is null 
        //    ? throw new InvalidOperationException("Admin not found!") 
        //    : admin.Id;

        await Task.Delay(10);
        return "foo";
    }
}
