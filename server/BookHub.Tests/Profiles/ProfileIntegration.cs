namespace BookHub.Tests.Profiles;

using System.Net;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Shared.Seed;
using Shared.Utils;

public sealed class ProfileIntegration : IAsyncLifetime
{
    private const string UserId = "test-user";
    private const string AdminId = "test-admin-id";

    private readonly BookHubWebApplicationFactory httpClientFactory = new();

    public async ValueTask InitializeAsync()
    {
        await this.httpClientFactory.ResetDatabase();
        await this.httpClientFactory.WithData(data => data.SeedUser(AdminId, "admin"));
    }

    public ValueTask DisposeAsync()
    {
        this.httpClientFactory.Dispose();
        return ValueTask.CompletedTask;
    }

    [Fact]
    public async Task AdminDelete_ShouldReturnNotFoundProblem_WhenTheProfileDoesNotExist()
    {
        var httpClient = this.httpClientFactory.CreateAdminClient(AdminId);

        var response = await httpClient.DeleteAsync("/Administrator/Profile/no-such-user/");

        var problem = await response.ShouldBeProblem(
            HttpStatusCode.NotFound,
            "The profile was not found.");

        // S-11: the requested ID isn't echoed back.
        problem.Detail.Should().NotContain("no-such-user");
    }

    [Fact]
    public async Task AdminDelete_ShouldReturnNoContent_AndAlso_ShouldSoftDeleteTheUser()
    {
        await this.httpClientFactory.WithData(data => data.SeedProfile(UserId));
        var httpClient = this.httpClientFactory.CreateAdminClient(AdminId);

        var response = await httpClient.DeleteAsync($"/Administrator/Profile/{UserId}/");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var isDeleted = await this.httpClientFactory.WithData(data => data
            .Users
            .IgnoreQueryFilters()
            .Where(u => u.Id == UserId)
            .Select(u => u.IsDeleted)
            .SingleAsync());

        isDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task Edit_ShouldReturnNotFoundProblem_WhenTheCallerHasNoProfile()
    {
        await this.httpClientFactory.WithData(data => data.SeedUser(UserId, "user"));
        var httpClient = this.httpClientFactory.CreateUserClient(UserId);

        var form = new MultipartFormDataContent
        {
            { new StringContent("First"), "FirstName" },
            { new StringContent("Last"), "LastName" },
            { new StringContent("1990-01-01"), "DateOfBirth" },
            { new StringContent("false"), "IsPrivate" },
        };

        var response = await httpClient.PutAsync("/Profile", form);

        await response.ShouldBeProblem(
            HttpStatusCode.NotFound,
            "The profile was not found.");
    }
}
