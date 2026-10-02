namespace BookHub.Tests.Admin;

using Areas.Admin.Service;
using Features.Identity.Data.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;

using static Common.Constants.Names;

// The factory replaces IAdminService with a mock, so these tests build the real AdminService
// on top of the factory's Identity stack (SQLite).
public sealed class AdminServiceIntegration : IAsyncLifetime
{
    private readonly BookHubWebApplicationFactory httpClientFactory = new();

    public async ValueTask InitializeAsync()
        => await this.httpClientFactory.ResetDatabase();

    public ValueTask DisposeAsync()
    {
        this.httpClientFactory.Dispose();
        return ValueTask.CompletedTask;
    }

    [Fact]
    public async Task GetIds_ShouldReturnEmpty_AndAlso_ShouldNotThrow_WhenThereAreNoAdmins()
    {
        using var scope = this.httpClientFactory.Services.CreateScope();
        var services = scope.ServiceProvider;

        await CreateUser(services, "regular-user", isAdmin: false);

        var adminService = NewAdminService(services);

        var adminIds = await adminService.GetIds();

        adminIds.Should().BeEmpty();
    }

    [Fact]
    public async Task GetIds_ShouldReturnEveryAdminId_WhenThereAreSeveralAdmins()
    {
        using var scope = this.httpClientFactory.Services.CreateScope();
        var services = scope.ServiceProvider;

        var firstAdminId = await CreateUser(services, "first-admin", isAdmin: true);
        var secondAdminId = await CreateUser(services, "second-admin", isAdmin: true);
        await CreateUser(services, "regular-user", isAdmin: false);

        var adminService = NewAdminService(services);

        var adminIds = await adminService.GetIds();

        adminIds.Should().BeEquivalentTo([firstAdminId, secondAdminId]);
    }

    private static AdminService NewAdminService(IServiceProvider services)
        => new(
            services.GetRequiredService<UserManager<UserDbModel>>(),
            NullLogger<AdminService>.Instance);

    private static async Task<string> CreateUser(
        IServiceProvider services,
        string username,
        bool isAdmin)
    {
        var userManager = services.GetRequiredService<UserManager<UserDbModel>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        if (!await roleManager.RoleExistsAsync(AdminRoleName))
        {
            await roleManager.CreateAsync(new IdentityRole(AdminRoleName));
        }

        var user = new UserDbModel
        {
            UserName = username,
            Email = $"{username}@test.local"
        };

        var createResult = await userManager.CreateAsync(user);
        createResult.Succeeded.Should().BeTrue();

        if (isAdmin)
        {
            var roleResult = await userManager.AddToRoleAsync(user, AdminRoleName);
            roleResult.Succeeded.Should().BeTrue();
        }

        return user.Id;
    }
}
