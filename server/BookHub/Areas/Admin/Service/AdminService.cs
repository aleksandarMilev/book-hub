namespace BookHub.Areas.Admin.Service
{
    using Features.Identity.Data.Models;
    using Microsoft.AspNetCore.Identity;

    using static Common.Constants;

    public class AdminService(UserManager<User> userManager) : IAdminService
    {
        private readonly UserManager<User> userManager = userManager;

        public async Task<string> GetId()
        {
            var admin = await this.userManager.FindByEmailAsync(AdminEmail);

            return admin is null 
                ? throw new InvalidOperationException("Admin not found!") 
                : admin.Id;
        }
    }
}
