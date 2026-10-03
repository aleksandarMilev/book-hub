namespace BookHub.Tests.Authorization;

using System.Net;
using System.Net.Http.Json;
using Features.Authors.Shared;
using Features.ReadingLists.Shared;

// The single source of truth for who may call which endpoint. Every routed endpoint must be
// either in Protected (with its access rule) or in Public; AuthorizationMatrixIntegration
// enforces that at runtime, so a new endpoint can't be added without deciding its authorization.
public static class AuthorizationMatrix
{
    public const string OwnerId = "owner-id";
    public const string OtherUserId = "other-id";
    public const string AdminId = "test-admin-id";

    // Anonymous by design ([AllowAnonymous] or .AllowAnonymous()), per the Phase 2b table.
    public static readonly IReadOnlyList<PublicEndpoint> Public =
    [
        new("GET", "Articles/{id}", s => $"/Articles/{s.ArticleId}/"),
        new("GET", "Authors/top", _ => "/Authors/top/"),
        new("GET", "Books/top", _ => "/Books/top/"),
        new("GET", "Profile/top", _ => "/Profile/top/"),
        new("GET", "Statistics", _ => "/Statistics"),
        new("GET", "Search/articles", _ => "/Search/articles/?searchTerm=seeded"),
        new("*", "/health", _ => "/health"),
        new("POST", "Identity/register", _ => "/Identity/register/", RequiresBody: true),
        new("POST", "Identity/login", _ => "/Identity/login/", RequiresBody: true),
        new("POST", "Identity/forgot-password", _ => "/Identity/forgot-password/", RequiresBody: true),
        new("POST", "Identity/reset-password", _ => "/Identity/reset-password/", RequiresBody: true),
    ];

    // Everything else requires an authenticated user (the fallback policy).
    // - User: any authenticated user. Endpoints scoped to the caller's own data (Profile/mine,
    //   ReadingChallenges, Notifications lists, ...) are User too: there is no target to get wrong.
    // - Owner: the caller must own the target. WrongUserStatus is 403 for public resources
    //   and 404 for private content whose existence must not leak.
    // - Admin: the Administrator role.
    public static readonly IReadOnlyList<ProtectedEndpoint> Protected =
    [
        // Articles
        new("GET", "Administrator/Articles/{id}", Access.Admin, s => $"/Administrator/Articles/{s.ArticleId}/"),
        new("POST", "Administrator/Articles", Access.Admin, _ => "/Administrator/Articles"),
        new("PUT", "Administrator/Articles/{id}", Access.Admin, s => $"/Administrator/Articles/{s.ArticleId}/"),
        new("DELETE", "Administrator/Articles/{id}", Access.Admin, s => $"/Administrator/Articles/{s.ArticleId}/"),

        // Authors
        new("GET", "Authors/names", Access.User, _ => "/Authors/names/"),
        new("GET", "Authors/{id}", Access.User, s => $"/Authors/{s.AuthorId}/"),
        new("POST", "Authors", Access.User, _ => "/Authors"),
        new("PUT", "Authors/{id}", Access.Owner, s => $"/Authors/{s.AuthorId}/", AuthorForm, HttpStatusCode.Forbidden),
        new("DELETE", "Authors/{id}", Access.Owner, s => $"/Authors/{s.AuthorId}/", WrongUserStatus: HttpStatusCode.Forbidden),
        new("GET", "Administrator/Authors/{id}", Access.Admin, s => $"/Administrator/Authors/{s.AuthorId}/"),
        new("PATCH", "Administrator/Authors/{id}/approve", Access.Admin, s => $"/Administrator/Authors/{s.AuthorId}/approve/"),
        new("PATCH", "Administrator/Authors/{id}/reject", Access.Admin, s => $"/Administrator/Authors/{s.AuthorId}/reject/"),

        // Books
        new("GET", "Books/genre/{id}", Access.User, _ => $"/Books/genre/{Shared.Seed.TestSeeder.OtherGenreId}/"),
        new("GET", "Books/author/{id}", Access.User, s => $"/Books/author/{s.AuthorId}/"),
        new("GET", "Books/{id}", Access.User, s => $"/Books/{s.BookId}/"),
        new("POST", "Books", Access.User, _ => "/Books"),
        new("PUT", "Books/{id}", Access.Owner, s => $"/Books/{s.BookId}/", BookForm, HttpStatusCode.Forbidden),
        new("DELETE", "Books/{id}", Access.Owner, s => $"/Books/{s.BookId}/", WrongUserStatus: HttpStatusCode.Forbidden),
        new("GET", "Administrator/Books/{id}", Access.Admin, s => $"/Administrator/Books/{s.BookId}/"),
        new("PATCH", "Administrator/Books/{id}/approve", Access.Admin, s => $"/Administrator/Books/{s.BookId}/approve/"),
        new("PATCH", "Administrator/Books/{id}/reject", Access.Admin, s => $"/Administrator/Books/{s.BookId}/reject/"),

        // Reading challenges (all scoped to the caller)
        new("GET", "ReadingChallenges/{id}", Access.User, _ => "/ReadingChallenges/2026/"),
        new("PUT", "ReadingChallenges", Access.User, _ => "/ReadingChallenges"),
        new("GET", "ReadingChallenges/{id}/progress", Access.User, _ => "/ReadingChallenges/2026/progress/"),
        new("POST", "ReadingChallenges/check-in", Access.User, _ => "/ReadingChallenges/check-in/"),
        new("GET", "ReadingChallenges/streak", Access.User, _ => "/ReadingChallenges/streak/"),

        // Data importer
        new("POST", "Administrator/DataImporter/all", Access.Admin, _ => "/Administrator/DataImporter/all/"),
        new("POST", "Administrator/DataImporter/articles", Access.Admin, _ => "/Administrator/DataImporter/articles/"),
        new("POST", "Administrator/DataImporter/authors", Access.Admin, _ => "/Administrator/DataImporter/authors/"),
        new("POST", "Administrator/DataImporter/books", Access.Admin, _ => "/Administrator/DataImporter/books/"),
        new("POST", "Administrator/DataImporter/genres", Access.Admin, _ => "/Administrator/DataImporter/genres/"),
        new("POST", "Administrator/DataImporter/books-genres", Access.Admin, _ => "/Administrator/DataImporter/books-genres/"),

        // Genres
        new("GET", "Genres", Access.User, _ => "/Genres/"),
        new("GET", "Genres/{id}", Access.User, _ => $"/Genres/{Shared.Seed.TestSeeder.OtherGenreId}/"),

        // Notifications (private: someone else's is a 404)
        new("GET", "Notifications/last", Access.User, _ => "/Notifications/last/"),
        new("GET", "Notifications", Access.User, _ => "/Notifications/"),
        new("DELETE", "Notifications/{id}", Access.Owner, s => $"/Notifications/{s.NotificationId}/", WrongUserStatus: HttpStatusCode.NotFound),
        new("PATCH", "Notifications/{id}/read", Access.Owner, s => $"/Notifications/{s.NotificationId}/read/", WrongUserStatus: HttpStatusCode.NotFound),

        // Reading lists (the owner's profile is private: other users get a 404, S-04)
        new("GET", "ReadingLists", Access.Owner, _ => $"/ReadingLists?userId={OwnerId}&status={ReadingListStatus.CurrentlyReading}", WrongUserStatus: HttpStatusCode.NotFound),
        new("GET", "ReadingLists/last-currently-reading", Access.Owner, _ => $"/ReadingLists/last-currently-reading?userId={OwnerId}", WrongUserStatus: HttpStatusCode.NotFound),
        new("POST", "ReadingLists", Access.User, _ => "/ReadingLists"),
        new("DELETE", "ReadingLists", Access.User, _ => "/ReadingLists"),

        // Reviews and votes
        new("GET", "Reviews/book/{id}", Access.User, s => $"/Reviews/book/{s.BookId}/"),
        new("GET", "Reviews/{id}", Access.User, s => $"/Reviews/{s.ReviewId}/"),
        new("POST", "Reviews", Access.User, _ => "/Reviews"),
        new("PUT", "Reviews/{id}", Access.Owner, s => $"/Reviews/{s.ReviewId}/", ReviewBody, HttpStatusCode.Forbidden),
        new("DELETE", "Reviews/{id}", Access.Owner, s => $"/Reviews/{s.ReviewId}/", WrongUserStatus: HttpStatusCode.Forbidden),
        new("POST", "Votes", Access.User, _ => "/Votes"),

        // Search (Search/articles is public)
        new("GET", "Search/books", Access.User, _ => "/Search/books/?searchTerm=seeded"),
        new("GET", "Search/genres", Access.User, _ => "/Search/genres/?searchTerm=other"),
        new("GET", "Search/authors", Access.User, _ => "/Search/authors/?searchTerm=seeded"),
        new("GET", "Search/profiles", Access.User, _ => "/Search/profiles/?searchTerm=first"),

        // Profiles (Profile/top is public)
        new("GET", "Profile/mine", Access.User, _ => "/Profile/mine/"),
        new("GET", "Profile/{id}", Access.User, _ => $"/Profile/{OwnerId}/"),
        new("PUT", "Profile", Access.User, _ => "/Profile"),
        new("DELETE", "Profile", Access.User, _ => "/Profile"),
        new("DELETE", "Administrator/Profile/{id}", Access.Admin, _ => $"/Administrator/Profile/{OwnerId}/"),
    ];

    public static ProtectedEndpoint ProtectedByKey(string key)
        => Protected.Single(e => e.Key == key);

    public static PublicEndpoint PublicByKey(string key)
        => Public.Single(e => e.Key == key);

    // Valid bodies for the owner checks: an invalid body would be rejected with a 400 by model
    // validation before the service gets to check ownership.
    private static HttpContent BookForm(MatrixSeed seed)
        => new MultipartFormDataContent
        {
            { new StringContent("Edited by someone else"), "Title" },
            { new StringContent("A valid short description"), "ShortDescription" },
            { new StringContent(new string('l', 250)), "LongDescription" },
        };

    private static HttpContent AuthorForm(MatrixSeed seed)
        => new MultipartFormDataContent
        {
            { new StringContent("Edited by someone else"), "Name" },
            { new StringContent(new string('b', 200)), "Biography" },
            { new StringContent(((int)Nationality.France).ToString()), "Nationality" },
            { new StringContent(((int)Gender.Other).ToString()), "Gender" },
        };

    private static HttpContent ReviewBody(MatrixSeed seed)
        => JsonContent.Create(new
        {
            content = "Edited by someone else",
            rating = 3,
            bookId = seed.BookId,
        });
}

public enum Access
{
    User,
    Owner,
    Admin,
}

public sealed record MatrixSeed(
    Guid ArticleId,
    Guid AuthorId,
    Guid BookId,
    Guid ReviewId,
    Guid NotificationId);

public sealed record ProtectedEndpoint(
    string Method,
    string Route,
    Access Access,
    Func<MatrixSeed, string> Url,
    Func<MatrixSeed, HttpContent>? Body = null,
    HttpStatusCode? WrongUserStatus = null)
{
    public string Key => $"{this.Method} {this.Route}";

    public HttpRequestMessage ToRequest(MatrixSeed seed)
        => new(new HttpMethod(this.Method), this.Url(seed))
        {
            Content = this.Body?.Invoke(seed),
        };
}

public sealed record PublicEndpoint(
    string Method,
    string Route,
    Func<MatrixSeed, string> Url,
    bool RequiresBody = false)
{
    public string Key => $"{this.Method} {this.Route}";
}
