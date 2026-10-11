namespace BookHub.Tests.Workflows;

using System.Net;
using Common;
using Features.Authors.Service.Models;
using Features.Notifications.Service.Models;
using Features.Notifications.Shared;
using Features.Search.Service.Models;
using Features.UserProfile.Service.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Shared.Seed;
using Shared.Utils;

// The book approval workflow, condensed, for authors: the same approval rules apply.
public sealed class AuthorApprovalWorkflowIntegration : IAsyncLifetime
{
    private const string CreatorId = "creator-id";
    private const string CreatorName = "creator";

    private static readonly string[] AdminIds = ["admin-1", "admin-2"];

    private readonly WorkflowFactory factory = new();

    public async ValueTask InitializeAsync()
    {
        await this.factory.ResetDatabase();

        await this.factory.WithData(async data =>
        {
            foreach (var adminId in AdminIds)
            {
                await data.SeedAdmin(adminId, adminId);
            }

            await data.SeedUser(CreatorId, CreatorName);
            await data.SeedProfile(CreatorId);
        });
    }

    public ValueTask DisposeAsync()
    {
        this.factory.Dispose();
        return ValueTask.CompletedTask;
    }

    [Fact]
    public async Task Lifecycle_NonAdminCreates_AdminsAreNotified_AdminApproves_CreatorIsNotifiedAndCounted_AuthorBecomesPublic()
    {
        const string Name = "Authorworkflow Writer";

        var creator = this.factory.CreateUserClient(CreatorId, CreatorName);
        var anonymous = this.factory.CreateAnonymousClient();

        var created = await (await creator.PostAsync("/Authors", Forms.Author(Name), TestContext.Current.CancellationToken))
            .ShouldHaveBody<AuthorDetailsServiceModel>(HttpStatusCode.Created);

        created.IsApproved.Should().BeFalse();
        var authorId = created.Id;

        foreach (var adminId in AdminIds)
        {
            (await this.NotificationsOf(this.factory.CreateAdminClient(adminId, adminId)))
                .Should()
                .ContainSingle(n =>
                    n.ResourceId == authorId &&
                    n.ResourceType == ResourceType.Author &&
                    n.Message == $"{CreatorName} has created '{Name}'");
        }

        await (await anonymous.GetAsync($"/Authors/{authorId}/", TestContext.Current.CancellationToken))
            .ShouldBeProblem(HttpStatusCode.NotFound);

        (await this.SearchIds(anonymous, "Authorworkflow")).Should().NotContain(authorId);

        await (await this.factory.CreateAdminClient(AdminIds[0], AdminIds[0]).PatchAsync(
                $"/Administrator/Authors/{authorId}/approve/",
                null,
                TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        (await this.NotificationsOf(creator))
            .Should()
            .ContainSingle(n => n.ResourceId == authorId && n.Message == $"'{Name}' has been approved");

        var profile = await (await creator.GetAsync("/Profile/mine/", TestContext.Current.CancellationToken))
            .ShouldHaveBody<ProfileServiceModel>();

        profile.CreatedAuthorsCount.Should().Be(1);

        var publicView = await (await anonymous.GetAsync($"/Authors/{authorId}/", TestContext.Current.CancellationToken))
            .ShouldHaveBody<AuthorDetailsServiceModel>();

        publicView.Name.Should().Be(Name);
        publicView.IsApproved.Should().BeTrue();

        (await this.SearchIds(anonymous, "Authorworkflow")).Should().Equal(authorId);
    }

    [Fact]
    public async Task Rejection_AdminRejects_CreatorIsNotified_AuthorIsGoneForEveryone()
    {
        const string Name = "Rejected Authorworkflow";

        var creator = this.factory.CreateUserClient(CreatorId, CreatorName);

        var created = await (await creator.PostAsync("/Authors", Forms.Author(Name), TestContext.Current.CancellationToken))
            .ShouldHaveBody<AuthorDetailsServiceModel>(HttpStatusCode.Created);

        await (await this.factory.CreateAdminClient(AdminIds[1], AdminIds[1]).PatchAsync(
                $"/Administrator/Authors/{created.Id}/reject/",
                null,
                TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        (await this.NotificationsOf(creator))
            .Should()
            .ContainSingle(n => n.ResourceId == created.Id && n.Message == $"'{Name}' has been rejected");

        HttpClient[] everyone =
        [
            creator,
            this.factory.CreateAnonymousClient(),
            this.factory.CreateAdminClient(AdminIds[0], AdminIds[0]),
        ];

        foreach (var client in everyone)
        {
            await (await client.GetAsync($"/Authors/{created.Id}/", TestContext.Current.CancellationToken))
                .ShouldBeProblem(HttpStatusCode.NotFound);
        }

        var isDeleted = await this.factory.WithData(data => data
            .Authors
            .IgnoreQueryFilters()
            .Where(a => a.Id == created.Id)
            .Select(a => a.IsDeleted)
            .SingleAsync(TestContext.Current.CancellationToken));

        isDeleted.Should().BeTrue();

        var profile = await (await creator.GetAsync("/Profile/mine/", TestContext.Current.CancellationToken))
            .ShouldHaveBody<ProfileServiceModel>();

        profile.CreatedAuthorsCount.Should().Be(0);
    }

    private async Task<List<NotificationServiceModel>> NotificationsOf(HttpClient client)
    {
        var page = await (await client.GetAsync("/Notifications/", TestContext.Current.CancellationToken))
            .ShouldHaveBody<PaginatedModel<NotificationServiceModel>>();

        return page.Items.ToList();
    }

    private async Task<List<Guid>> SearchIds(
        HttpClient client,
        string searchTerm)
    {
        var page = await (await client.GetAsync($"/Search/authors/?searchTerm={searchTerm}", TestContext.Current.CancellationToken))
            .ShouldHaveBody<PaginatedModel<SearchAuthorServiceModel>>();

        return page.Items.Select(a => a.Id).ToList();
    }
}
