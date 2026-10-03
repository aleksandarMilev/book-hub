namespace BookHub.Tests.ErrorHandling;

using System.Net;
using Data;
using Features.Challenges.Data.Models;
using Features.Reviews.Data.Models;
using FluentAssertions;
using Infrastructure.ExceptionHandling;
using Infrastructure.Services.ImageWriter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Seed;
using Shared.Utils;

// The global error handling: every failure is a ProblemDetails with a traceId, expected
// failures map to 4xx, and nothing internal leaks from a 500. The failures are triggered by
// test-only endpoints (ErrorEndpointsStartupFilter) running the real pipeline and database.
public sealed class ErrorHandlingIntegration : IAsyncLifetime
{
    private const string UserId = "test-user";

    private readonly ErrorEndpointsFactory httpClientFactory = new();

    public async ValueTask InitializeAsync()
    {
        await this.httpClientFactory.ResetDatabase();
        await this.httpClientFactory.WithData(data => data.SeedUser(UserId, "user"));
    }

    public ValueTask DisposeAsync()
    {
        this.httpClientFactory.Dispose();
        return ValueTask.CompletedTask;
    }

    [Fact]
    public async Task UnhandledException_ShouldReturnInternalServerErrorProblem_WithoutExceptionDetails()
    {
        var httpClient = this.httpClientFactory.CreateUserClient(UserId);

        var response = await httpClient.GetAsync(ErrorEndpointsStartupFilter.Unhandled);

        var problem = await response.ShouldBeProblem(
            HttpStatusCode.InternalServerError,
            GlobalExceptionHandler.UnexpectedErrorDetail);

        problem.Extensions.Should().NotContainKey("exception");

        var body = await response.Content.ReadAsStringAsync();
        body.Should().NotContain(ErrorEndpointsStartupFilter.SecretMessage);
        body.Should().NotContain(nameof(InvalidOperationException));
    }

    [Fact]
    public async Task UniqueViolation_ShouldReturnConflictProblem()
    {
        var httpClient = this.httpClientFactory.CreateUserClient(UserId);

        var response = await httpClient.PostAsync(
            ErrorEndpointsStartupFilter.DuplicateCheckIn,
            content: null);

        await response.ShouldBeProblem(
            HttpStatusCode.Conflict,
            ExpectedFailures.UniqueViolationDetail);
    }

    [Fact]
    public async Task DuplicateReview_InsertedPastTheServiceCheck_ShouldReturnConflictProblem_AndAlso_ShouldKeepOneReview()
    {
        var book = await this.httpClientFactory.WithData(data => data.SeedBook("Raced book"));
        var httpClient = this.httpClientFactory.CreateUserClient(UserId);

        var response = await httpClient.PostAsync(
            $"{ErrorEndpointsStartupFilter.DuplicateReview}?bookId={book.Id}",
            content: null);

        await response.ShouldBeProblem(
            HttpStatusCode.Conflict,
            ExpectedFailures.UniqueViolationDetail);

        var reviews = await this.httpClientFactory.WithData(data => data
            .Reviews
            .CountAsync(r => r.BookId == book.Id && r.CreatorId == UserId));

        reviews.Should().Be(1);
    }

    [Fact]
    public async Task ForeignKeyViolation_OnInsert_ShouldReturnBadRequestProblem()
    {
        var httpClient = this.httpClientFactory.CreateUserClient(UserId);

        var response = await httpClient.PostAsync(
            ErrorEndpointsStartupFilter.MissingReference,
            content: null);

        await response.ShouldBeProblem(
            HttpStatusCode.BadRequest,
            ExpectedFailures.MissingReferenceDetail);
    }

    [Fact]
    public async Task ImageValidationException_ShouldReturnBadRequestProblem_WithTheValidationMessage()
    {
        var httpClient = this.httpClientFactory.CreateUserClient(UserId);

        var response = await httpClient.GetAsync(ErrorEndpointsStartupFilter.InvalidImage);

        await response.ShouldBeProblem(
            HttpStatusCode.BadRequest,
            ErrorEndpointsStartupFilter.ImageMessage);
    }

    [Fact]
    public async Task Unauthenticated_ShouldReturnUnauthorizedProblem()
    {
        var httpClient = this.httpClientFactory.CreateAnonymousClient();

        var response = await httpClient.GetAsync("/Genres/");

        await response.ShouldBeProblem(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task NonAdmin_OnAdminEndpoint_ShouldReturnForbiddenProblem()
    {
        var httpClient = this.httpClientFactory.CreateUserClient(UserId);

        var response = await httpClient.DeleteAsync($"/Administrator/Articles/{Guid.NewGuid()}/");

        await response.ShouldBeProblem(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task UnknownRoute_ShouldReturnNotFoundProblem()
    {
        var httpClient = this.httpClientFactory.CreateUserClient(UserId);

        var response = await httpClient.GetAsync("/no-such-route/");

        await response.ShouldBeProblem(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ModelValidationError_ShouldReturnValidationProblem_WithTheFieldErrors()
    {
        var httpClient = this.httpClientFactory.CreateUserClient(UserId);

        var response = await httpClient.PostAsync("/Books", new MultipartFormDataContent());

        await response.ShouldBeProblem(HttpStatusCode.BadRequest);

        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("\"errors\"");
    }

    private sealed class ErrorEndpointsFactory : BookHubWebApplicationFactory
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureTestServices(services => services
                .AddTransient<IStartupFilter, ErrorEndpointsStartupFilter>());
        }
    }

    // Appends the endpoints after the app's own pipeline, so requests to them still pass
    // through UseExceptionHandler, authentication and authorization first.
    private sealed class ErrorEndpointsStartupFilter : IStartupFilter
    {
        public const string Unhandled = "/test-errors/unhandled";
        public const string DuplicateCheckIn = "/test-errors/duplicate-check-in";
        public const string MissingReference = "/test-errors/missing-reference";
        public const string DuplicateReview = "/test-errors/duplicate-review";
        public const string InvalidImage = "/test-errors/invalid-image";

        public const string SecretMessage = "secret-internal-detail";
        public const string ImageMessage = "Image file is empty.";

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
            => app =>
            {
                next(app);

                app.Map(Unhandled, branch => branch.Run(
                    _ => throw new InvalidOperationException(SecretMessage)));

                app.Map(InvalidImage, branch => branch.Run(
                    _ => throw new ImageValidationException(ImageMessage)));

                // A real unique violation (23505) on ReadingCheckIns (UserId, Date).
                app.Map(DuplicateCheckIn, branch => branch.Run(async context =>
                {
                    var data = context.RequestServices.GetRequiredService<BookHubDbContext>();
                    var today = DateOnly.FromDateTime(DateTime.UtcNow);

                    data.ReadingCheckIns.Add(new ReadingCheckInDbModel { UserId = UserId, Date = today });
                    await data.SaveChangesAsync();

                    data.ChangeTracker.Clear();

                    data.ReadingCheckIns.Add(new ReadingCheckInDbModel { UserId = UserId, Date = today });
                    await data.SaveChangesAsync();
                }));

                // B-20: two live reviews by one user for one book, inserted directly so the
                // service's duplicate check is bypassed (as in a race). The filtered unique
                // index on Reviews (CreatorId, BookId) rejects the second one.
                app.Map(DuplicateReview, branch => branch.Run(async context =>
                {
                    var data = context.RequestServices.GetRequiredService<BookHubDbContext>();
                    var bookId = Guid.Parse(context.Request.Query["bookId"]!);

                    for (var i = 0; i < 2; i++)
                    {
                        data.Reviews.Add(new ReviewDbModel
                        {
                            Content = "A racing review",
                            Rating = 4,
                            BookId = bookId,
                            CreatorId = UserId,
                        });

                        await data.SaveChangesAsync();
                        data.ChangeTracker.Clear();
                    }
                }));

                // A real foreign key violation (23503): a review for a book that doesn't exist.
                app.Map(MissingReference, branch => branch.Run(async context =>
                {
                    var data = context.RequestServices.GetRequiredService<BookHubDbContext>();

                    data.Reviews.Add(new ReviewDbModel
                    {
                        Content = "Review of a missing book",
                        Rating = 3,
                        BookId = Guid.NewGuid(),
                        CreatorId = UserId,
                    });

                    await data.SaveChangesAsync();
                }));
            };
    }
}
