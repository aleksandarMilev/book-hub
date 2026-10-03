namespace BookHub.Tests.Authorization;

using System.Net;
using Features.ReadingLists.Data.Models;
using Features.ReadingLists.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Shared.Seed;
using Shared.Utils;

using static AuthorizationMatrix;

// Table-driven authorization tests over AuthorizationMatrix. One app host is shared by the
// class (AuthorizationMatrixFixture), and every test still gets a fresh, freshly seeded database.
// The access checks assert "not 401/403" rather than full success, so the matrix doesn't depend
// on request bodies.
public sealed class AuthorizationMatrixIntegration(AuthorizationMatrixFixture fixture)
    : IClassFixture<AuthorizationMatrixFixture>, IAsyncLifetime
{
    private readonly BookHubWebApplicationFactory httpClientFactory = fixture.Factory;

    private MatrixSeed seed = null!;

    public static TheoryData<string> ProtectedEndpoints
        => new(Protected.Select(e => e.Key));

    public static TheoryData<string> AdminEndpoints
        => new(Protected.Where(e => e.Access == Access.Admin).Select(e => e.Key));

    public static TheoryData<string> OwnerEndpoints
        => new(Protected.Where(e => e.Access == Access.Owner).Select(e => e.Key));

    public static TheoryData<string> PublicEndpoints
        => new(Public.Select(e => e.Key));

    public async ValueTask InitializeAsync()
    {
        await this.httpClientFactory.ResetDatabase();
        this.seed = await this.Seed();
    }

    public ValueTask DisposeAsync()
        => ValueTask.CompletedTask;

    [Theory]
    [MemberData(nameof(ProtectedEndpoints))]
    public async Task Anonymous_ShouldGetUnauthorizedProblem(string endpoint)
    {
        var httpClient = this.httpClientFactory.CreateAnonymousClient();

        using var request = ProtectedByKey(endpoint).ToRequest(this.seed);
        var response = await httpClient.SendAsync(request);

        await response.ShouldBeProblem(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [MemberData(nameof(AdminEndpoints))]
    public async Task NonAdmin_ShouldGetForbiddenProblem(string endpoint)
    {
        var httpClient = this.httpClientFactory.CreateUserClient(OwnerId);

        using var request = ProtectedByKey(endpoint).ToRequest(this.seed);
        var response = await httpClient.SendAsync(request);

        await response.ShouldBeProblem(HttpStatusCode.Forbidden);
    }

    [Theory]
    [MemberData(nameof(OwnerEndpoints))]
    public async Task WrongUser_ShouldGetForbiddenOrNotFoundProblem(string endpoint)
    {
        var protectedEndpoint = ProtectedByKey(endpoint);
        var httpClient = this.httpClientFactory.CreateUserClient(OtherUserId);

        using var request = protectedEndpoint.ToRequest(this.seed);
        var response = await httpClient.SendAsync(request);

        await response.ShouldBeProblem(protectedEndpoint.WrongUserStatus!.Value);
    }

    [Theory]
    [MemberData(nameof(ProtectedEndpoints))]
    public async Task AllowedCaller_ShouldNotBeRejected(string endpoint)
    {
        var protectedEndpoint = ProtectedByKey(endpoint);

        var httpClient = protectedEndpoint.Access switch
        {
            Access.Admin => this.httpClientFactory.CreateAdminClient(AdminId),
            Access.Owner => this.httpClientFactory.CreateUserClient(OwnerId),
            _ => this.httpClientFactory.CreateUserClient(OtherUserId),
        };

        using var request = protectedEndpoint.ToRequest(this.seed);
        var response = await httpClient.SendAsync(request);

        response.StatusCode.Should().NotBe(HttpStatusCode.Unauthorized);
        response.StatusCode.Should().NotBe(HttpStatusCode.Forbidden);

        // Where the wrong user gets a 404, the owner must get past it.
        if (protectedEndpoint.WrongUserStatus == HttpStatusCode.NotFound)
        {
            response.StatusCode.Should().NotBe(HttpStatusCode.NotFound);
        }
    }

    [Theory]
    [MemberData(nameof(PublicEndpoints))]
    public async Task PublicEndpoint_ShouldWorkAnonymously(string endpoint)
    {
        var publicEndpoint = PublicByKey(endpoint);
        var httpClient = this.httpClientFactory.CreateAnonymousClient();

        var method = publicEndpoint.Method == "*" ? HttpMethod.Get : new HttpMethod(publicEndpoint.Method);
        using var request = new HttpRequestMessage(method, publicEndpoint.Url(this.seed));
        var response = await httpClient.SendAsync(request);

        if (publicEndpoint.RequiresBody)
        {
            // Identity endpoints need a real body; their full flows are in IdentityIntegration
            // and DeletedUserTokenIntegration. Here: authorization lets an anonymous caller through.
            response.StatusCode.Should().NotBe(HttpStatusCode.Unauthorized);
            response.StatusCode.Should().NotBe(HttpStatusCode.Forbidden);
            return;
        }

        response.IsSuccessStatusCode.Should().BeTrue(
            $"{endpoint} must stay public, but returned {(int)response.StatusCode}");
    }

    [Fact]
    public async Task UploadedImages_ShouldBePubliclyReadable()
    {
        var httpClient = this.httpClientFactory.CreateAnonymousClient();

        // A tracked seed image under wwwroot: static files are served before authorization.
        var response = await httpClient.GetAsync("/images/books/1984.jpg");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be("image/jpeg");
    }

    [Fact]
    public async Task UnknownRoute_ShouldGetUnauthorizedProblem_WhenAnonymous()
    {
        // With the fallback policy, a request that matches no endpoint still needs a user.
        var httpClient = this.httpClientFactory.CreateAnonymousClient();

        var response = await httpClient.GetAsync("/no-such-route/");

        await response.ShouldBeProblem(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public void EveryEndpoint_ShouldBeInTheMatrixOrThePublicList()
    {
        this.httpClientFactory.CreateClient();

        var routed = this.httpClientFactory
            .Services
            .GetRequiredService<EndpointDataSource>()
            .Endpoints
            .OfType<RouteEndpoint>()
            .Select(e => new
            {
                Key = $"{string.Join(",", e.Metadata.GetMetadata<HttpMethodMetadata>()?.HttpMethods ?? ["*"])} {e.RoutePattern.RawText}",
                IsAnonymous = e.Metadata.GetMetadata<IAllowAnonymous>() is not null,
            })
            .ToList();

        var protectedKeys = Protected.Select(e => e.Key).ToList();
        var publicKeys = Public.Select(e => e.Key).ToList();

        protectedKeys.Should().OnlyHaveUniqueItems();
        publicKeys.Should().OnlyHaveUniqueItems();
        protectedKeys.Intersect(publicKeys).Should().BeEmpty();

        routed
            .Select(e => e.Key)
            .Except(protectedKeys.Concat(publicKeys))
            .Should()
            .BeEmpty("every endpoint must be added to AuthorizationMatrix.Protected or .Public");

        protectedKeys
            .Concat(publicKeys)
            .Except(routed.Select(e => e.Key))
            .Should()
            .BeEmpty("the matrix must not list endpoints that no longer exist");

        // The decision and the code must agree: only the public list may opt out of auth.
        routed
            .Where(e => e.IsAnonymous)
            .Select(e => e.Key)
            .Should()
            .BeEquivalentTo(publicKeys);
    }

    private async Task<MatrixSeed> Seed()
        => await this.httpClientFactory.WithData(async data =>
        {
            // The owner's profile is private, so other users can't see the owner's reading lists.
            await data.SeedProfile(OwnerId, firstName: "Owner", isPrivate: true);
            await data.SeedProfile(OtherUserId, firstName: "Other");
            await data.SeedProfile(AdminId, firstName: "Admin");

            var article = await data.SeedArticle("A seeded article");
            var author = await data.SeedAuthor("Seeded author", creatorId: OwnerId);
            var book = await data.SeedBook("Seeded book", creatorId: OwnerId, authorId: author.Id);
            var review = await data.SeedReview(book.Id, OwnerId);
            var notification = await data.SeedNotification(OwnerId);

            data.ReadingLists.Add(new ReadingListDbModel
            {
                UserId = OwnerId,
                BookId = book.Id,
                Status = ReadingListStatus.CurrentlyReading,
            });

            await data.SaveChangesAsync();

            return new MatrixSeed(
                article.Id,
                author.Id,
                book.Id,
                review.Id,
                notification.Id);
        });
}

// One app host for the whole matrix class: starting a host per test would add minutes.
public sealed class AuthorizationMatrixFixture : IAsyncDisposable
{
    public BookHubWebApplicationFactory Factory { get; } = new();

    public async ValueTask DisposeAsync()
        => await this.Factory.DisposeAsync();
}
