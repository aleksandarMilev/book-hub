namespace BookHub.Tests.Identity;

using System.Net;
using System.Net.Http.Headers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Shared.Identity;
using Shared.Utils;

// Runs the app's real JwtBearer pipeline (RealJwtFactory, not the test scheme): the tokens are
// signed by /Identity/register, so OnTokenValidated's deleted-user check is what's under test (S-06).
public sealed class DeletedUserTokenIntegration : IAsyncLifetime
{
    private const string Username = "jwtreader";

    // Must require a user: on a public endpoint a rejected token is served as anonymous (200).
    private const string ProtectedUrl = "/Profile/mine/";

    private readonly RealJwtFactory httpClientFactory = new();

    public async ValueTask InitializeAsync()
        => await this.httpClientFactory.ResetDatabase();

    public ValueTask DisposeAsync()
    {
        this.httpClientFactory.Dispose();
        return ValueTask.CompletedTask;
    }

    [Fact]
    public async Task Token_ShouldBeRejectedWith401_AfterTheUserDeletesTheirAccount()
    {
        var httpClient = await this.RegisterAndAuthenticate();

        var before = await httpClient.GetAsync(ProtectedUrl, TestContext.Current.CancellationToken);
        before.StatusCode.Should().Be(HttpStatusCode.OK);

        var delete = await httpClient.DeleteAsync("/Profile", TestContext.Current.CancellationToken);
        delete.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // The same signed, unexpired token.
        var after = await httpClient.GetAsync(ProtectedUrl, TestContext.Current.CancellationToken);

        await after.ShouldBeProblem(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Token_ShouldBeRejectedWith401_WhenTheUserIsSoftDeletedElsewhere()
    {
        var httpClient = await this.RegisterAndAuthenticate();

        // As an admin's DELETE /Administrator/Profile/{id} would do: a soft delete.
        await this.httpClientFactory.WithData(async data =>
        {
            var user = await data.Users.SingleAsync(u => u.UserName == Username);

            data.Users.Remove(user);
            await data.SaveChangesAsync();
        });

        var response = await httpClient.GetAsync("/Profile/mine/", TestContext.Current.CancellationToken);

        await response.ShouldBeProblem(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Token_ShouldBeAccepted_WhileTheUserExists()
    {
        var httpClient = await this.RegisterAndAuthenticate();

        var response = await httpClient.GetAsync("/Profile/mine/", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Request_ShouldBeRejectedWith401_WhenTheTokenIsNotValid()
    {
        var httpClient = this.httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            "not-a-jwt");

        var response = await httpClient.GetAsync(ProtectedUrl, TestContext.Current.CancellationToken);

        await response.ShouldBeProblem(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task PublicEndpoint_ShouldServeTheRequestAnonymously_WhenTheTokenIsNotValid()
    {
        // [AllowAnonymous] skips the challenge, so the catalog still loads with a stale token
        // (the client's 401 interceptor fires on the next protected call instead).
        var httpClient = this.httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            "not-a-jwt");

        var response = await httpClient.GetAsync("/Genres/", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task QueryFilters_ShouldSeeTheCaller_AfterTheDeletedUserCheckUsedTheDbContext()
    {
        // OnTokenValidated resolves the request's DbContext before HttpContext.User is set, so the
        // context must read the current user per query: a non-admin's create reloads the new,
        // unapproved book through the approval filter, which only lets its creator see it.
        var httpClient = await this.RegisterAndAuthenticate();

        var form = new MultipartFormDataContent
        {
            { new StringContent("A book awaiting approval"), "Title" },
            { new StringContent("A valid short description"), "ShortDescription" },
            { new StringContent(new string('l', 250)), "LongDescription" },
        };

        var created = await httpClient.PostAsync("/Books", form, TestContext.Current.CancellationToken);
        created.StatusCode.Should().Be(HttpStatusCode.Created);

        var bookUrl = created.Headers.Location!.PathAndQuery;
        var asCreator = await httpClient.GetAsync(bookUrl, TestContext.Current.CancellationToken);
        var asAnonymous = await this.httpClientFactory.CreateClient().GetAsync(bookUrl, TestContext.Current.CancellationToken);

        asCreator.StatusCode.Should().Be(HttpStatusCode.OK);
        await asAnonymous.ShouldBeProblem(HttpStatusCode.NotFound);
    }

    private Task<HttpClient> RegisterAndAuthenticate()
        => this.httpClientFactory.RegisterAndAuthenticate(Username);
}
