# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

BookHub is a book community platform: a React 18 + Vite + TypeScript SPA in `client/`, an ASP.NET Core (.NET 10) Web API with EF Core (Npgsql) + Identity in `server/`, and PostgreSQL 18 (the official `postgres:18-alpine` image, no custom build).

## Commands

### Full stack (Docker)

```bash
cp .env.example .env
docker compose -f docker-compose.dev.yml --env-file .env up --build
```

Client: `http://localhost:5173`. API + Swagger UI (Development only, served at the root): `http://localhost:8080`. Health: `/health`.

### Server (from repo root)

```bash
dotnet run --project server/BookHub/BookHub.csproj
dotnet build server/BookHub.sln -c Release -warnaserror    # what CI runs (keep 0 warnings)
dotnet test server/BookHub.sln
dotnet test server/BookHub.sln --filter "FullyQualifiedName~BooksIntegration"            # one class
dotnet test server/BookHub.sln --filter "FullyQualifiedName~BooksUnit.TopThree_Should"   # one test
```

Migrations live in `server/BookHub/Data/Migrations` (needs the `dotnet-ef` tool):

```bash
dotnet ef migrations add <Name> --project server/BookHub --output-dir Data/Migrations
```

Migrations are applied automatically on startup only in Development. Production doesn't apply them. The history starts at `InitialPostgres` (the SQL Server migrations were deleted, with no data migration).

For local `dotnet run`, start only the database with `docker compose -f docker-compose.dev.yml --env-file .env up -d postgres`. `appsettings.Development.json` points at `localhost:5432` with the `.env.example` credentials.

### Client (from `client/`)

```bash
npm run dev
npm run build
npm run lint          # npm run lint:fix to auto-fix
npm run typecheck
npm run format:check  # npm run format to write
npx vitest run [path] # single run; `npm run test` is `vitest` (watch mode in a TTY)
```

Husky hooks run from `client/`: **pre-commit** runs lint-staged (eslint --fix + prettier), and **pre-push** runs `typecheck` and `test`. CI (`.github/workflows/ci.yml`) runs on pull requests to and pushes to `master`/`develop`, as two jobs that are the required status checks: **server** (restore, Release build with `-warnaserror`, `dotnet test` with a TRX artifact) and **client** (`npm ci`, lint, typecheck, `npx vitest run`, build). `format:check` isn't in CI yet (F-12). `global.json` pins the .NET SDK (10.0.4xx band, `latestFeature`), and CI installs the SDK from it. Client tests (Vitest) live next to the code they cover; run them with `npx vitest run`.

## Server architecture

**Vertical feature slices.** Each `server/BookHub/Features/<Feature>/` has the same layout:

- `Data/Models` (`*DbModel`) and `Data/Configuration` (`IEntityTypeConfiguration`, picked up by `ApplyConfigurationsFromAssembly`)
- `Service/` with `I<Name>Service` + `<Name>Service`, and `Service/Models` (`*ServiceModel`)
- `Web/` with controllers, `Web/Models` (`*WebModel`) and a feature-local `ApiRoutes` static class of route fragments
- `Shared/` with `*Mapping.cs` (hand-written extension methods such as `ToDbModel`, `ToCreateServiceModel`, and `IQueryable` projections; there's no AutoMapper) and `Constants`

The request flow is: WebModel → `.ToCreateServiceModel()` → service → DbModel. Services return ServiceModels.

**DI is convention-based** (`Infrastructure/Extensions/ServiceCollectionExtensions.AddServices`). Each concrete class is registered against the interface named exactly `I{ClassName}`, with a lifetime taken from the marker interface that interface extends: `ITransientService`, `IScopedService` or `ISingletonService` (in `Infrastructure/Services/ServiceLifetimes`). A new service isn't registered unless it follows both conventions. Don't add manual registrations.

**Controllers.**

- User endpoints inherit `Common/ApiController` (route `[controller]`).
- Admin endpoints inherit `Areas/Admin/Web/AdminApiController`: route `Administrator/[controller]`, `[Authorize(Roles = "Administrator")]`. Features with both kinds split them into `Web/User/` and `Web/Admin/`, which can share a controller name (for example, two `BooksController`s).
- Mutating service methods return `Infrastructure/Services/Result` (`Result` / `ResultWith<T>`, implicitly convertible from `bool` or an error-message `string`, so services just `return true;` or `return errorMessage;`). A failure carries an `ErrorKind`: a plain string is `BadRequest` (400), and `Result.NotFound(msg)` / `Result.Forbidden(msg)` / `Result.Conflict(msg)` (also on `ResultWith<T>`) give 404 / 403 / 409. Controllers map them with `this.NoContentOrProblem(result)`, `this.OkOrProblem(result, selector)` or `this.CreatedAtRouteOrProblem(result, routeName, routeValues)` (`Infrastructure/Extensions/ControllerExtensions`).
- The global `ModelOrNotFoundActionFilter` turns any `ObjectResult` with a `null` value into 404, so `return this.Ok(await service.Details(id))` is the idiom for nullable lookups.

Authorization and error responses follow the rules in **API conventions** below.

**`BookHubDbContext` behaviour** (it depends on `ICurrentUserService`, so it is per-request):

- **Soft delete:** removing an `IDeletableEntity` is rewritten to `IsDeleted = true`. Created/Modified audit fields are filled in `SaveChanges` for every `IEntity`, and Deleted fields for `IDeletableEntity`.
- **The current user is read per query, never at construction.** `CurrentUserId` and `IsAdmin` (used by the query filters) are getters over `ICurrentUserService`. The scoped context can be created before the user is known (`OnTokenValidated` resolves it during authentication), so don't cache user state in the context's constructor or fields.

**PostgreSQL conventions:**

- **Timestamps** (`CreatedOn`, `ModifiedOn`, `DeletedOn`, `CompletedOn`, …) are `timestamptz`. Npgsql only accepts `DateTimeKind.Utc` for them, so always write `DateTime.UtcNow` (or a `DateTimeKind.Utc` literal, e.g. in `HasData`). They're read back as UTC and serialize with a `Z`.
- **Date-only values** (`DateOfBirth`, `PublishedDate`, `BornAt`, `DiedAt`) stay `DateTime` in C# but are mapped with `.HasColumnType("date")` in their configurations. Do the same for any new date-only property. They read back as `Kind=Unspecified`, so the JSON has no `Z`, and the client's `new Date()` doesn't shift the day.
- **Case sensitivity:** string equality is case-sensitive in PostgreSQL. Identity lookups use the `Normalized*` columns. For a new case-insensitive filter use `EF.Functions.ILike`, and compare explicitly for uniqueness checks. Don't use nondeterministic collations. The database is initialized with the ICU root collation (`POSTGRES_INITDB_ARGS` in Compose and in the test container) for linguistic ordering and Unicode case folding.
- Never build SQL by string concatenation with user input. The only raw SQL in the app is the constant anchor in `StatisticsQuery`.
- **Global query filters**, built by reflection: `IDeletableEntity` hides deleted rows. `IApprovableEntity` (Books, Authors) hides unapproved rows unless the current user is an admin or matches the entity's `CreatorId`. Admin/approval code uses `.IgnoreQueryFilters().ApplyIsDeletedFilter()` to see unapproved rows while still excluding deleted ones.

**Approval workflow (Books, Authors).** A non-admin's create produces an unapproved entity and a notification to every admin (`IAdminService.GetIds()`). An admin's create is auto-approved. A non-admin's **edit doesn't modify the entity**: it upserts a pending row in `BookEdits`/`AuthorEdits` (pending images go under a separate pending image path). `Details` shows the pending edit on top of the entity only to the creator and admins. Everyone else sees the approved version. Admin `Approve` copies the pending edit onto the entity and deletes it, and `Reject` discards it. An admin's own edit applies directly and discards any pending edit. Approving a never-approved entity increments the creator's `CreatedBooksCount`/`CreatedAuthorsCount`. Changes to these entities usually need to touch both the main and `*Edit` models/mappings.

**Other cross-cutting pieces:**

- `IImageWriter` / `IImageValidator`: images go to `wwwroot`. Entities and service models implement `IImageDdModel` / `IImageServiceModel`.
- `IPageClamper` and `PaginatedModel<T>` handle pagination.
- A global per-IP fixed-window rate limiter.
- **Search** (`Features/Search`) uses PostgreSQL full-text search. Books, Authors, Articles, Genres and Profiles have a shadow `SearchVector` property: a generated, stored `tsvector` column (`simple` config: lowercased, no stemming or stopwords, because content mixes English and Bulgarian) with a GIN index. Everything goes through `Infrastructure/Extensions/FullTextSearchExtensions`:
  - `HasSearchVector(...)` is used in the entity configurations;
  - `ApplyFullTextSearch(searchTerm)` is applied to the DbModel query *before* projection;
  - `ToPrefixTsQuery` turns input into `term:* & term:*`, splitting on any non-letter/digit so tsquery operators can't get through. The tsquery is always a parameter.
  - Blank input means no filter. Input with no letters or digits returns an empty page.
- **Seed/demo data:** the initial migration seeds the "Other" genre (`HasData`, ID `52e607d4-…`). The admin endpoints `POST /Administrator/DataImporter/{all|books|authors|genres|articles|books-genres}/` import `Features/DataImporter/Data/*.json` and skip IDs that already exist.
- Development startup creates the admin `admin@mail.com` / `admin1234` and a built-in user. Outside Development, `UseProductionAdminRole` creates the admin only when `BootstrapAdmin:Enabled` is true.

**Environment-specific behaviour:**

- Development: relaxed Identity password rules, JWT issuer/audience not validated, CORS allow-any.
- Non-Development: `Cors:AllowedOrigins` (semicolon-separated) is required or startup throws.
- Config comes from `appsettings*.json` or env vars (`JwtSettings__*`, `EmailSettings__*`, `ConnectionStrings__DefaultConnection`). See the README for the full env var table.

## API conventions

### Authorization

- **Every endpoint requires an authenticated user by default.** The fallback authorization policy (`RequireAuthenticatedUser`, in `ServiceCollectionExtensions.AddJwtAuthentication`) applies to any endpoint without its own `[Authorize]`/`[AllowAnonymous]`, and to requests that match no endpoint: an anonymous request to an unknown URL gets a 401, not a 404.
- **Public endpoints opt out explicitly:** `[AllowAnonymous]` on the action or controller, or `.AllowAnonymous()` on a minimal endpoint (as `/health` does). The public set is:
  - the home page: `Books/top`, `Authors/top`, `Profile/top` and `Statistics`;
  - `Articles/{id}` and `Search/articles`;
  - **the read-only catalog** (Phase 2c, like Goodreads): `Books/{id}`, `Books/genre/{id}`, `Books/author/{id}`, `Authors/{id}`, `Genres`, `Genres/{id}`, `Reviews/book/{id}`, `Reviews/{id}`, `Search/books`, `Search/authors` and `Search/genres`;
  - the four `Identity` endpoints and `/health`.

  Everything else needs a user: profiles (`Profile/{id}`, `Profile/mine`), `Search/profiles`, `Authors/names` (the create-book form), and every endpoint that writes data.
- **Public endpoints must work with no user.** `ICurrentUserService.GetId()` is `null` and `IsAdmin()` is `false` for an anonymous caller:
  - never write `GetId()!` on a public code path;
  - a creator/owner comparison must require a non-null ID (`string.Equals(null, null)` is `true`, so an anonymous caller would "own" every row with a null `CreatorId`);
  - user-specific fields stay in the response with an empty value (`ReadingStatus` is `null`), so the shape doesn't change;
  - the query filters already give an anonymous caller approved, non-deleted rows only (the creator clause checks `CurrentUserId != null`). Children of an approvable entity go through the filtered parent: a review of an unapproved or deleted book is a 404, like a missing one (`ReviewService` checks `data.Books`).

  On an `[AllowAnonymous]` endpoint an invalid or rejected token (expired, a deleted user) isn't a 401: the request is served anonymously. Tests that need a 401 for a bad token must call a protected endpoint.
- **Static files** (`wwwroot/images`: covers, author photos, avatars) are public: `UseStaticFiles` short-circuits before `UseAuthorization`. Keep it before authentication in `Program.cs`. The Swagger UI (Development) also runs before authentication.
- **Owner checks** live in the services (`CreatorId == caller || admin`) and return `Result.Forbidden` (403) for public resources or `Result.NotFound` (404) for private content. Always take the acting user from the claims (`ICurrentUserService`), never from the route or body.
- **Deleted users:** `OnTokenValidated` rejects the token (401) when the user no longer exists or is soft-deleted. It's one primary-key query per authenticated request. There is no role re-validation or refresh token yet (the rest of S-06 is Phase 4).
- **Every new endpoint must be added to `server/BookHub.Tests/Authorization/AuthorizationMatrix.cs`**, either to `Protected` (with its access: `User`, `Owner` with the wrong-user status, or `Admin`) or to `Public`. `AuthorizationMatrixIntegration.EveryEndpoint_ShouldBeInTheMatrixOrThePublicList` enumerates the routed endpoints at runtime and fails otherwise. It also fails if the `[AllowAnonymous]` endpoints and the `Public` list disagree.

### Error responses

Error responses are always RFC 9457 ProblemDetails (`application/problem+json`) with a `traceId`. The human-readable message is in `detail`, and validation errors are a ValidationProblemDetails with per-field `errors`.

- **Status codes:**
  - 400: validation errors and invalid input;
  - 401: not authenticated (or a deleted user's token);
  - 403: authenticated, but modifying a public resource you don't own (or a role-gated endpoint);
  - 404: not found, **or private content whose existence must not leak** (someone else's notification, a private profile's reading lists, someone else's unapproved book/author, and the reviews of a book you can't see);
  - 409: duplicates and conflicts.
- **Messages returned to clients** never contain internal type names (`…DbModel`) or IDs (S-11). Use `Common.Constants.ErrorMessages.ResourceNotFound` / `ResourceForbidden` with a friendly resource name ("book", "review"), and keep the type name and IDs in the log template.
- **Exceptions** go through `Infrastructure/ExceptionHandling/GlobalExceptionHandler` (`UseExceptionHandler()` in every environment):
  - `ExpectedFailures` maps a `DbUpdateException` by Postgres `SqlState`: a unique violation → 409; a foreign key violation → 400, or 409 when deleting;
  - `ImageValidationException` → 400;
  - anything else → 500 with a generic `detail`, plus the exception only in Development.

  Back "one per X" rules with a unique index (as `Reviews (CreatorId, BookId)` is): the service's read-then-write check alone loses races. On a soft-deletable table, filter the index with `.HasFilter("NOT \"IsDeleted\"")` (as Reviews, `BookEdits` and `AuthorEdits` do), or a soft-deleted row blocks the next one.
- `UseStatusCodePages()` gives empty-body 401/403/404 responses a ProblemDetails body. The rate limiter's 429 is a ProblemDetails too.

## Server tests (`server/BookHub.Tests`)

xUnit v3 + FluentAssertions + NSubstitute + Testcontainers. The project references `xunit.v3.mtp-off` and runs through VSTest (`xunit.runner.visualstudio` + `Microsoft.NET.Test.Sdk`), so plain `dotnet test` works. Don't switch to the default `xunit.v3` package without also opting into Microsoft Testing Platform in `global.json`: MTP v2 fails `dotnet test` in VSTest mode on the .NET 10 SDK. The test project is an executable (`OutputType Exe`). `IAsyncLifetime` members return `ValueTask`. Test classes run in parallel, at most 4 at a time, and the tests inside a class run one after another (`[assembly: Parallelization(Mode = ParallelMode.Collections, MaxThreads = 4)]` in `AssemblyInfo.cs`; see the rules under **Testing approach**). The analyzer rule xUnit1051 is enforced: a call inside a test that takes a `CancellationToken` must get `TestContext.Current.CancellationToken`, or the `-warnaserror` build fails (`dotnet format analyzers server/BookHub.Tests/BookHub.Tests.csproj --diagnostics xUnit1051 --severity warn` applies the fix). Test classes are named `<Feature>Integration.cs`, `<Workflow>WorkflowIntegration.cs` (in `Workflows/`) or, for pure logic only, `<Subject>Unit.cs`.

**Docker must be running for `dotnet test`.** Every test uses a real PostgreSQL database:

- `Shared/Database/PostgresServer` starts **one** `postgres:18-alpine` container per test run (a lazy static; Testcontainers' Ryuk removes it at exit). It uses the same ICU initdb arguments as Compose. Keep its image on the same major as the Compose files. On first use it applies the **real migrations** (`MigrateAsync`) to a `bookhub_template` database.
- Each test gets its own database cloned from that template (`CREATE DATABASE … TEMPLATE`, roughly 0.1 s), so every test starts from the migrated schema plus the HasData seed ("Other" genre). Disposing the `TestDatabase` drops it.
- **Integration tests** use `BookHubWebApplicationFactory`:
  - It runs `Program` in the `"Testing"` environment, where `Program.cs` skips CORS and the database registration. The factory registers Npgsql against its own cloned database, swaps in `ImageWriterMock` and `AdminServiceMock("test-admin-id")`, and replaces JWT with a test auth scheme. The `Create*Client` helpers need the test scheme.
  - Two virtual switches change that:
    - `UseRealAdminService => true` keeps the app's real `AdminService`, so the admins are the users seeded with `TestSeeder.SeedAdmin` (`Workflows/WorkflowFactory` does this);
    - `UseTestAuthentication => false` keeps the real JwtBearer scheme with signed tokens from `/Identity/register|login`. `Shared/Identity/RealJwtFactory` does this (with the real `AdminService`), and also has `RegisterAndAuthenticate(username)`, `RegisterForm(...)` and an `EmailSender` (`Shared/Mocks/RecordingEmailSender`: records welcome and reset emails, `FailWelcomeEmails` simulates an SMTP outage).
  - `ImageWriterMock.FailWritesFor = "profiles"` makes image writes for one resource throw (used to fail profile creation).
  - Call `ResetDatabase()` in `InitializeAsync` (it clones a fresh database) and dispose the factory in `DisposeAsync`.
  - Clients: `CreateUserClient(userId, username)`, `CreateAdminClient(...)` and `CreateAnonymousClient()`. The authenticated ones send `Authorization: <scheme> user|admin:<id>:<username>`.
  - Authorization coverage lives in `Authorization/AuthorizationMatrixIntegration` (table-driven over `AuthorizationMatrix`, one shared host per class through `AuthorizationMatrixFixture`, a fresh database per test). Add every new endpoint there.
  - Seed and assert through `factory.WithData(data => …)` with the `Shared/Seed/TestSeeder` extensions (`SeedUser`, `SeedAdmin`, `SeedProfile`, `SeedGenre`, `SeedAuthor`, `SeedBook`, `SeedArticle`, `SeedReview`, `SeedNotification`). Seed a `UserDbModel` for any user ID the request uses, because PostgreSQL enforces the foreign keys.
  - Assert error responses with `await response.ShouldBeProblem(HttpStatusCode.X, "detail")` (`Shared/Utils/ProblemDetailsAssertions`). It checks the status, the `application/problem+json` content type, the `traceId` and, optionally, `detail`.
  - In workflows, `ShouldHaveStatus(...)` and `ShouldHaveBody<T>(...)` (`Shared/Utils/ResponseAssertions`) print the response body when the status is wrong. `Shared/Utils/Forms` builds valid book, author and profile multipart bodies.

## Testing approach

These rules apply to all new and changed server tests:

- **Prefer integration tests over unit tests:** real PostgreSQL through Testcontainers, real HTTP through `WebApplicationFactory`, real DI. Each test validates a **workflow** and its observable outcome (HTTP status, response body, database state), not that a mock was called.
- **Unit tests are only for pure logic with no I/O** (for example `FullTextSearchExtensions.ToPrefixTsQuery`, validators, mapping helpers).
- **Mock only true external boundaries:** email sending (`IEmailSender`), and the file system (`IImageWriter`) where writing real files is impractical. Never mock our own services or the DbContext.
- Make new integration tests cheap to write by extending `TestSeeder` and the factory's client helpers rather than adding per-class copies.
- **Workflow tests** (`server/BookHub.Tests/Workflows/`) drive a whole user journey through the API and assert every step (status, body, database state, notifications):
  - `BookApprovalWorkflowIntegration`: a non-admin's book → every admin notified → hidden from anonymous visitors, lists and search → approved → creator notified and counted → public; rejection; no admins;
  - `BookEditWorkflowIntegration`: pending edit → approved; a second pending edit superseded by an admin's direct edit; editing again after a rejection; a deleted pending genre falls back to "Other"; an edit made while the book awaited approval;
  - `AuthorApprovalWorkflowIntegration`: the approval lifecycle and rejection for authors;
  - `IdentityWorkflowIntegration`: register with SMTP down → login → forgot password → reset from the emailed link → only the new password works;
  - `ReviewWorkflowIntegration`: create/edit/delete reviews keep the book's and author's ratings and the profile's review count in step; vote toggling;
  - `ReadingProgressWorkflowIntegration`: yearly goal → currently reading → read → challenge progress and profile counters → check-in streak → a second check-in is a 409;
  - `ReadingListPrivacyWorkflowIntegration`: a private profile's lists are hidden from others but not from the owner or an admin, until it becomes public;
  - `AccountDeletionWorkflowIntegration`: a deleted account loses access (401, no login, profile and lists 404); its review stays public and its username stays taken (current behavior).
- **Tests run in parallel across classes**, so a class must not share mutable state with another one:
  - no `static` mutable fields in test code (counters, caches, lists, mocks): keep state on the class instance, the factory, or a class fixture. `PostgresServer` is the only exception, and its shared state is guarded by locks;
  - no `Environment.SetEnvironmentVariable`: pass settings to the host with `UseSetting` (as `ConfigureTestSettings` does);
  - in-memory app state (the 30 s statistics cache, the welcome-email queue, the rate limiter) is per host, so it's safe as long as each class owns its factory;
  - every test gets its own database. `PostgresServer` serializes the clone step (`CloneLock` around `CREATE DATABASE … TEMPLATE`, because concurrent clones of one template fail); it takes about 0.1 s. The test container runs with `max_connections=300` for the parallel hosts' connection pools;
  - a class that really can't run alongside others goes into a non-parallel collection (`[CollectionDefinition(DisableParallelization = true)]`) with a comment saying why. Don't turn parallelization off globally;
  - the per-host rate limiter allows 240 requests a minute, so a class that shares one host across many tests (`AuthorizationMatrixIntegration`, about 150 requests) has to stay below it.

## Client architecture

- **Feature folders** in `client/src/features/<feature>/` hold `api/api.ts`, `hooks/`, `components/` and `types/`. Shared code is in `src/shared/`, and the app shell, router and route guards (`AuthenticatedRoute`, `AdminRoute`) are in `src/app/`.
- **Imports:** always use the `@/` alias (it maps to `src/`), never relative paths across features. Imports are auto-sorted by `simple-import-sort`. Type-only imports must use `import type` (`verbatimModuleSyntax`).
- **API layer** (`shared/api/http.ts`): the axios instances are `http` (base `VITE_REACT_APP_SERVER_URL`, default `http://localhost:8080`) and `httpAdmin` (`/administrator`). Every API function:
  - takes the JWT `token` and an optional `AbortSignal` explicitly, and builds its config with `getAuthConfig(token, signal)` / `getPublicConfig(signal)`, or `getConfig(token, signal)` for public catalog endpoints that anonymous visitors also call (it sends no `Authorization` header when the token is empty)
  - wraps its call in `try { … } catch (e) { return processError(e, fallbackMessage) }`. `processError` rethrows cancellations. For a 4xx it otherwise throws an `Error` carrying the ProblemDetails `detail`, or the first validation message from `errors`. In every other case (a 5xx, a generic title only, a network error) the `Error` carries the fallback message.
- **Paths:** all API paths are in `shared/lib/constants/api.ts` (`routes`), and fallback error strings are in `shared/lib/constants/errorMessages.ts`.
- **Auth state:** a Zustand store persisted to `localStorage` (`shared/stores/auth`), read through `useAuth()` for `token`, `userId`, `isAdmin` and `isAuthenticated`.
- **Pages** are lazy-loaded in `src/app/routes.tsx`. The catalog pages (book, author and genre lists and details, and a book's reviews) have no route guard; on them, account-only actions show a "Log in to …" link to the login page for anonymous visitors. Everything else stays behind `AuthenticatedRoute`/`AdminRoute`.
- **i18n:** i18next with `en` and `bg` namespaces in `shared/i18n/locales/<lang>/<namespace>.json`. Add new keys to both languages. A new namespace also has to be imported and registered in `shared/i18n/i18n.ts`.
- **TypeScript is very strict:** `noUncheckedIndexedAccess`, `exactOptionalPropertyTypes` and `verbatimModuleSyntax` are all on. For example, an optional property can't be assigned `undefined` unless its type includes it.
- **Libraries:** forms use Formik + Yup, and the UI uses Bootstrap / react-bootstrap / MDB.

Note: the app is about to have a huge refactoring. Findings during the code review are at docs\review\2026-10-code-review.md
