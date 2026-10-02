# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

BookHub is a book community platform: a React 18 + Vite + TypeScript SPA in `client/`, an ASP.NET Core (.NET 10) Web API with EF Core + Identity in `server/`, and a SQL Server 2022 image with Full-Text Search in `sqlserver/`.

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
dotnet build server/BookHub/BookHub.csproj -c Release      # what CI runs
dotnet test server/BookHub.sln
dotnet test server/BookHub.sln --filter "FullyQualifiedName~BooksIntegration"            # one class
dotnet test server/BookHub.sln --filter "FullyQualifiedName~BooksUnit.TopThree_Should"   # one test
```

Migrations live in `server/BookHub/Data/Migrations` (needs the `dotnet-ef` tool):

```bash
dotnet ef migrations add <Name> --project server/BookHub --output-dir Data/Migrations
```

Migrations are applied automatically on startup only in Development. Production doesn't apply them.

### Client (from `client/`)

```bash
npm run dev
npm run build
npm run lint          # npm run lint:fix to auto-fix
npm run typecheck
npm run format:check  # npm run format to write
npx vitest run [path] # single run; `npm run test` is `vitest` (watch mode in a TTY)
```

Husky hooks run from `client/`: **pre-commit** runs lint-staged (eslint --fix + prettier), and **pre-push** runs `typecheck` and `test`. CI (`.github/workflows/build-and-deploy.yml`) only builds the API and the client on pushes to `develop`/`master`; it doesn't run tests or lint. There are currently no client test files.

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
- Mutating service methods return `Infrastructure/Services/Result` (`Result` / `ResultWith<T>`, implicitly convertible from `bool` or an error-message `string`, so services just `return true;` or `return errorMessage;`). Controllers map those with `this.NoContentOrBadRequest(result)` / `this.OkOrBadRequest(...)`, and the bad-request body is `{ errorMessage }`.
- The global `ModelOrNotFoundActionFilter` turns any `ObjectResult` with a `null` value into 404, so `return this.Ok(await service.Details(id))` is the idiom for nullable lookups.

**`BookHubDbContext` behaviour** (it depends on `ICurrentUserService`, so it is per-request):

- **Soft delete:** removing an `IDeletableEntity` is rewritten to `IsDeleted = true`. Created/Modified/Deleted audit fields are filled in `SaveChanges`.
- **Global query filters**, built by reflection: `IDeletableEntity` hides deleted rows. `IApprovableEntity` (Books, Authors) hides unapproved rows unless the current user is an admin or matches the entity's `CreatorId`. Admin/approval code uses `.IgnoreQueryFilters().ApplyIsDeletedFilter()` to see unapproved rows while still excluding deleted ones.

**Approval workflow (Books, Authors).** A non-admin's create produces an unapproved entity and a notification to the admin (`IAdminService.GetId()`). An admin's create is auto-approved. An **edit doesn't modify the entity**: it upserts a pending row in `BookEdits`/`AuthorEdits` (pending images go under a separate pending image path). `Details` shows the pending edit on top of the entity only to the creator and admins. Everyone else sees the approved version. Admin `Approve` copies the pending edit onto the entity and deletes it, and `Reject` discards it. Changes to these entities usually need to touch both the main and `*Edit` models/mappings.

**Other cross-cutting pieces:**

- `IImageWriter` / `IImageValidator`: images go to `wwwroot`. Entities and service models implement `IImageDdModel` / `IImageServiceModel`.
- `IPageClamper` and `PaginatedModel<T>` handle pagination.
- A global per-IP fixed-window rate limiter.
- Search (`Features/Search`) uses `EF.Functions.Contains`, which requires SQL Server Full-Text Search. The FTS catalog is created by the `FullTextSearch` migration.
- Seed/demo data: the admin endpoints `POST /Administrator/DataImporter/{all|books|authors|genres|articles|books-genres}/` import `Features/DataImporter/Data/*.json`. The README's `Features/*/Data/Seed` path is out of date.
- Development startup creates the admin `admin@mail.com` / `admin1234` and a built-in user. `UseProductionAdminRole` (driven by `BootstrapAdmin:*` config) is intentionally unused, but it's kept for disaster recovery: don't delete it.

**Environment-specific behaviour:**

- Development: relaxed Identity password rules, JWT issuer/audience not validated, CORS allow-any.
- Non-Development: `Cors:AllowedOrigins` (semicolon-separated) is required or startup throws.
- Config comes from `appsettings*.json` or env vars (`JwtSettings__*`, `EmailSettings__*`, `ConnectionStrings__DefaultConnection`). See the README for the full env var table.

## Server tests (`server/BookHub.Tests`)

xUnit + FluentAssertions + NSubstitute. Test parallelization is disabled assembly-wide. Each feature has `<Feature>Unit.cs` and `<Feature>Integration.cs`:

- **Unit tests** build the service directly against an in-memory **SQLite** `BookHubDbContext` with substituted dependencies.
- **Integration tests** use `BookHubWebApplicationFactory`. It runs `Program` in the `"Testing"` environment, where `Program.cs` skips CORS and SQL Server registration. The factory swaps in SQLite in-memory, `ImageWriterMock` and `AdminServiceMock("test-admin-id")`, and replaces JWT with a test auth scheme. Use `CreateUserClient(userId, username)` / `CreateAdminClient(...)`. They send `Authorization: <scheme> user|admin:<id>:<username>`. Call `ResetDatabase()` in `InitializeAsync`, and seed matching `UserDbModel` rows.
- Genres uses its own `GenresWebApplicationFactory` (EF InMemory).
- Full-text search can't run on SQLite, so search isn't covered.

## Client architecture

- **Feature folders** in `client/src/features/<feature>/` hold `api/api.ts`, `hooks/`, `components/` and `types/`. Shared code is in `src/shared/`, and the app shell, router and route guards (`AuthenticatedRoute`, `AdminRoute`) are in `src/app/`.
- **Imports:** always use the `@/` alias (it maps to `src/`), never relative paths across features. Imports are auto-sorted by `simple-import-sort`. Type-only imports must use `import type` (`verbatimModuleSyntax`).
- **API layer** (`shared/api/http.ts`): the axios instances are `http` (base `VITE_REACT_APP_SERVER_URL`, default `http://localhost:8080`) and `httpAdmin` (`/administrator`). Every API function:
  - takes the JWT `token` and an optional `AbortSignal` explicitly, and builds its config with `getAuthConfig(token, signal)` / `getPublicConfig(signal)`
  - wraps its call in `try { … } catch (e) { return processError(e, fallbackMessage) }`. `processError` rethrows cancellations, and otherwise throws an `Error` carrying the server's `errorMessage`.
- **Paths:** all API paths are in `shared/lib/constants/api.ts` (`routes`), and fallback error strings are in `shared/lib/constants/errorMessages.ts`.
- **Auth state:** a Zustand store persisted to `localStorage` (`shared/stores/auth`), read through `useAuth()` for `token`, `userId`, `isAdmin` and `isAuthenticated`.
- **Pages** are lazy-loaded in `src/app/routes.tsx`.
- **i18n:** i18next with `en` and `bg` namespaces in `shared/i18n/locales/<lang>/<namespace>.json`. Add new keys to both languages. A new namespace also has to be imported and registered in `shared/i18n/i18n.ts`.
- **TypeScript is very strict:** `noUncheckedIndexedAccess`, `exactOptionalPropertyTypes` and `verbatimModuleSyntax` are all on. For example, an optional property can't be assigned `undefined` unless its type includes it.
- **Libraries:** forms use Formik + Yup, and the UI uses Bootstrap / react-bootstrap / MDB.

Note: the app is about to have a huge refactoring. Findings during the code review are at docs\review\2026-10-code-review.md
