namespace BookHub.Tests.Admin;

using Areas.Admin.Service;
using Features.Identity.Data.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using static Common.Constants.Names;

// IAdminService as the app resolves it, over the real Identity stack (PostgreSQL). The workflows
// cover the same rule end to end (every admin notified, none when there are no admins).
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

        var adminIds = await services
            .GetRequiredService<IAdminService>()
            .GetIds(TestContext.Current.CancellationToken);

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

        var adminIds = await services
            .GetRequiredService<IAdminService>()
            .GetIds(TestContext.Current.CancellationToken);

        adminIds.Should().BeEquivalentTo([firstAdminId, secondAdminId]);
    }

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
