namespace BookHub.Tests.Profiles;

using System.Net;
using Features.Books.Service.Models;
using Features.UserProfile.Service.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Shared.Seed;
using Shared.Utils;

using static Features.UserProfile.Shared.Constants.Paths;

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
        var httpClient = await this.httpClientFactory.CreateAdminClient(AdminId);

        var response = await httpClient.DeleteAsync("/Administrator/Profile/no-such-user/", TestContext.Current.CancellationToken);

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
        var httpClient = await this.httpClientFactory.CreateAdminClient(AdminId);

        var response = await httpClient.DeleteAsync($"/Administrator/Profile/{UserId}/", TestContext.Current.CancellationToken);

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
        var httpClient = await this.httpClientFactory.CreateUserClient(UserId);

        var form = new MultipartFormDataContent
        {
            { new StringContent("First"), "FirstName" },
            { new StringContent("Last"), "LastName" },
            { new StringContent("1990-01-01"), "DateOfBirth" },
            { new StringContent("false"), "IsPrivate" },
        };

        var response = await httpClient.PutAsync("/Profile", form, TestContext.Current.CancellationToken);

        await response.ShouldBeProblem(
            HttpStatusCode.NotFound,
            "The profile was not found.");
    }

    [Fact]
    public async Task Avatar_IsWrittenOnRegistration_ReplacedOnEdit_AndDeletedWhenRemoved()
    {
        // 1. Registered with an avatar: the file is on disk.
        var registered = await (await this.httpClientFactory.CreateClient().PostAsync(
                "/Identity/register/",
                BookHubWebApplicationFactory.RegisterForm("pictured").WithImage("avatar.jpg"),
                TestContext.Current.CancellationToken))
            .ShouldHaveBody<BookHubWebApplicationFactory.TokenResponse>();

        var httpClient = this.httpClientFactory.CreateBearerClient(registered.Token);
        var first = (await this.Mine(httpClient)).ImagePath;

        this.httpClientFactory.ImageFiles(ProfilesImagePathPrefix).Should().Equal(first);

        // 2. A new avatar replaces the file.
        await (await httpClient.PutAsync("/Profile", Forms.Profile(isPrivate: false).WithImage("new.jpg"), TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        var second = (await this.Mine(httpClient)).ImagePath;

        second.Should().NotBe(first);
        this.httpClientFactory.ImageFiles(ProfilesImagePathPrefix).Should().Equal(second);

        // 3. An edit without an image keeps it.
        await (await httpClient.PutAsync("/Profile", Forms.Profile(isPrivate: true), TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        (await this.Mine(httpClient)).ImagePath.Should().Be(second);
        this.httpClientFactory.ImageExists(second).Should().BeTrue();

        // 4. Removing it goes back to the default avatar and deletes the file.
        var remove = Forms.Profile(isPrivate: true);
        remove.Add(new StringContent("true"), "RemoveImage");

        await (await httpClient.PutAsync("/Profile", remove, TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        (await this.Mine(httpClient)).ImagePath.Should().Be(DefaultImagePath);
        this.httpClientFactory.ImageFiles(ProfilesImagePathPrefix).Should().BeEmpty();
    }

    [Theory]
    [MemberData(nameof(Images.Invalid), MemberType = typeof(Images))]
    public async Task Edit_ShouldReturnValidationProblem_AndKeepTheAvatar_WhenTheImageIsInvalid(
        string kind,
        string fileName,
        string contentType,
        string expectedMessage)
    {
        var avatar = this.httpClientFactory.SeedImageFile(ProfilesImagePathPrefix, "avatar.jpg");
        await this.httpClientFactory.WithData(async data =>
        {
            var profile = await data.SeedProfile(UserId);
            profile.ImagePath = avatar;
            await data.SaveChangesAsync();
        });

        var httpClient = await this.httpClientFactory.CreateUserClient(UserId);

        var form = Forms.Profile(isPrivate: false).WithFile(Images.Bytes(kind), fileName, contentType);
        var response = await httpClient.PutAsync("/Profile", form, TestContext.Current.CancellationToken);

        await response.ShouldBeProblem(HttpStatusCode.BadRequest);

        var body = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        body.Should().Contain(expectedMessage);

        // Nothing was written, and the current avatar is untouched.
        this.httpClientFactory.ImageFiles(ProfilesImagePathPrefix).Should().Equal(avatar);
        (await this.Mine(httpClient)).ImagePath.Should().Be(avatar);
    }

    [Fact]
    public async Task BookCreate_ShouldReturnValidationProblem_AndWriteNothing_WhenTheCoverIsNotAnImage()
    {
        var httpClient = await this.httpClientFactory.CreateUserClient(UserId);

        // A text file disguised as a PNG: the extension and content type pass, the bytes don't.
        var form = Forms.Book("A book with a fake cover").WithFile(Images.Bytes("text"), "cover.png", "image/png");
        var response = await httpClient.PostAsync("/Books", form, TestContext.Current.CancellationToken);

        await response.ShouldBeProblem(HttpStatusCode.BadRequest);

        this.httpClientFactory.ImageFiles("books").Should().BeEmpty();

        var books = await this.httpClientFactory.WithData(data => data.Books.IgnoreQueryFilters().CountAsync());
        books.Should().Be(0);
    }

    private async Task<ProfileServiceModel> Mine(HttpClient httpClient)
        => await (await httpClient.GetAsync("/Profile/mine/", TestContext.Current.CancellationToken))
            .ShouldHaveBody<ProfileServiceModel>();
}
