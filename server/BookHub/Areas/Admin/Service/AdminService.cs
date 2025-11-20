namespace BookHub.Areas.Admin.Service
{
    using Features.Identity.Data.Models;
    using Microsoft.AspNetCore.Identity;

    using static Common.Constants;

    public class AdminService(UserManager<User> userManager) : IAdminService
    {
        public async Task<string> GetId()
        {
            //TODO: change the approach with the admin role feature
            var admin = await userManager.FindByEmailAsync("foo");

            return admin is null 
                ? throw new InvalidOperationException("Admin not found!") 
                : admin.Id;
        }
    }
}
