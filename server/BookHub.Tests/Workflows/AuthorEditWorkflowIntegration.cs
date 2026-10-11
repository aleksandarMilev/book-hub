namespace BookHub.Tests.Workflows;

using System.Globalization;
using System.Net;
using Common;
using Features.Authors.Service.Models;
using Features.Authors.Shared;
using Features.Notifications.Service.Models;
using Features.UserProfile.Service.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Shared.Seed;
using Shared.Utils;

using static Features.Authors.Shared.Constants.Paths;

// The book edit workflow, for authors: a non-admin's edit waits in a pending row that only the
// creator and the admins see, an admin approves or rejects it, and an admin's own edit applies
// directly and supersedes it. Photos are real files, checked on disk at every step.
public sealed class AuthorEditWorkflowIntegration : IAsyncLifetime
{
    private const string CreatorId = "creator-id";
    private const string CreatorName = "creator";
    private const string OriginalName = "Original Author";

    private static readonly string[] AdminIds = ["admin-1", "admin-2"];

    private readonly BookHubWebApplicationFactory factory = new();

    private Guid authorId;
    private string photo = null!;

    public async ValueTask InitializeAsync()
    {
        await this.factory.ResetDatabase();

        this.photo = this.factory.SeedImageFile(ImagePathPrefix, "photo.jpg");

        await this.factory.WithData(async data =>
        {
            foreach (var adminId in AdminIds)
            {
                await data.SeedAdmin(adminId, adminId);
            }

            await data.SeedUser(CreatorId, CreatorName);
            await data.SeedProfile(CreatorId);

            this.authorId = (await data.SeedAuthor(
                OriginalName,
                creatorId: CreatorId,
                imagePath: this.photo)).Id;
        });
    }

    public ValueTask DisposeAsync()
    {
        this.factory.Dispose();
        return ValueTask.CompletedTask;
    }

    [Fact]
    public async Task PendingEdit_IsShownToTheCreatorAndAdminsOnly_ThenReplaced_ThenApproved()
    {
        var creator = await this.factory.CreateUserClient(CreatorId, CreatorName);
        var admin = await this.factory.CreateAdminClient(AdminIds[0], AdminIds[0]);
        var anonymous = this.factory.CreateAnonymousClient();
        var otherUser = await this.factory.CreateUserClient("other-id");

        // 1. The creator proposes a new name, nationality and photo.
        await this.Edit(creator, "First Proposal", Nationality.France);

        var firstPendingPhoto = await this.PendingPhoto();

        this.factory.ImageFiles(PendingImagePathPrefix).Should().Equal(firstPendingPhoto);

        // The creator and the admins see the proposal; everyone else sees the approved author.
        (await this.Details(creator)).Name.Should().Be("First Proposal");
        (await this.AdminDetails(admin)).Name.Should().Be("First Proposal");
        (await this.Details(anonymous)).Name.Should().Be(OriginalName);
        (await this.Details(otherUser)).ImagePath.Should().Be(this.photo);

        foreach (var adminId in AdminIds)
        {
            (await this.NotificationsOf(await this.factory.CreateAdminClient(adminId, adminId)))
                .Should()
                .ContainSingle(n => n.ResourceId == this.authorId && n.Message == $"{CreatorName} has edited '{OriginalName}'");
        }

        // 2. A second proposal replaces the first one: still one pending row, and its photo
        //    replaced the first pending photo on disk.
        await this.Edit(creator, "Second Proposal", Nationality.Germany);

        var secondPendingPhoto = await this.PendingPhoto();

        (await this.PendingEdits()).Should().Be(1);
        this.factory.ImageFiles(PendingImagePathPrefix).Should().Equal(secondPendingPhoto);
        (await this.Details(creator)).Name.Should().Be("Second Proposal");

        // 3. Approved: the second proposal is the author now, for everyone.
        await (await admin.PatchAsync($"/Administrator/Authors/{this.authorId}/approve/", null, TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        var approved = await this.Details(anonymous);

        approved.Name.Should().Be("Second Proposal");
        approved.Nationality.Should().Be(Nationality.Germany);
        approved.Biography.Should().Be(new string('x', 200));
        approved.ImagePath.Should().Be(secondPendingPhoto);

        (await this.PendingEdits()).Should().Be(0);

        // The old photo's file is deleted, and only the approved one is left.
        this.factory.ImageExists(this.photo).Should().BeFalse();
        this.factory.ImageFiles(ImagePathPrefix).Should().BeEmpty();
        this.factory.ImageFiles(PendingImagePathPrefix).Should().Equal(secondPendingPhoto);

        (await this.NotificationsOf(creator))
            .Should()
            .ContainSingle(n => n.ResourceId == this.authorId && n.Message == "'Second Proposal' has been approved");

        // An edit of an already approved author doesn't count as a new author.
        var profile = await (await creator.GetAsync("/Profile/mine/", TestContext.Current.CancellationToken))
            .ShouldHaveBody<ProfileServiceModel>();

        profile.CreatedAuthorsCount.Should().Be(0);
    }

    [Fact]
    public async Task PendingEdit_IsRejected_ThenANewOne_IsSupersededByAnAdminsDirectEdit()
    {
        var creator = await this.factory.CreateUserClient(CreatorId, CreatorName);
        var admin = await this.factory.CreateAdminClient(AdminIds[1], AdminIds[1]);
        var anonymous = this.factory.CreateAnonymousClient();

        // 1. Rejected: the author is unchanged, the proposal and its photo are gone.
        await this.Edit(creator, "Rejected Proposal", Nationality.Italy);

        var rejectedPhoto = await this.PendingPhoto();

        await (await admin.PatchAsync($"/Administrator/Authors/{this.authorId}/reject/", null, TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        (await this.Details(creator)).Name.Should().Be(OriginalName);
        (await this.PendingEdits()).Should().Be(0);

        this.factory.ImageExists(rejectedPhoto).Should().BeFalse();
        this.factory.ImageFiles(ImagePathPrefix).Should().Equal(this.photo);

        (await this.NotificationsOf(creator))
            .Should()
            .ContainSingle(n => n.ResourceId == this.authorId && n.Message == $"'{OriginalName}' has been rejected");

        // 2. A new proposal (a rejected one doesn't block it), then an admin edits directly.
        await this.Edit(creator, "Superseded Proposal", Nationality.Spain);

        var supersededPhoto = await this.PendingPhoto();

        await (await admin.PutAsync(
                $"/Authors/{this.authorId}/",
                Forms.Author("The Admin's Version").WithImage(),
                TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        // 3. The admin's version is live for everyone, including the creator, and the superseded
        //    proposal, its photo and the old photo are all gone.
        var live = await this.Details(anonymous);

        live.Name.Should().Be("The Admin's Version");
        (await this.Details(creator)).Name.Should().Be("The Admin's Version");
        (await this.PendingEdits()).Should().Be(0);

        this.factory.ImageExists(supersededPhoto).Should().BeFalse();
        this.factory.ImageFiles(PendingImagePathPrefix).Should().BeEmpty();
        this.factory.ImageFiles(ImagePathPrefix).Should().Equal(live.ImagePath);

        // Approving now has nothing to apply, so it can't bring the superseded proposal back.
        await (await admin.PatchAsync($"/Administrator/Authors/{this.authorId}/approve/", null, TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        (await this.Details(anonymous)).Name.Should().Be("The Admin's Version");
    }

    [Fact]
    public async Task Approve_ShouldApplyTheEdit_AndCountTheAuthor_WhenTheCreatorEditedItWhilePending()
    {
        var creator = await this.factory.CreateUserClient(CreatorId, CreatorName);

        var created = await (await creator.PostAsync("/Authors", Forms.Author("Written In Haste"), TestContext.Current.CancellationToken))
            .ShouldHaveBody<AuthorDetailsServiceModel>(HttpStatusCode.Created);

        await (await creator.PutAsync(
                $"/Authors/{created.Id}/",
                Forms.Author("Edited Before Approval"),
                TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        await (await (await this.factory.CreateAdminClient(AdminIds[0], AdminIds[0])).PatchAsync(
                $"/Administrator/Authors/{created.Id}/approve/",
                null,
                TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        var publicView = await (await this.factory.CreateAnonymousClient().GetAsync($"/Authors/{created.Id}/", TestContext.Current.CancellationToken))
            .ShouldHaveBody<AuthorDetailsServiceModel>();

        publicView.Name.Should().Be("Edited Before Approval");
        publicView.IsApproved.Should().BeTrue();

        // B-24, for authors: approving through the pending-edit branch still counts the new author.
        var profile = await (await creator.GetAsync("/Profile/mine/", TestContext.Current.CancellationToken))
            .ShouldHaveBody<ProfileServiceModel>();

        profile.CreatedAuthorsCount.Should().Be(1);
    }

    [Fact]
    public async Task AdminDetails_ShouldReturnNotFoundProblem_WhenTheAuthorDoesNotExist()
    {
        var admin = await this.factory.CreateAdminClient(AdminIds[0], AdminIds[0]);

        await (await admin.GetAsync($"/Administrator/Authors/{Guid.NewGuid()}/", TestContext.Current.CancellationToken))
            .ShouldBeProblem(HttpStatusCode.NotFound);
    }

    private async Task Edit(
        HttpClient client,
        string name,
        Nationality nationality)
    {
        var form = new MultipartFormDataContent
        {
            { new StringContent(name), "Name" },
            { new StringContent(new string('x', 200)), "Biography" },
            { new StringContent(((int)nationality).ToString(CultureInfo.InvariantCulture)), "Nationality" },
            { new StringContent(((int)Gender.Female).ToString(CultureInfo.InvariantCulture)), "Gender" },
        };

        await (await client.PutAsync(
                $"/Authors/{this.authorId}/",
                form.WithImage(),
                TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);
    }

    private async Task<AuthorDetailsServiceModel> Details(HttpClient client)
        => await (await client.GetAsync($"/Authors/{this.authorId}/", TestContext.Current.CancellationToken))
            .ShouldHaveBody<AuthorDetailsServiceModel>();

    private async Task<AuthorDetailsServiceModel> AdminDetails(HttpClient admin)
        => await (await admin.GetAsync($"/Administrator/Authors/{this.authorId}/", TestContext.Current.CancellationToken))
            .ShouldHaveBody<AuthorDetailsServiceModel>();

    private async Task<List<NotificationServiceModel>> NotificationsOf(HttpClient client)
    {
        var page = await (await client.GetAsync("/Notifications/", TestContext.Current.CancellationToken))
            .ShouldHaveBody<PaginatedModel<NotificationServiceModel>>();

        return page.Items.ToList();
    }

    private Task<string> PendingPhoto()
        => this.factory.WithData(data => data
            .AuthorEdits
            .Where(e => e.AuthorId == this.authorId)
            .Select(e => e.ImagePath)
            .SingleAsync(TestContext.Current.CancellationToken));

    private Task<int> PendingEdits()
        => this.factory.WithData(data => data
            .AuthorEdits
            .CountAsync(e => e.AuthorId == this.authorId, TestContext.Current.CancellationToken));
}
