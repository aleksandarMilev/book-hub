# BookHub code review, October 2026

- **Date:** 2026-10-02
- **Commit reviewed:** `d78e9fc` (master)
- **Scope:** read-only audit of `server/`, `client/`, `sqlserver/`, Docker Compose files, `.github/`, README.md and CLAUDE.md.
- **Method:** static review of the source, plus the build, test, lint, typecheck, audit and `compose config` commands listed in each section. Findings marked **Unverified** could not be confirmed at runtime. The reason is given each time, usually that the Docker daemon wasn't running.

## 1. Executive summary

The backend has a sound structure. Feature slices are consistent, services work through EF Core projections, reads use `AsNoTracking`, list endpoints are paginated, image uploads are validated properly (size, extension, MIME type and magic bytes, with GUID file names), and the full-text search is parameterized. Most of the remaining problems sit at the edges: deployment, bootstrap, email and a handful of authorization checks.

The production stack **cannot work as committed**:

- The production client is compiled against `http://server:8080`, a hostname that only exists inside the Docker network.
- No migration step exists for production.
- No admin user is created, and creating a book or author as a normal user then fails with a 500, because `AdminService.GetId()` throws when there is no admin.
- Registration is coupled to sending the welcome email. If SMTP fails, the account is soft-deleted and that username/email is blocked permanently.

The server tests **don't compile** (10 errors in `BooksUnit.cs`). After patching a scratch copy, 96 of 105 tests pass and 9 fail. 11 of the 15 server features have no tests, and CI runs neither the tests nor the client's lint and typecheck.

The ChatController IDOR from the previous audit is **partially fixed**. Chat details and messages now use the caller's claims, but `access/{userId}` and `invited/{userId}` still take the user ID from the route. Invited users who haven't accepted can read the last 20 messages, invitation accept/reject sends a notification to a client-chosen user, and reading lists ignore profile privacy.

On the client, TypeScript discipline is good: zero `any`, a clean ESLint run, and abort-controller cleanup. However, `tsc` fails with 40 errors, there are no tests, uploads of book, author, article and chat images are very likely broken by a header bug, and the styling layer (Bootstrap ×2, MDB and 63 CSS files) is where most of the weakness is.

**Top 5 issues**

1. **D-01 (Critical):** the production client can't reach the API, because the build bakes in `http://server:8080`.
2. **B-01 + D-04 (Critical/High):** a fresh production database gets no migrations and no admin. Book and author creation by normal users then fails with a 500 *after* the row has been saved.
3. **F-01 (High):** FormData is sent with a forced `Content-Type: application/json`, which axios turns into JSON. Image uploads for books, authors, articles and chats are dropped.
4. **B-02 + B-03 (High):** registration fails, and permanently locks the username/email, whenever the welcome email fails. Production also never sets `AppUrlsSettings__ClientBaseUrl`, so the password-reset links are broken.
5. **T-01 + T-03 + D-05 (High):** the test project doesn't compile, 11 features have no tests (including Chat, ReadingLists and Identity, where this audit found security bugs), and CI wouldn't notice either problem.

**Counts by severity** (76 findings): Critical 2, High 9, Medium 26, Low 39.

## 2. Findings table

ID prefixes: S = security, B = backend, T = tests, F = frontend, D = Docker/CI, DOC = documentation. All paths are relative to the repo root.

### Security

| ID | Sev | Location | Issue | Suggested fix |
|---|---|---|---|---|
| S-01 | Medium | `server/BookHub/Features/Chat/Web/ChatController.cs:56-68` | **The previous ChatController IDOR is only partly fixed.** `Details` and `Messages` now use the caller's ID (`ChatService.cs:32`, `ChatMessageService.cs:554`). `CanAccessChat(id, userId)` and `IsInvited(id, userId)` still take `userId` from the route, so any authenticated user can check whether any user is a member of or invited to any chat. `NotJoined(userId)` is acceptable, because it filters by `CreatorId == caller`. | Remove the `{userId}` route segment and use `User.GetId()` in the controller. Update `client/src/features/chat/api/api.ts:80,96` and `app/routes/guards/chat/hooks/useHasAccess.ts`. |
| S-02 | Medium | `server/BookHub/Features/Chat/Web/Models/ProcessChatInvitationWebModel.cs:8-17`, `Features/Chat/Service/ChatService.cs:285-290, 334-339` | Accepting or rejecting an invitation creates a notification for the `ChatCreatorId` **sent in the request body**, with a `ChatName` that is also in the body. Anyone holding one invitation can send a notification with chosen text to any user ID. `InviteUserToChat` also trusts the `ChatName` from the body (`ChatService.cs:384-388`). | Load the chat by `ChatId` and use `chat.CreatorId` and `chat.Name` from the database. Remove those fields from the web models. |
| S-03 | Medium | `server/BookHub/Features/Chat/Service/ChatService.cs:33-63` | `Details` gates on `CanAccessChat`, which is also true for invited users who haven't accepted, and returns the participants plus the last 20 messages. `GetForChat` correctly requires acceptance (`ChatMessageService.cs:555`). | Use `CanAccessChatAndHasAcceptedInvitation` in `Details`, and give pending invitees a separate minimal "invitation preview" model. |
| S-04 | Medium | `server/BookHub/Features/ReadingLists/Web/ReadingListsController.cs:18-45`, `Features/ReadingLists/Service/ReadingListService.cs:24-79` | `All(userId, …)` and `LastCurrentlyReading(userId)` return any user's reading lists. They ignore `UserProfile.IsPrivate`, which `ProfileService.OtherUser` (`ProfileService.cs:66-69`) does respect. A private profile's lists can be read through the API. | When `userId` isn't the caller's ID and the target profile is private (and the caller isn't an admin), return 404 or 403. |
| S-05 | Low | `server/BookHub/Features/Chat/Web/ChatMessageController.cs:14` | The controller has no `[Authorize]`. Anonymous calls reach the service with `GetId()! == null`, which then rejects them with a 400 rather than a 401. | Add `[Authorize]`. Consider a fallback policy (`options.FallbackPolicy = RequireAuthenticatedUser`) so that anonymous access has to be opted into. |
| S-06 | Medium | `client/src/shared/stores/auth/auth.ts:19-25,34`, `server/BookHub/Features/Identity/Shared/Constants.cs:40-44`, `Features/Identity/Service/IdentityService.cs:296-301` | The JWT is stored in `localStorage` (readable by any XSS) and lasts 7 days, or 30 with "remember me". There is no refresh or revocation, and no security-stamp check, so the token of a deleted or demoted user stays valid until it expires. | Short-lived access token (15 min) with a rotating refresh token in an `HttpOnly; Secure; SameSite` cookie. At minimum, validate the user's existence and `IsDeleted` in `OnTokenValidated`. |
| S-07 | Medium | `server/BookHub/appsettings.Development.json:14`, `Infrastructure/Extensions/ServiceCollectionExtensions.cs:217`, `README.md:91` | The secret's length is never validated. The committed dev secret `dev-secret-change-me` is 20 bytes. **Verified:** with Microsoft.IdentityModel, HS256 token creation then throws `IDX10720 … key size must be greater than 256 bits`, so login and registration fail in local (non-Docker) development. The README recommends "16+ characters", which isn't enough. | Fail fast at startup when `Secret` is shorter than 32 bytes (`ValidateOnStart`). Use a 32+ byte dev secret through user-secrets. Fix the README. |
| S-08 | Medium | `server/BookHub/Program.cs:45-52`, `Infrastructure/Extensions/AppBuilderExtensions.cs:16-32`, `ServiceCollectionExtensions.cs:52-71` | Forwarded headers are misconfigured. `UseForwardedHeaders` runs *after* `UseHsts`/`UseHttpsRedirection`. Only `KnownProxies` is cleared: `KnownIPNetworks` still trusts loopback only, so a reverse proxy on the Docker bridge (`172.x`) isn't trusted. Behind a proxy, the per-IP rate limiter would put every user in one 240 req/min bucket, and the scheme would read as `http`. Checked against the framework defaults, not at runtime. | Call `UseForwardedHeaders` first. Set `KnownIPNetworks` to the Docker network, or clear both lists when only the proxy can reach the API. Remove the unnecessary `async` from that method. |
| S-09 | Medium | `docker-compose.prod.yml:30` | The API connects to SQL Server as `sa`. | Create a least-privilege login (`db_datareader`/`db_datawriter`, plus DDL only for the migration job). |
| S-10 | Low | `server/BookHub/Infrastructure/Extensions/AppBuilderExtensions.cs:85-97,131-132` | Development seeding creates a user with the owner's real email address and the password `123456`, plus `admin@mail.com`/`admin1234`. **Verified:** this only runs when `IsDevelopment()` (`Program.cs:69-76`), so it can't run in production as configured. | Move the seed credentials to configuration, use a placeholder email, and document the built-in user. |
| S-11 | Low | `server/BookHub/Common/Constants.cs:19-25` | Error strings returned to clients contain internal type names and user IDs, for example `User with Id: … can not modify BookDbModel with Id: …`. | Return generic, translatable messages or error codes to the client and keep the detail in the logs. |
| S-12 | Low | `client/nginx.conf:8-28` | The security headers are declared at server level, but the `location` blocks declare their own `add_header`, so nginx doesn't inherit the server-level headers for JS/CSS/images or `index.html`. There is also no CSP. | Move the headers into an include and add it to each location. Add a CSP and HSTS at the TLS proxy. |
| S-13 | Low | `.gitignore:3` | `appsettings.*` is ignored, yet both appsettings files are tracked. Edits get committed silently. A pattern search of git history found only placeholder secrets. That search was regex-based (`Password=`, `Secret`, AWS/OpenAI key formats, private keys), not a full secret scan. | Use `dotnet user-secrets` for local values and keep only empty templates in git. Run `gitleaks` once over the history. |

### Backend

| ID | Sev | Location | Issue | Suggested fix |
|---|---|---|---|---|
| B-01 | **Critical** | `server/BookHub/Areas/Admin/Service/AdminService.cs:12-14`, `Features/Books/Service/BookService.cs:227-234,335-342`, `Features/Authors/Service/AuthorService.cs:168,278`, `Program.cs:69-76` | Production never creates an admin, because `UseProductionAdminRole` is never called. `GetId()` uses `SingleOrDefault` and throws when there are 0 admins *or more than 1*. Non-admin book/author create and edit calls it **after `SaveChangesAsync`**, so the row is saved and the client gets a 500. A second admin would break it the same way. | Add a startup flag that calls `UseProductionAdminRole` (it already exists and is config-driven). Make `GetId` return all admin IDs (notify each) or a configured ID. Send the notification before committing, or put it in the same transaction. |
| B-02 | High | `server/BookHub/Features/Identity/Service/IdentityService.cs:77-115,39-62`, `Features/Identity/Data/Models/UserDbModel.cs:14-16`, `Data/BookHubDbContext.cs:102-112` | `Register` treats a failure to send the welcome email as fatal and rolls back with `userManager.DeleteAsync`. Because `UserDbModel` is an `IDeletableEntity`, that is a **soft delete**. The uniqueness checks use `IgnoreQueryFilters()`, so the username and email stay "taken" for good. In production, one SMTP outage permanently blocks those sign-ups. The same also happens in local development unless an SMTP server is listening on `localhost:1025`. | Send the email after registration succeeds (fire-and-forget through an outbox or background queue) and never fail registration because of it. If a rollback is needed, hard-delete. |
| B-03 | High | `server/BookHub/Features/Identity/Service/IdentityService.cs:92-96,201-207`, `appsettings.json:34-36`, `docker-compose.prod.yml:28-40` | Neither Compose file sets `AppUrlsSettings__ClientBaseUrl`, and production falls back to `""`. `"".TrimEnd('/')` isn't null, so the `?? throw` guard never fires, and reset and welcome links come out as the relative `/identity/reset-password?...`, which is broken in an email. | Bind the settings with `ValidateDataAnnotations().ValidateOnStart()` (`[Required, Url]`) and add the variable to Compose, `.env.example` and the README. |
| B-04 | Medium | `server/BookHub/Program.cs:41-52`, `Infrastructure/Services/ImageWriter/ImageWriter.cs:30` | There is no `UseExceptionHandler` or ProblemDetails outside Development. Unhandled exceptions return a bare 500 with no correlation ID. Examples: `ImageWriter` throws `InvalidOperationException`, unique-index races in check-ins, votes and reading-list adds, and FK violations (B-06). | Add `AddProblemDetails()` with `UseExceptionHandler()`, map `DbUpdateException` unique/FK violations to 409/400, and log with a trace ID. |
| B-05 | Medium | `server/BookHub/Features/ReadingLists/Web/ApiRoutes.cs:5`, `client/src/shared/lib/constants/api.ts:147` | `LastCurrentlyReadingRoute = "/last-currently-reading"` starts with `/`, so it ignores the controller prefix and the endpoint is `GET /last-currently-reading`. The client calls `/readingLists/last-currently-reading`, which returns 404, so the profile's "currently reading" box never loads. | Remove the leading slash. |
| B-06 | Medium | `server/BookHub/Features/Books/Service/BookService.cs:213-221,618-622` | When no genres are given, the book gets the hard-coded genre `52e607d4-…`, which only exists after an admin runs the DataImporter (`genres.json`); there's no `HasData`. Client-supplied genre IDs aren't validated either. Either case produces an FK violation and a 500 on a fresh database. The method also adds to the caller's collection. | Seed the "Other" genre in a migration (`HasData`), filter `genreIds` against existing genres, and copy the list instead of mutating it. |
| B-07 | Low | `server/BookHub/Features/Chat/Web/ChatController.cs:41,88,133`, `Chat/Web/ChatMessageController.cs:31,51`, `Authors/Web/User/AuthorsController.cs:53`, `Reviews/Web/ReviewsController.cs:57`, `ReadingLists/Web/ReadingListsController.cs:38` | These actions return `BadRequest(string)`, a plain text body rather than `{ errorMessage }`. The client's `processError` (`client/src/shared/api/http.ts:45-52`) can't read it and shows the generic fallback message. | Use `NoContentOrBadRequest` or `OkOrBadRequest` everywhere, or switch to ProblemDetails. |
| B-08 | Low | `server/BookHub/Features/Statistics/Data/Queries/AllStatistics/StatisticsQuery.cs:12-20` | The raw SQL counts bypass the query filters, so the home-page statistics include soft-deleted and unapproved rows. | Add `WHERE IsDeleted = 0 AND IsApproved = 1` to the SQL, or use filtered LINQ `CountAsync`. |
| B-09 | Low | `server/BookHub/Features/Reviews/Service/ReviewService.cs:173-216,306-413`, `Features/UserProfile/Service/ProfileService.cs:224-432` | The denormalized counters drift. Rating averages are read-modify-write with no concurrency control. Deleting a review never decrements `ReviewsCount`. Deleting a book never adjusts the author's rating or the profile counters, and no `Decrement*` exists for reviews, books or authors. | Recompute aggregates with SQL (`AVG`/`COUNT`) inside `ExecuteUpdateAsync`, or add a rowversion. Add the missing decrements. |
| B-10 | Low | `server/BookHub/Features/Books/Shared/BookMapping.cs:56,119` | `ThenBy(r => r.CreatedBy == userId)` compares the *username* (`CreatedBy`) with the *user ID*, so it never matches. The intended "my review first" ordering doesn't happen. | Compare `r.CreatorId == userId`, and order by it first. |
| B-11 | Low | `server/BookHub/Data/BookHubDbContext.cs:114-126`, `Features/Reviews/Data/Models/VoteDbModel.cs:8`, `Features/Reviews/Web/VotesController.cs:17-22` | Audit fields are only set for `IDeletableEntity`, so `VoteDbModel` (`Entity<int>`) keeps `CreatedOn = 0001-01-01`. `VotesController` ignores the service's result and always returns 200. | Apply the audit logic to `IEntity`, and return 404 when the review doesn't exist. |
| B-12 | Low | `server/BookHub/Data/BookHubDbContext.cs:129-224` | `dotnet ef` reports warning 10622 three times: `BookGenreDbModel`, `ChatUser` and `VoteDbModel` are required dependents of filtered principals, so rows can disappear unexpectedly from queries. | Add matching filters to the dependents, or make those navigations optional. |
| B-13 | Low | `server/BookHub/Infrastructure/Extensions/ServiceCollectionExtensions.cs:196-201`, `docker-compose.prod.yml:41-43` | The DataProtection key ring isn't persisted. In a container, every restart invalidates pending password-reset tokens. | `AddDataProtection().PersistKeysToFileSystem(...)` on a volume. |
| B-14 | Low | `server/BookHub/Features/Books/Web/User/Models/CreateBookWebModel.cs:33-35`, `Features/Challenges/Service/ReadingChallengeService.cs.cs:158` | `Pages` and `PublishedDate` have no range checks, so negative pages and future dates are accepted. Daily check-ins use the UTC date, so a Bulgarian user's day boundary is 2–3 hours off. The service file names end in `.cs.cs`. | Add `[Range]` and a date validator. Accept the client's local date, or store the user's timezone. Rename the files. |
| B-15 | Low | `server/BookHub/Features/Books/Web/User/BooksController.cs:58`, `Features/Genres/Web/GenresController.cs:20`, `Features/Search/Web/SearchController.cs:27`, `Features/Search/Service/SearchService.cs:33-39` (repeated 6×) | Several `ActionResult<T>` types are wrong (book create is typed as `AuthorDetailsServiceModel`, genre details as `IEnumerable`, genre search as `SearchBookServiceModel`), so Swagger and any generated client types are wrong too. The full-text query-building block is copied 6 times. | Fix the types, and extract a `ToPrefixFullTextQuery(term)` helper. |
| B-16 | Medium | `server/BookHub/BookHub.csproj:12-21` | `dotnet list package --vulnerable` reports a transitive **Microsoft.OpenApi 2.3.0 (High, GHSA-v5pm-xwqc-g5wc)** through Swashbuckle. All Microsoft packages are on 10.0.1, against 10.0.12 available. | Update to Swashbuckle 10.2.3 or pin Microsoft.OpenApi to a fixed version. Bump the 10.0.x packages. Add Dependabot for NuGet. |

### Tests

| ID | Sev | Location | Issue | Suggested fix |
|---|---|---|---|---|
| T-01 | High | `server/BookHub.Tests/Books/BooksUnit.cs:214,307,369,445,634,713,782,907,962,1033` | **The test project doesn't compile:** CS7036 ×10. The `BookService` constructor gained `IPageClamper` and changed parameter order, but the tests weren't updated. As a result `dotnet test` runs nothing. | Update the 10 constructor calls (one shared factory helper). |
| T-02 | Medium | `server/BookHub.Tests/Books/BooksUnit.cs`, `Books/BooksIntegration.cs`, `Authors/AuthorsUnit.cs`, `Authors/AuthorsIntegration.cs` (the `Edit_*` tests) | With T-01 patched in a scratch copy, **9 of 105 tests fail**, all `Edit_*`. They still expect edits to change the entity directly, which no longer matches the pending-edit workflow. Example: `Expected dbModel.Title … "Edited valid title" but found "Seed book …"`. | Rewrite them to assert on `BookEdits`/`AuthorEdits`, and add tests for `Approve` and `Reject`. |
| T-03 | High | `server/BookHub.Tests/` (only `Articles`, `Authors`, `Books`, `Genres`) | **11 of 15 server features have no tests:** Challenges, Chat, DataImporter, Emails, Identity, Notifications, ReadingLists, Reviews, Search, Statistics, UserProfile. `AdminService`, `ImageValidator` and the DbContext query filters aren't covered either. Chat, ReadingLists and Identity are exactly where S-01–S-06 and B-02 are. | Prioritize integration tests for authorization: non-member, invitee and other-user cases for Chat, ReadingLists, Notifications, Reviews and Profile. Then Identity (register, login, lockout, reset). |
| T-04 | Medium | `client/package.json:13`, `client/.husky/pre-push:5` | There are no client test files. `vitest run` exits 1 ("No test files found"), so the pre-push hook (`typecheck && test`) can never pass. It's either being bypassed or isn't installed. | Add a first Vitest + Testing Library + MSW test, or `--passWithNoTests` as a stopgap. |
| T-05 | Low | `server/BookHub.Tests/BookHub.Tests.csproj` | Transitive vulnerabilities: `SQLitePCLRaw.lib.e_sqlite3` 2.1.11 (High) and `System.Security.Cryptography.Xml` 9.0.0 (8 High advisories). `xunit` 2.9.3 is deprecated in favour of `xunit.v3`. | Bump the EF Sqlite/InMemory/Mvc.Testing packages to 10.0.12 and plan a move to xunit.v3. |

### Frontend

| ID | Sev | Location | Issue | Suggested fix |
|---|---|---|---|---|
| F-01 | High | `client/src/shared/api/http.ts:9-15`; callers: `features/book/api/api.ts:79,93`, `features/author/api/api.ts:58,77`, `features/article/api/api.ts:35-39,59`, `features/chat/api/api.ts:110,128` | `getAuthConfig` always sets `Content-Type: application/json`. For a `FormData` body, axios 1.x then runs `JSON.stringify(formDataToJSON(data))` (verified in `node_modules/axios/lib/defaults/index.js:53-56`), and a `File` serializes to `{}`. Image uploads for books, authors, articles and chats are dropped or rejected. Profile and register work because they don't use `getAuthConfig` (`features/profile/api/api.ts:76`, `features/identity/api/api.ts:40`). **Unverified end-to-end**, because the stack wasn't run. | Remove the default `Content-Type` from `getAuthConfig` and let axios set it, or add a `getAuthMultipartConfig`. |
| F-02 | Medium | e.g. `client/src/features/reading-list/api/api.ts:24`, `features/profile/hooks/useCrud.ts:31`, `features/search/api/api.ts:24` | **`npm run typecheck` fails with 40 errors in 14 files.** Most are TS7030/TS2366: `catch { processError(...) }` without `return`, so the inferred return type includes `undefined`. That then causes TS2345 in the hooks. Vite builds anyway, because it doesn't typecheck. | `return processError(...)` everywhere (CLAUDE.md already states this convention), and add `tsc` to CI. |
| F-03 | Medium | `client/src/shared/stores/auth/auth.ts:34`, `client/src/shared/api/http.ts:6-7` | `isAuthenticated = Boolean(username)`. The token's `exp` is never checked, and there's no axios response interceptor. After 7 days the UI looks logged in while every call fails with 401. | Add a response interceptor: on 401, `resetAuth()` and redirect to login. Decode `exp` on load. |
| F-04 | Medium | `client/src/features/identity/components/register/validation/registerSchema.ts:37`; server `Infrastructure/Extensions/ServiceCollectionExtensions.cs:183-189`, `Features/Identity/Shared/Constants.cs:23` | The client's password rule is only `required()`. The server accepts 6–128 characters at the model level, but production Identity requires 8 characters with a digit, a lowercase and an uppercase letter. Users only find out from a server error. | Expose the rules (or mirror them) and validate the same way on both sides. |
| F-05 | Medium | `client/index.html:25-28`, `client/src/main.tsx:1-3`, `client/package.json:29-36,42-44` | Three overlapping CSS frameworks: Bootstrap **5.3.0-alpha1 from a CDN**, Bootstrap 5.3 from npm, and MDB (itself Bootstrap-based). MDB is used in 29 components and react-bootstrap in 3. Icons come from three systems (FontAwesome CSS, FontAwesome SVG and react-icons). | Pick a single system (see section 5). At minimum, remove the CDN link. |
| F-06 | Medium | `client/src/**/*.css` (e.g. `client/src/app/App.css:3`) | **63 CSS files, 10,780 lines, 118 distinct hex colors, 194 `!important`, and only 12 CSS custom properties.** Border radii vary across 10, 12, 14, 16 and 18 px and 999px. Two accent palettes compete (`#9a6bff`/`#ff6ec7` against Bootstrap's `#6ea8fe`). 54 files contain media queries, but each one is ad hoc. | Replace with design tokens (see section 5). |
| F-07 | Medium | `client/package.json` | `npm audit`: 19 vulnerabilities (1 critical, 9 high, 8 moderate, 1 low). Direct dependencies affected: **axios 1.15.2** (high, runtime), **vite 7.3.2** (high, dev server), **vitest 4.0.18** (critical, dev only) and react-router-dom 6.30.3 (moderate, open redirect). | Bump axios to ≥1.20, vite to ≥7.3.6, vitest to ≥4.1.11 and react-router-dom to ≥6.30.6 (or 7.18+). Turn on Dependabot. |
| F-08 | Low | `client/index.html:7-24` | Four Google Fonts requests (Roboto, Merriweather, Edu AU VIC WA NT Pre, Sevillana, Poppins), but the most-used font in the CSS, `'Inter'` (43 rules), is never loaded and falls back to `sans-serif`. | Choose 1–2 fonts and self-host them. |
| F-09 | Low | `client/src/features/author/components/introduction/AuthorIntroduction.tsx:18,33`, `features/book/components/details/full-info/reading-list-buttons/ReadingListButtons.tsx:75`, `shared/components/errors/error-boundary/ErrorBoundary.tsx:46`, `features/chat/components/list-item/ChatListItem.tsx:45,54`, `shared/lib/constants/errorMessages.ts` (whole file), `shared/i18n/locales/{en,bg}/books.json` | Some English strings bypass i18next, including every fallback error. Key mismatches: `books` has `invalidDate` (en) versus `validation.invalidDate` (bg), and `identity.register.optionalField` is missing from en. Otherwise en/bg parity is good: 15 namespaces, about 630 keys. | Move the strings into the namespaces, and add a key-parity check to CI. |
| F-10 | Low | `client/src/app/routes/guards/chat/ChatRoute.tsx:15`, `chat/hooks/useHasAccess.ts:13-23` | The guard renders the protected element while the check is still loading, and `useHasAccess` has no abort or `catch`. | Show a spinner while loading, and add an `AbortController` and `catch`. |
| F-11 | Low | `client/src/shared/lib/constants/api.ts:98-170` | The `routes` constant mixes API paths and SPA paths (e.g. `createBook: '/books/new'`, `admin.*`). One stale API path is `markNotificationRead: '/notifications/markAsRead'`; it isn't used, but it's misleading. | Split it into `apiRoutes` and `appRoutes`. |
| F-12 | Low | e.g. `client/src/main.tsx:1` | `prettier --check` fails on 158 files, mostly because 188 of the 308 files in `src/` start with a UTF-8 BOM and some lack a final newline. ESLint is clean. | Run `npm run format` once, and add `.editorconfig` (`charset = utf-8`). |
| F-13 | Low | `client/package.json:34,51` | `@microsoft/signalr` is installed but never imported. Chat and notifications aren't real-time: there's no polling or socket, and new messages appear only on refetch. | Remove it, or implement a SignalR hub (see open questions). |
| F-14 | Low | `client/src/features/chat/hooks/useCrud.ts` (640 lines), `features/profile/hooks/useCrud.ts` (308) | Fetching, cache state, form state and navigation are mixed into "god hooks". Loading and error handling is repeated by hand in about 15 `useCrud` files. | A query library (see section 5) removes most of this code. |
| F-15 | Low | `client/package.json` | `npm outdated`: React 18.3 (latest 19.3), react-router 6 (7.18), FontAwesome 6 (7), mdb-react-ui-kit 9 (10), TypeScript 5.9 (7.0), Vite 7 (8), eslint 9 (10), eslint-plugin-react-hooks 5 (7). | Handle during the rewrite rather than upgrading in place. |

### Docker, CI and deployment

| ID | Sev | Location | Issue | Suggested fix |
|---|---|---|---|---|
| D-01 | **Critical** | `docker-compose.prod.yml:56-57`, `client/Dockerfile.prod:7-10` | The production SPA is built with `VITE_REACT_APP_SERVER_URL=http://server:8080`. `server` only resolves inside the Docker network, and the browser can't reach it. Over HTTPS it would also be blocked as mixed content. | Serve the API behind the same origin (`/api` proxied by nginx or Caddy) and build with a relative base URL, or pass the public `https://api.<domain>` as a build arg. |
| D-02 | High | `server/BookHub/Dockerfile.prod:6-7,19`, `docker-compose.prod.yml:42` | The app runs as `appuser`, but `COPY --from=build` leaves `/app/wwwroot` owned by root. The named volume `server_uploads` is initialized from that root-owned directory, so `Directory.CreateDirectory` and `FileStream` in `ImageWriter` would fail with `UnauthorizedAccessException`. **Unverified at runtime:** Docker wasn't running. | `COPY --chown=$APP_UID:$APP_UID`, and use the image's built-in `app` user (`USER $APP_UID`) instead of `useradd`. |
| D-03 | High | `docker-compose.prod.yml:25-27,61-62` | No TLS and no reverse proxy. The API ports 8080 and 8081 are published on every interface (nothing listens on 8081), and the client is on plain port 80. | Add Caddy or Traefik (automatic Let's Encrypt) as the only published service on 80/443, and remove the published ports from `server` and `client`. |
| D-04 | High | `server/BookHub/Program.cs:69-76` | Migrations are applied only in Development, and there's no production mechanism, so a fresh production database has no schema. See section 3.4 for the proposal. | EF migration bundle run as a one-shot `migrate` service. |
| D-05 | High | `.github/workflows/build-and-deploy.yml:1-23` | CI only builds, on pushes to `develop`/`master`. It has no `pull_request` trigger, doesn't run tests, typecheck, lint or format, and doesn't pin the .NET or Node versions (no `setup-dotnet` or `setup-node`). It's named "deploy" but builds no images and deploys nothing. Dependabot PRs are being merged without checks. Whether `ubuntu-latest` ships the .NET 10 SDK is **Unverified** (CI logs weren't available). | Run on `pull_request` and `push`, with `actions/setup-dotnet@v5` (10.0.x) and `actions/setup-node` (22/24). Run `dotnet test`, `npm ci`, `lint`, `typecheck`, `test` and `build`. Add an image build and push to GHCR on `master`, and make the checks required in branch protection. |
| D-06 | Medium | `client/Dockerfile.prod:3-6`, `client/Dockerfile.dev:3-5`, `.dockerignore:7` | The client build context is `./client`, which has no `.dockerignore`, so `COPY . .` copies the host's `node_modules` over the `npm ci` result. With a Windows host, that means Windows native binaries for esbuild/rollup in a Linux image. The `chmod -R a+x node_modules/.bin \|\| true` line looks like a workaround for this. **Unverified at runtime.** | Add `client/.dockerignore` (`node_modules`, `dist`, `.env*`) and remove the `chmod` hack. |
| D-07 | Medium | `docker-compose.prod.yml:22-24,46-50,58-60`, `docker-compose.dev.yml:24-25` | The production healthcheck uses `curl`, which isn't in `mcr.microsoft.com/dotnet/aspnet:10.0`, so it would always be unhealthy (**Unverified**, Docker not running). SQL Server has no healthcheck, and `depends_on` has no `condition`, so the API can start before SQL accepts connections. In dev, `MigrateAsync` would then fail on the first run. | Healthcheck SQL with `sqlcmd`/`/opt/mssql-tools18`. Use `condition: service_healthy` and `service_completed_successfully` (migrations). Healthcheck the API with a tiny `--health` mode or a `wget`-free probe. |
| D-08 | Medium | `docker-compose.prod.yml:11,30-40`, `.env.example:1` | Missing variables silently become empty strings: `docker compose config` with no `.env` only warns. The example `SA_PASSWORD=sa-pass` doesn't meet SQL Server's password policy, so the container won't start when the template is copied as-is. | Use `${VAR:?required}`, and put a compliant placeholder in `.env.example`. |
| D-09 | Medium | `client/Dockerfile.dev:1`, `client/Dockerfile.prod:1,12`, `sqlserver/Dockerfile:1` | The client images use **Node 20, which reached end of life in April 2026**. `mssql/server:2022-latest` and `nginx:alpine` are floating tags, so builds aren't reproducible. The .NET images (`sdk:10.0`, `aspnet:10.0`) are current and supported. | Use `node:22-alpine` or `node:24-alpine`, and pin `mssql/server:2022-CU<n>-ubuntu-22.04` and `nginx:1.x-alpine`. |
| D-10 | Medium | `docker-compose.prod.yml:2-17` | No log shipping or retention, no database backups, and no SQL memory cap. SQL Server on Linux needs at least 2 GB of RAM, so it won't fit a 1 GB VPS; 2 GB is tight next to the API and nginx. | Set `MSSQL_MEMORY_LIMIT_MB`, schedule `BACKUP DATABASE` to a mounted volume with off-site copies, and set Docker `logging` options. Consider PostgreSQL (see open questions). |
| D-11 | Low | `sqlserver/Dockerfile:7` | `apt-key add` is deprecated. | Use `gpg --dearmor` into `/usr/share/keyrings` with `signed-by=`. |
| D-12 | Low | `.gitattributes:1`, `.gitignore:4` | `.gitattributes` is **UTF-16LE**, which git can't parse, so its `eol=lf` rules are ignored (husky scripts with CRLF break on Linux). `.gitignore:4` is malformed (`.vscode/# local Azure/ACA…`), and `.vscode/settings.json` is tracked despite being ignored. | Re-save `.gitattributes` as UTF-8 and fix the `.gitignore` line. |
| D-13 | Low | `docker-compose.dev.yml:12-13,43-44,62` | Dev publishes SQL Server's 1433 on every interface with the `.env` password. The bind mount `./server:/src/server` overlays the host's `bin/` and `obj/` (Windows restore output) inside the container. `host.docker.internal` doesn't resolve on Linux without `extra_hosts`. | Bind 1433 to `127.0.0.1`, add anonymous volumes for `bin`/`obj`, and add `extra_hosts: ["host.docker.internal:host-gateway"]`. |
| D-14 | Low | `.dockerignore:7` | The pattern `node_modules` only matches the root, so the server build context (`.`) still uploads `client/node_modules`. That's slow, but nothing is copied into the image. | Use `**/node_modules`, and also `client/` for the server build context. |
| D-15 | Low | `docker-compose.prod.yml:12` | `MSSQL_PID=Express` together with Full-Text Search: whether FTS is supported on the Express edition on Linux is **Unverified** (no running container to test against). | Run a test `CREATE FULLTEXT CATALOG` on an Express container before going live. |

### Documentation drift

| ID | Sev | Location | Claim | Reality |
|---|---|---|---|---|
| DOC-01 | Low | `README.md:123` | "Seed data is loaded from … `Features/*/Data/Seed/*.json`" | No such path exists. Seeding is done manually through the admin endpoints `POST /Administrator/DataImporter/{all,…}/` reading `Features/DataImporter/Data/*.json`. CLAUDE.md:80 already notes this. |
| DOC-02 | Medium | `README.md:91` | `APP_SECRET`: "16+ characters recommended" | HS256 requires at least 32 bytes, or token creation throws IDX10720 (verified). See S-07. |
| DOC-03 | Low | `README.md:111` | "API: `8081` (HTTPS)" | Nothing listens on 8081 in either Compose stack (`ASPNETCORE_URLS=http://+:8080`). Only the `https` launch profile uses it. |
| DOC-04 | Low | `README.md:100,156` | "`CORS_ALLOWED_ORIGINS` … (Production only) … must be set in Production or startup will fail" | It applies to every non-Development environment (Staging too). The check runs inside the CORS options callback, which is evaluated **lazily on the first request**, so the app starts and then every request returns 500 (`ServiceCollectionExtensions.cs:85-117`). |
| DOC-05 | Low | `README.md:87-105` | The environment variable table | `AppUrlsSettings__ClientBaseUrl` (required for emails, B-03) and `BootstrapAdmin__Enabled/Email/Password/Role` are missing. |
| DOC-06 | Low | `README.md:126-131` | "Default Admin (Development Only)" | Accurate for the admin, but it doesn't mention that Development also creates a second user (`mileww.sasho`, password `123456`, `AppBuilderExtensions.cs:74-102`). |
| DOC-07 | Low | `README.md:28,143` | Lists Vitest in the stack, and `npm run test` | There are no client tests, and `npm run test` exits 1. |
| DOC-08 | Low | `README.md:157` | Uploads go to the `server_uploads` volume in production | The mount is correct, but the volume isn't writable by the app user (D-02). |
| DOC-09 | Low | `CLAUDE.md:66` | "the bad-request body is `{ errorMessage }`" | 8 actions return a plain-string body (B-07). |
| DOC-10 | Low | `CLAUDE.md:90` | "Each feature has `<Feature>Unit.cs` and `<Feature>Integration.cs`" | Only 4 of 15 features do (T-03). |
| DOC-11 | Low | `CLAUDE.md:49` | pre-push runs `typecheck` and `test` | Both currently fail (F-02, T-04), so the hook can't pass. |
| DOC-12 | Low | `server/BookHub/Properties/launchSettings.json:6,16` | `launchUrl: "swagger"` | Swagger is served at the root (`RoutePrefix = ""`), so `/swagger` returns 404. |

**README claims that checked out:** migrations are applied automatically in Development only (`Program.cs:69-76`). Swagger UI runs only in Development (`SwaggerGen` is registered in every environment, which is harmless). The default admin is created only in Development. Admin endpoints require the `Administrator` role, because every admin controller inherits `AdminApiController`: Articles, Authors, Books, DataImporter and Profile. CORS origins are required outside Development (lazily, see DOC-04). The health check is at `/health`.

## 3. Detailed sections

### 3.1 Repository map

- `server/BookHub.sln` holds two projects: `BookHub` (net10.0 Web API) and `BookHub.Tests` (xUnit).
- There are 15 feature slices under `server/BookHub/Features`: Articles, Authors, Books, Challenges, Chat, DataImporter, Emails, Genres, Identity, Notifications, ReadingLists, Reviews, Search, Statistics and UserProfile. There's also `Areas/Admin` (`AdminService`, `AdminApiController`), plus `Infrastructure` (DI conventions, filters, ImageWriter/Validator, PageClamper, Result, settings, validation attributes).
- There are 2 migrations (`Init` and `FullTextSearch`, both from 2026-02-21). `dotnet ef migrations has-pending-model-changes` reports **"No changes have been made to the model since the last migration"**.
- `server/BookHub/wwwroot/images` holds **466 tracked image files** (seed images plus some uploads).
- `client/src` has 13 feature folders, `shared/` and `app/`: 95 `.tsx`, 107 `.ts` and 63 `.css` files, 37 lazy-loaded pages and 40 routes.

### 3.2 Backend

**What is solid.** The slices follow one layout throughout. DI is convention-based. The `Result` type keeps controllers thin. Reads use `AsNoTracking` and server-side projections, and I found no N+1 queries. Paginated endpoints are clamped by `PageClamper`, chat history is cursor-paged and capped at 100 (`ChatMessageService.cs:568`), and chat messages have composite indexes (`ChatConfiguration.cs`). Image upload validation is thorough: 2 MB cap, extension and MIME allow-lists, magic-byte sniffing for JPEG, PNG, WebP and AVIF, GUID file names, and paths built server-side (`ImageValidator.cs`, `ImageWriter.cs:148-186`). Full-text search uses `EF.Functions.Contains` with a parameterized, quote-escaped prefix term, so I found no injection path. The one raw SQL query (`StatisticsQuery.cs`) is a constant. Login lockout is configured (`ServiceCollectionExtensions.cs:167-169`), forgot-password doesn't reveal whether an account exists, and the password-reset token lifespan is 2 hours. A global fixed-window rate limiter is in place. The email templates HTML-encode user input (`WelcomeEmailTemplate.cs:11,35`).

**Authorization sweep.** I checked every controller and action:

| Controller | Identity source | Verdict |
|---|---|---|
| Articles (user/admin) | admin role | OK |
| Authors / Books (user) | `CreatorId == caller \|\| admin` in the service | OK, but see B-01 |
| Authors / Books (admin) | admin role, plus a redundant `IsAdmin()` check | OK |
| ReadingChallenges | caller's claims only | OK |
| Chat | Details/Messages use claims. `access/{userId}` and `invited/{userId}` use the route | **S-01, S-02, S-03** |
| ChatMessage | claims, but no `[Authorize]` | **S-05** |
| DataImporter | admin role | OK |
| Genres / Statistics / Search | read-only. Chat search is filtered to the caller's accepted chats | OK |
| Identity | anonymous by design | see B-02, B-03 |
| Notifications | `ReceiverId == caller` for list, delete and mark-read | OK |
| ReadingLists | writes use claims. Reads take `userId` from the query | **S-04** |
| Reviews / Votes | `CreatorId == caller \|\| admin` | OK |
| Profile (user/admin) | `Mine` and `Edit` use claims. `OtherUser` respects `IsPrivate`. Admin delete is role-gated | OK |

**Error-handling strategy.** Services return `Result` for expected failures, which is good. There is no global handler for unexpected ones (B-04). Logging is `ILogger` to the console, with sanitized IDs in some services (`StringSanitizerService`), which is inconsistent. There's no structured logging and no request correlation.

**Entities don't leak into responses.** Every endpoint returns ServiceModels. `DataImporterService` deserializes JSON directly into DbModels, but only behind admin endpoints, which is acceptable.

**Build:** `dotnet build server/BookHub.sln -c Release`

- `BookHub` builds with **0 compiler warnings**.
- `BookHub.Tests` **fails with 10 errors** (CS7036, T-01).
- 22 warnings, all NU1903 vulnerable-package warnings, the same 11 reported twice.

**Packages:**

- `--outdated` for BookHub: every `Microsoft.*` package is on 10.0.1, against 10.0.12 available. Also MailKit 4.16.0 → 4.18.1, Swashbuckle 10.1.0 → 10.2.3 and Containers.Tools.Targets 1.22.1 → 1.23.0.
- `--outdated` for Tests: coverlet 6.0.4 → 10.1.0, NSubstitute 5.3 → 6.2, xunit.runner.visualstudio 3.1.5 → 4.0, Test.Sdk 18.0.1 → 18.10.1, EF/Mvc.Testing 10.0.2 → 10.0.12.
- `--vulnerable --include-transitive`: Microsoft.OpenApi 2.3.0 (High) in both projects. Tests only: SQLitePCLRaw.lib.e_sqlite3 2.1.11 (High) and System.Security.Cryptography.Xml 9.0.0 (8 × High).
- `--deprecated`: xunit 2.9.3 ("Legacy", use xunit.v3).

### 3.3 Server tests

- `dotnet test server/BookHub.sln`: **didn't run**, because the test project doesn't compile (T-01).
- **Patched scratch run.** I copied the tracked `server/` files to the session scratchpad, reordered the 10 `new BookService(...)` arguments there, and ran `dotnet test`. Result: **105 tests, 96 passed, 9 failed, 22 s.**
  - All 9 failures are `Edit_*` tests that predate the pending-edit workflow (T-02): 3 in BooksUnit, 2 in BooksIntegration, 2 in AuthorsUnit and 2 in AuthorsIntegration.
  - The repo itself wasn't modified.
- **Coverage by feature.** The four tested features are Articles, Authors, Books and Genres (unit and integration). The other 11 have none (T-03). The previous audit counted 9 untested features; my count of 11 includes DataImporter and Emails.

### 3.4 Migrations in production: proposal

| Option | How | Pros | Cons |
|---|---|---|---|
| **A. Migration bundle in a one-shot service (recommended)** | Add a build stage: `dotnet ef migrations bundle -r linux-x64 --self-contained -o /out/efbundle`. Run a Compose service `migrate` with `depends_on: sqlserver: {condition: service_healthy}`, and give `server` the dependency `migrate: {condition: service_completed_successfully}`. | Explicit, idempotent and auditable. The migration identity can have DDL rights while the API's login doesn't (S-09). Works for the `suppressTransaction` FTS migration. | One more image stage and service. |
| B. Idempotent SQL script | `dotnet ef migrations script --idempotent` in CI, applied with `sqlcmd` | Reviewable SQL, DBA-friendly | A manual step unless it's scripted. Needs `sqlcmd` in some image. |
| C. Startup flag | `if (config["Database:MigrateOnStartup"] == "true") await app.UseMigrations();` | Simplest, 3 lines | Races if you ever run more than one replica. The API needs DDL rights, and a failed migration takes the API down with it. |

For a single-VPS portfolio deployment, **A** is the clean choice. **C** is acceptable if you keep a single replica and want minimal moving parts. Bundle the admin bootstrap (B-01) into the same one-shot job, or into the same startup flag.

### 3.5 Frontend

**Commands**, run from `client/` after `npm ci`. I had to run `npm ci` because `node_modules` was absent; it ran with `HUSKY=0` so `.git/config` stayed untouched.

| Command | Result |
|---|---|
| `npm run lint` | **Pass, 0 problems** |
| `npm run typecheck` | **Fail: 40 errors in 14 files** (F-02) |
| `npx vitest run` (`npm run test` in CI mode) | **Fail: "No test files found", exit 1** (T-04) |
| `npm run build` | **Pass** in 15.9 s. Largest chunks: `index-*.js` 468.6 kB (156 kB gzip), `index-*.js` 156.6 kB, `ProfileDetails-*.js` 79.8 kB |
| `npm run format:check` | **Fail: 158 files** (F-12) |
| `npm audit` | 19 vulnerabilities: 1 critical (vitest), 9 high (axios, vite, brace-expansion, browserslist, form-data, js-yaml, nanoid, postcss, ws), 8 moderate, 1 low (F-07) |
| `npm outdated` | 39 packages behind. Majors: React 19, RR 7, TS 7, Vite 8, ESLint 10, FA 7, MDB 10 (F-15) |

**Structure.** The feature-folder layout (`api/`, `hooks/`, `components/`, `types/`) is consistent and the `@/` alias is used throughout. Separation is reasonable: API calls live in `api.ts` and state in hooks, but the hooks are oversized (F-14). Components mostly render, with some leaked logic, for example `ProfileDetails.tsx` destructures 17 values from one hook.

**State and data fetching.**

- **Zustand:** a small, sensible store. The persisted auth object holds the JWT in `localStorage` (S-06).
- **Axios:** two instances, no interceptors, no 401 handling (F-03), and the Content-Type bug (F-01).
- **Fetch logic:** duplicated across about 15 `useCrud.ts` files. Each one re-implements loading, error and abort handling by hand. Abort controllers *are* cleaned up in the effects I sampled (e.g. `features/chat/hooks/useCrud.ts:89-104,299-303,341-345`).

**Forms.** All 9 forms use `useFormik` + Yup consistently, with 929 lines of schemas. Book constraints match the server exactly (`bookSchema.ts:7-9` against `Books/Shared/Constants.cs`). The password rules don't match (F-04).

**TypeScript.** It's very strict (`exactOptionalPropertyTypes`, `noUncheckedIndexedAccess`). There's **zero `any`**, only 29 `as` casts, and one `eslint-disable` for exhaustive-deps (`useLogoutEffect.ts`). The 612 lines of types under `features/*/types` are hand-copied from the backend DTOs and can drift (e.g. B-15).

**React correctness.** There are 74 `useEffect`s. In the sample I read, the dependencies are correct and the cleanup is present. There's no polling and there are no sockets, so nothing leaks, but chat isn't real-time (F-13). The ChatRoute guard flashes the protected content (F-10).

**Routing.** `AuthenticatedRoute`, `AdminRoute` and `ChatRoute` are client-side only. They're fine as UX, and the server enforces everything apart from the gaps listed under Security.

**i18n.** Good overall: 15 namespaces in `en`/`bg` with near-perfect key parity. The small gaps are listed in F-09.

**Styling.** See F-05 to F-08:

- three CSS frameworks;
- 63 CSS files, 10,780 lines, 118 colors and 194 `!important`;
- the main font is never loaded.

**Accessibility basics:**

- All 28 `<img>` tags have `alt`. Some values are generic ("Book", "Profile").
- 46 `htmlFor` labels and 52 `aria-*` attributes.
- 6 clickable `<div>`/`<span>`/`<li>` elements without keyboard handling.
- Contrast **Unverified**: no browser audit was run.

### 3.6 Docker, CI and deployment

`docker compose -f docker-compose.dev.yml --env-file .env.example config --quiet` and the same command for production both exit **0**, so the syntax is valid. Without a `.env`, Compose warns and substitutes blanks (D-08). The containers weren't started, because the Docker Desktop daemon wasn't running.

**Images:**

- The server images use current, supported `sdk:10.0`/`aspnet:10.0` in a multi-stage build. Layer caching is right: `.csproj` restore first, then source. They run as non-root, but see D-02.
- The client image is multi-stage, from Node 20 (EOL) to `nginx:alpine`, with an SPA fallback and gzip. The API URL is baked in at build time and is wrong for production (D-01). Host `node_modules` leaks into the build (D-06).
- The SQL Server image is `2022-latest` with `mssql-server-fts`, installed through the deprecated `apt-key` (D-11).

**Compose, development against production:**

| Aspect | Development | Production |
|---|---|---|
| SQL 1433 published | yes, all interfaces | **no** (good) |
| DB volume | `sql_data_dev` | `sql_data` |
| Uploads volume | bind mount `./server/BookHub/wwwroot` | `server_uploads` (not writable, D-02) |
| Healthchecks | API only (curl, available in the SDK image) | API only (curl missing from the image, D-07) |
| `depends_on` conditions | none | none |
| Restart policy | SQL `always`, others none | SQL `always`, others none |
| TLS / proxy | n/a | **none** (D-03) |

**Can production be deployed publicly as committed? No.** These are missing: a reachable API URL (D-01), TLS and a reverse proxy (D-03), migrations (D-04), an admin bootstrap (B-01), a writable uploads volume (D-02), a configured `ClientBaseUrl` and working SMTP (B-02, B-03), domain-specific CORS (`Cors__AllowedOrigins=https://<domain>`), persisted DataProtection keys (B-13), log retention, database backups and a SQL memory cap (D-10). A VPS needs **at least 2 GB of RAM for SQL Server alone, with 4 GB recommended**.

## 4. Documentation drift

See the DOC-01 to DOC-12 table in section 2, and the list of README claims that checked out under it.

## 5. Frontend: refactor or rewrite

### Verdict: **REWRITE the UI layer and keep the data layer.**

**Why.** The findings fall into two very different groups.

- **The plumbing is decent and worth keeping.** It has zero `any`, ESLint is clean, strict TypeScript passes apart from one repeated `return` omission, abort controllers are handled, the feature layout is consistent, the i18n is complete in two languages, and the Yup schemas match the server.
- **The presentation layer is the weak part, and it would have to be rewritten during a refactor anyway.** It stacks three Bootstrap-family frameworks (one of them an alpha build from a CDN), 63 ad-hoc CSS files with 118 colors and 194 `!important`, MDB in 29 components, three icon systems, and a font that is never loaded. Removing MDB and Bootstrap means touching nearly every one of the 74 feature components, and normalizing the CSS means rewriting most of the 10,780 lines. At that point a "refactor" *is* a rewrite, minus the design system you'd end up with.
- Fat hooks (F-14) and the missing loading/error/401 handling (F-03) are best fixed with a query library, which replaces the `useCrud` hooks rather than patching them.

**Effort**, solo and part-time, rough estimates:

| Option | Work | Estimate |
|---|---|---|
| Refactor in place | Fix F-01/F-02/F-03/F-04 (2–3 days). Remove the CDN Bootstrap and decide between MDB and react-bootstrap (1 week). Consolidate 63 CSS files into tokens and fix responsive layout (2–3 weeks). Add tests (1 week). | **~60–90 h**. The result still looks like Bootstrap/MDB and still carries the hook sprawl. |
| Rewrite the UI, keep the data layer | Scaffold and design tokens (1 week), rebuild 37 pages on a component library (3–5 weeks), tests (1 week) | **~100–150 h**. The result is a consistent design system, accessible primitives, and much less code. |

The rewrite costs roughly 1.5–2× the refactor, but only the rewrite produces something that looks portfolio-grade. If time is the constraint, apply the four Phase 1 client fixes (F-01 to F-04) to the existing client so the app works at redeploy, and then do the rewrite.

### Proposed stack

| Concern | Choice | Reason |
|---|---|---|
| Styling | **Tailwind CSS v4** with CSS-variable design tokens | One token source replaces 63 CSS files and both Bootstrap copies. Responsive and dark mode come built in. |
| Components | **shadcn/ui** (Radix primitives) | Accessible primitives (focus, ARIA, keyboard) in your own code, with no runtime UI dependency to fight. |
| Icons | **lucide-react** | One tree-shakable icon set instead of three. |
| Data fetching | **TanStack Query v5** | Caching, loading/error states, retries, invalidation and abort, replacing about 15 hand-written `useCrud` hooks. |
| Forms | **React Hook Form + Zod** (`@hookform/resolvers`) | Fewer re-renders than Formik, and Zod types replace the duplicated form types. The 929 lines of Yup port almost line for line. Keeping Yup with the resolver is a cheaper option. |
| API types | **openapi-typescript** generated from Swagger | Removes the 612 lines of hand-copied DTO types (after fixing B-15). |
| Routing | **React Router 7** | A small step from v6, and it fixes the audit's open-redirect advisory. |
| Framework | **React 19** with current Vite | Supported versions, and React Compiler-ready. |
| Auth | Zustand (keep), with the token in memory and a refresh cookie | Pairs with S-06. Add a 401 interceptor in the meantime. |
| Tests | **Vitest + Testing Library + MSW** | Already partly installed. MSW lets the API layer be tested without the server. |

**What to keep:**

- `shared/i18n/locales/**` (2,217 lines; en/bg parity)
- the `i18n.ts` setup
- `features/*/api/api.ts` (1,337 lines), with F-01 fixed; wrap each call in a query hook
- the Yup schemas, as the source for the Zod port
- the Zustand auth store
- `shared/lib/utils` and the `routes` constants, split per F-11
- the `features/*/types` files, until openapi-typescript replaces them
- the ESLint and Prettier configuration
- the strict `tsconfig.json`

## 6. Proposed roadmap

**Phase 1: make it correct and safe** (about 1–2 weeks)

- **Findings:** B-01, B-02, B-03, B-05, B-06, S-01, S-02, S-03, S-04, S-05, S-07, F-01, F-02, F-03, F-04, B-16, F-07.
- **Done when:**
  - a new user can register with SMTP down;
  - a non-admin can create a book with an image on a freshly migrated database, and the admin is notified;
  - `GET /chat/{id}/access/{otherUser}` no longer exists;
  - a private profile's reading list returns 403/404 to other users;
  - `npm run typecheck` exits 0;
  - `dotnet list package --vulnerable` and `npm audit --omit=dev` show no High or Critical issues.

**Phase 2: backend tests and the CI gate** (about 1–2 weeks)

- **Findings:** T-01, T-02, T-03, T-04, T-05, D-05, B-04, B-07, B-12.
- **Done when:**
  - `dotnet test` passes in CI on every PR;
  - integration tests cover the authorization matrix in section 3.2 (one negative test per row) and the Identity flows;
  - the client has at least smoke tests;
  - branch protection requires the CI checks.

**Phase 3: Docker, deployment and redeploy** (about 1 week)

- **Findings:** D-01, D-02, D-03, D-04, D-06, D-07, D-08, D-09, D-10, D-11, D-12, D-13, D-14, D-15, S-08, S-09, S-12, S-13, B-13, DOC-01 to DOC-12.
- **Done when:**
  - `docker compose -f docker-compose.prod.yml up` on a clean VPS produces `https://<domain>` with a valid certificate;
  - migrations run through the bundle service;
  - an admin is bootstrapped from configuration;
  - an image upload survives a container restart;
  - SQL Server isn't reachable from the internet;
  - a nightly backup file exists;
  - the README matches reality.

**Phase 4: client rewrite** (about 4–7 weeks)

- **Findings:** F-05, F-06, F-08, F-09, F-10, F-11, F-12, F-13, F-14, F-15, plus S-06 (token handling).
- **Done when:**
  - every current route has been rebuilt on the new stack;
  - Lighthouse scores at least 90 for accessibility and best practices on the main pages;
  - the layout works at 360 px;
  - there's no Bootstrap or MDB in `package.json`;
  - the client's CI runs lint, typecheck, test and build.

**Phase 5: polish** (ongoing)

- **Findings:** B-08, B-09, B-10, B-11, B-14, B-15, S-10, S-11.
- **Done when:** the statistics and counters match `COUNT(*)` queries, and Swagger types match the actual responses.

## 7. Open questions for you

1. **Hosting target.** A single VPS (Hetzner, DigitalOcean) or a PaaS? `.gitignore:4-6` mentions "Azure/ACA patch artifacts". Are you planning Azure Container Apps? The answer changes D-03 and D-10 and the migration approach.
2. **Domain.** Which one, and should the API be same-origin (`/api`, recommended) or on a separate subdomain? This decides the D-01 fix and the CORS value.
3. **Database.** Keep SQL Server (2–4 GB RAM, Express limits, FTS image maintenance) or move to PostgreSQL (`tsvector` search, much lighter)? This is the biggest cost lever for a small VPS.
4. **Email.** Which SMTP provider (Resend, Postmark, SES)? Should registration send a welcome email at all, or only password resets?
5. **Feature scope.** Chat isn't real-time and carries most of the authorization bugs. Should you implement SignalR, keep it as is, or drop it? Same question for reading challenges and articles (admin-only authoring).
6. **Admin bootstrap.** Is a config-driven `BootstrapAdmin` on first start acceptable, or do you want a CLI or one-shot job? Should there be exactly one admin, or several (B-01)?
7. **Demo data for reviewers.** Should production be pre-seeded through the DataImporter, with a public read-only demo account?
8. **Approval workflow for admins.** Today an admin's *edit* also goes to the pending queue and needs approving (`BookService.cs:246-350`). Is that intended?
9. **Languages.** Keep Bulgarian as a first-class language in the rewrite?
10. **Account deletion.** Accounts are soft-deleted and keep their email and username forever. Do you want a hard-delete or anonymize path (GDPR) for a public deployment?

---

*Side effects of this audit, none of them tracked by git:*

- `npm ci` created `client/node_modules`;
- `npm run build` created `client/dist`;
- `dotnet build` created `server/**/bin` and `server/**/obj`;
- the patched test run and the JWT key-size check ran in a scratch directory outside the repo.

No tracked file was modified.

## Status updates

- **2026-10-02:** S-01, S-02, S-03, S-05, F-10 and F-13 are resolved by removing the Chat feature (Phase 0). The old code is preserved under the git tag `chat-before-removal`, and a rebuild is tracked in `docs/backlog.md`. Chat is no longer part of F-01 (the affected uploads are now books, authors and articles) or F-14 (the remaining god hook is `features/profile/hooks/useCrud.ts`). The chat references in B-07, B-12, F-09 and T-03 no longer apply either, but those findings stay open for their non-chat parts.
- **2026-10-02 (Phase 1a, backend correctness):**
  - **T-01:** fixed. `BooksUnit` builds `BookService` through one shared `NewBooksService` helper with the current constructor. Baseline after the fix: 105 tests, 96 passed, 9 failed (all `Edit_*`).
  - **T-02:** fixed for the `Edit_*` tests. The 9 failing tests are rewritten: non-admin callers assert the pending `BookEdits`/`AuthorEdits` row, and admin callers assert direct edits. Broader Approve/Reject coverage stays in Phase 2.
  - **B-01:** fixed. `UseProductionAdminRole` runs on startup outside Development when `BootstrapAdmin:Enabled` is true, with `BootstrapAdminSettings` validated on start. `IAdminService.GetIds()` returns every admin (a warning is logged and notifications are skipped when there are none). Admin notifications for book/author create and non-admin edit are staged before the single `SaveChangesAsync`, so the entity and its notifications commit together.
  - **B-02:** fixed. Registration no longer rolls back on an email failure. The welcome email goes through a bounded in-memory channel and `WelcomeEmailBackgroundService`, which logs failures with the user ID only. Restart durability (outbox) is in `docs/backlog.md`.
  - **B-03:** fixed. `AppUrlsSettings.ClientBaseUrl` is `[Required, Url]` and validated on start. `AppUrlsSettings__ClientBaseUrl` (from `CLIENT_BASE_URL`) is in both Compose files, `.env.example` and the README.
  - **B-05:** fixed. The route is now `GET /ReadingLists/last-currently-reading`, matching the client.
  - **B-06:** validation part fixed. Unknown genre IDs return a 400 `{ errorMessage }`. "Other" is attached only when it exists, and the caller's list is no longer mutated. Seeding "Other" is deferred to the Postgres migration (`docs/backlog.md`).
  - **S-04:** fixed. Other users' reading lists return 404 when the profile is private or missing, unless the caller is an admin.
  - **S-07:** fixed. `JwtSettings:Secret` must be at least 32 UTF-8 bytes (validated on start), keys are derived with UTF-8, and the committed dev secret is a 32+ byte placeholder. Moving it to user-secrets is still S-13.
  - **DOC-02, DOC-05:** fixed. The README says "32+ bytes" and lists `CLIENT_BASE_URL` and the `BOOTSTRAP_ADMIN_*` variables.
  - **B-17 (new, Low):** fixed. `NotificationService.CreateOnAuthorRejected` used `ResourceType.Book` and now uses `ResourceType.Author`.
  - **Open question 8:** resolved. Admin edits to books and authors apply directly, and only non-admin edits go to the pending queue.
- **2026-10-02 (Phase 1b, minimal client fixes):**
  - **F-01:** fixed. `getAuthConfig` and `getPublicConfig` no longer set `Content-Type`, so axios sends `FormData` as multipart (with boundary) and plain objects as JSON. Every `getAuthConfig` caller was checked: book, author and article create/edit send `FormData`, which the server binds from multipart (13/13 multipart create integration tests pass). Reviews, votes, reading lists and challenges send plain objects, and the rest have no body. Profile edit and register already built their own config and are unchanged.
  - **F-02:** fixed. All 32 `catch` blocks now `return processError(...)` (it returns `never`), which also clears the hook errors. `npm run typecheck`: 40 errors → 0.
  - **F-03:** fixed. `onSessionExpired` in `shared/api/http.ts` adds a response interceptor to `http` and `httpAdmin`. On a 401 for a request that carried a bearer token (identity endpoints excluded; a failed login returns 400 anyway), `app/session.ts` calls `resetAuth()` and navigates to the login page through the exported data router. The login page has no return-URL support, so none is passed. On load, an expired or malformed persisted token is cleared (`shared/lib/utils/jwt.ts`, using the existing `jwt-decode` dependency).
  - **F-04:** fixed on the client. `features/identity/validation/passwordSchema.ts` mirrors production Identity: 8–128 characters, a digit, a lowercase and an uppercase letter, no symbol required. It uses Unicode classes so Cyrillic letters count as on the server. Register uses it in its Yup schema. Reset password had no schema (hand-written checks in `ResetPassword.tsx`), so it now runs the same schema in its submit handler. Messages are under `identity:password.validation.*` in en and bg. Still open on the server: the web models allow 6–128 (`Identity/Shared/Constants.cs:23`) while production Identity requires 8, and Development Identity requires only 6 characters with no character classes, so the client is stricter than a dev server.
  - **T-04:** partly fixed. There are 3 Vitest files (36 tests) next to the code they cover: `getAuthConfig` (including a FormData-through-axios regression test), the 401 interceptor (through axios's `adapter` option, with no new library), the JWT expiry helper and the password schema. `npx vitest run` passes, so the pre-push hook can pass. Testing Library/MSW component tests and CI stay in Phase 2.
  - **DOC-07, DOC-11:** resolved by the above (client tests exist, and pre-push `typecheck` and `test` both pass). CLAUDE.md's "There are currently no client test files" is now out of date.
  - **F-16 (new, Low):** `features/search/api/api.ts` calls the global `axios.get` with an absolute URL instead of `http`, so authenticated search requests bypass the 401 interceptor.
  - **F-17 (new, Low):** `features/genre/hooks/useCrud.ts:81` still has `setGenre(data ?? [])`, which would put an array into `GenreDetails | null` state. It's unreachable now that the API function can't return `undefined`. Fix it in the Phase 4 rewrite.
  - **F-18 (new, Low):** `features/notification/api/api.ts` `remove` swallows every non-cancel error and returns `false` instead of using `processError`, so the caller can't show the server's message.
- **2026-10-02 (Phase 1c, dependency and image updates):** patch and minor updates only. Phase 1's exit criterion holds: `dotnet list package --vulnerable` and `npm audit --omit=dev` show no High or Critical advisories.
  - **B-16:** fixed.
    - Every `Microsoft.*` 10.0.x package in `BookHub.csproj` went from 10.0.1 to 10.0.12.
    - Swashbuckle went from 10.1.0 to 10.2.3. It now resolves Microsoft.OpenApi 2.7.5, the patched version for GHSA-v5pm-xwqc-g5wc, so no pin was needed.
    - MailKit went from 4.16.0 to 4.18.1, and Containers.Tools.Targets from 1.22.1 to 1.23.0.
    - `has-pending-model-changes` still reports no changes.
  - **T-05:** advisories fixed.
    - Mvc.Testing, EF InMemory and EF Sqlite went from 10.0.2 to 10.0.12, and Test.Sdk from 18.0.1 to 18.10.1.
    - SQLitePCLRaw.lib.e_sqlite3 now resolves 2.1.12.
    - System.Security.Cryptography.Xml 9.0.0 came from BookHub's EF Tools → EF Design 10.0.1 → Microsoft.Build.Tasks.Core. It is no longer in the graph, because EF Design 10.0.12 uses Roslyn Workspaces.MSBuild 5.0.0. No direct pin was needed.
    - The xunit.v3 move and the other test-framework majors are deferred to Phase 2 (`docs/backlog.md`).
  - **NuGet advisories:** 11 High rows (OpenApi ×2, SQLitePCLRaw ×1, Crypto.Xml ×8) → 0. `dotnet build server/BookHub.sln` warnings: 22 NU1903 → 0. Tests: 131/131 before and after.
  - **F-07:** High and Critical advisories fixed.
    - axios went from 1.15.2 to 1.20.0, vite from 7.3.2 to 7.3.6, vitest from 4.0.18 to 4.1.11, and react-router-dom from 6.30.3 to 6.30.6.
    - `npm audit fix` (without `--force`) then updated the transitive packages.
    - `npm audit`: 19 (1 critical, 9 high, 8 moderate, 1 low) → 2 moderate.
    - `npm audit --omit=dev`: 5 (2 high, 3 moderate) → 2 moderate.
  - **F-07 still open (Moderate):** react-router `>=6.0.0 <7.18.0` has two advisories that no 6.x release fixes: the backslash open redirect in `<Link>`/`useNavigate` (GHSA-wrjc-x8rr-h8h6), and SSR `deserializeErrors` (GHSA-337j-9hxr-rhxg), which doesn't apply to this SPA. The fix is React Router 7.18+, in the Phase 4 rewrite.
  - **D-09:** fixed for the client. Both client Dockerfiles use `node:22-alpine`, the production image uses `nginx:1.30-alpine` (current stable line, 1.30.5), and `package.json` `engines.node` is `>=22`. Not verified with `docker build`, because the Docker daemon wasn't running. The `mssql/server:2022-latest` pin is superseded by the PostgreSQL migration (Phase 1.5).
  - **Dependabot** is configured in `.github/dependabot.yml`:
    - nuget, npm, docker (`client/`, `server/BookHub/`) and github-actions;
    - weekly, with minor and patch updates grouped per ecosystem;
    - `sqlserver/` is excluded.

    Its PRs must not be merged until the Phase 2 CI gate (D-05) runs tests on pull requests.
  - **Still deferred:**
    - Client majors go to Phase 4 (F-15).
    - FluentAssertions 8.8.0 → 8.11.0 and the other client minors were out of this phase's scope, so Dependabot will pick them up.
- **2026-10-02 (Phase 1.5, SQL Server → PostgreSQL):**
  - **Database:** PostgreSQL 18 (`postgres:18-alpine`) through `Npgsql.EntityFrameworkCore.PostgreSQL` 10.0.3, with `EnableRetryOnFailure`.
    - The SQL Server migrations, including `RemoveChat`, are replaced by one `InitialPostgres` migration. No data was migrated.
    - The database is initialized with the ICU root collation (`--locale-provider=icu --icu-locale=und`), for linguistic ordering and Cyrillic case folding.
    - Timestamps are `timestamptz` (UTC). Date-only values (`DateOfBirth`, `PublishedDate`, `BornAt`, `DiedAt`) are `date` and stay `DateTime` in C#.
    - **API note:** JSON timestamps now end in `Z` (they were unspecified-kind before, which the client read as local time). Date-only values serialize exactly as before.
  - **D-11, D-15:** obsolete. The custom SQL Server FTS image (`sqlserver/`) and the Express-edition question are gone.
  - **D-10:** the RAM concern is largely resolved: PostgreSQL idles at tens of MB instead of SQL Server's 2 GB minimum. Backups and log retention are still open (Phase 3).
  - **B-06:** fixed. The "Other" genre is seeded by the migration (`HasData`, same ID), so books created without genres get it on a fresh database (verified end to end). Importing `genres.json` skips it as an existing row.
  - **B-08:** fixed. Statistics is one LINQ query with explicit `!IsDeleted` / `IsApproved` filters instead of raw SQL. It ignores the global filters, because the 30 s cache is shared by every caller and an admin's filters include unapproved rows.
  - **B-15:** the search part is fixed. One helper (`Infrastructure/Extensions/FullTextSearchExtensions`) replaces the 5 copies of the query-building code. It covers the tsvector column configuration, a safe prefix tsquery (`term:* & term:*`, splitting on any non-letter/digit, always sent as a parameter), and the blank/operator-only rules. The wrong `ActionResult<T>` types are still open.
  - **Search behavior change:** SQL Server matched a *phrase* prefix with language word breakers and stopwords. PostgreSQL ANDs every prefix term across all of a table's indexed columns ('simple' config, no stemming or stopwords). For example, "stephen king" now matches FirstName + LastName. Blank input still returns everything. Input with no letters or digits returns an empty page (it was a SQL error risk before).
  - **B-11:** the audit half is fixed. Created/Modified fields are set for every `IEntity`, so `Votes.CreatedOn` is no longer `0001-01-01`. `VotesController` still ignores the result and returns 200.
  - **D-07 / D-13 / D-08:** partly fixed.
    - D-07: the database has a `pg_isready` healthcheck, and the API waits for it (`condition: service_healthy`) in both stacks.
    - D-13: dev publishes 5432 on `127.0.0.1` only. The bind-mounted `bin`/`obj` and `host.docker.internal` issues remain.
    - D-08: `.env.example` has working placeholder credentials. The `${VAR:?}` guards and the API's `curl` healthcheck in prod are still open.
  - **T-05:** the SQLite and InMemory test packages are removed. Tests run on PostgreSQL through Testcontainers 4.15 (one container per run, a database per test cloned from a migrated template). Docker must be running for `dotnet test`. Two ported tests needed setup changes, with unchanged assertions:
    - `GenresUnit` seeds the user its books reference, because PostgreSQL enforces the foreign key.
    - `BooksUnit…AndOtherGenreDoesNotExist` deletes the migration-seeded "Other" genre first.
  - **New tests:** 42 (131 → 173, all passing; full run about 50 s, against about 22 s on SQLite):
    - `SearchIntegration`: English and Bulgarian prefixes, case-insensitivity, multiple terms, every endpoint, pagination, operator-only and blank input, SQL/tsquery syntax as plain text;
    - `StatisticsIntegration`: soft-deleted and unapproved rows excluded, including when an admin asks first;
    - `DataImporterIntegration`: import all, the "Other" skip, and Bulgarian author search;
    - `FullTextSearchUnit`: the tsquery builder.
  - **DOC-01:** fixed (the README's seeding section).
  - **S-09:** still open (Phase 3). The API connects as `POSTGRES_USER`; a least-privilege role and a migration-only role come with D-04.
  - **B-18 (new, Low):** the DataImporter's `CreatedOn` values (articles.json) are overwritten by `ApplyAuditInfo` on insert, so every imported article shows the import time. This predates the migration (verified on the dev stack).
  - **B-19 (new, Low):** the Profiles search isn't `AsNoTracking`, and it returns private profiles (flagged with `IsPrivate`, which the client has to honour). This is unchanged by this phase.
- **2026-10-02 (Phase 2a, test framework upgrades and the CI gate):** no production code changed. Tests: 173/173 before and after (about 45 s either way).
  - **D-05:** fixed, except the image build and push (Phase 3). `.github/workflows/ci.yml` replaces `build-and-deploy.yml`.
    - **Triggers:** pull requests to `master`/`develop`, and pushes to them. Runs are cancelled when a newer one starts on the same ref. Permissions: `contents: read`.
    - **server job:** checkout, `setup-dotnet` from `global.json`, a NuGet package cache, `dotnet restore`, `dotnet build --no-restore -c Release -warnaserror`, then `dotnet test --no-build -c Release` against Testcontainers PostgreSQL, using the runner's Docker. It uploads a TRX artifact (`server-test-results`) on success and on failure.
    - **client job:** Node 22 with an npm cache keyed on `client/package-lock.json`, `npm ci`, lint, typecheck, `npx vitest run`, build. `format:check` is left out until F-12 is fixed (Phase 4).
    - **`global.json`:** pins the SDK to the 10.0.4xx band (`10.0.400`, `rollForward: latestFeature`), so the D-05 question about which SDK `ubuntu-latest` ships no longer matters.
    - **Side effect of `-warnaserror`:** a newly published NuGet advisory is reported as a build warning (NU190x), so it turns CI red even without a code change. That's intended: it is the gate.
    - **Still to do:** branch protection on `master` (and `develop`) requiring the `server` and `client` checks. This is a GitHub setting, and it's part of Phase 2's exit criteria.
  - **T-05:** fully resolved.
    - `xunit` 2.9.3 → `xunit.v3.mtp-off` 4.0.1, `xunit.runner.visualstudio` 3.1.5 → 4.0.0, `NSubstitute` 5.3.0 → 6.2.0, `coverlet.collector` 6.0.4 → 10.1.0 and `FluentAssertions` 8.8.0 → 8.11.0.
    - `dotnet list package --vulnerable --include-transitive` and `--deprecated` both report nothing.
    - Tests still run through VSTest, so `dotnet test` is unchanged. The plain `xunit.v3` 4.x package enables Microsoft Testing Platform v2, and its MSBuild targets fail `dotnet test` in VSTest mode on the .NET 10 SDK ("Testing with VSTest target is no longer supported…"). The `mtp-off` variant is the same framework with MTP turned off. Moving to MTP (a `test.runner` entry in `global.json`) is a separate decision.
    - **Migration changes:**
      - the test project is `OutputType Exe`;
      - `IAsyncLifetime` members return `ValueTask` (10 integration classes);
      - `CollectionBehavior(DisableTestParallelization = true)` is un-callable in xunit.v3 4.x, so it became `[assembly: Parallelization(Mode = ParallelMode.None)]`. The runner confirms "parallel mode = none".
      - No assertions changed.
    - **No assembly fixture:** xunit.v3 assembly fixtures would mean injecting the fixture into every class and threading it into the static `CreateTestDb` helpers, so the lazy static `PostgresServer` stays.
  - **T-06 (new, Low):** xunit.v3's analyzer rule xUnit1051 (pass `TestContext.Current.CancellationToken` to calls that take a token) has about 300 hits. It's suppressed in `BookHub.Tests.csproj` so the build stays at 0 warnings. Adopt it during the Phase 2b/2c test work and remove the `NoWarn` (`docs/backlog.md`).
  - **T-07 (new, Low):** the `AdminServiceIntegration` header comment still says the factory's Identity stack is SQLite. It's PostgreSQL since Phase 1.5.
  - **Docs:** README has a CI badge and a "Running Tests" section. CLAUDE.md describes the new CI and xunit.v3 setup. The Dependabot header now allows merging once both checks are green; docker and docker-compose PRs still need a local `docker compose build`, because CI doesn't build images.
- **2026-10-02 (Phase 2b, step 2: consistent error handling):** every error response is an RFC 9457 ProblemDetails (`application/problem+json`) with a `traceId`. Tests: 173 → 218, all passing.
  - **B-04:** fixed.
    - `AddProblemDetails()`, `UseExceptionHandler()` and `UseStatusCodePages()` now run in every environment, including Testing. `UseDeveloperExceptionPage` is gone.
    - `Infrastructure/ExceptionHandling/GlobalExceptionHandler` turns unhandled exceptions into a 500 with a generic `detail` and the `traceId`. It logs them at Error with the trace ID. The exception text is added only in Development.
    - `ExpectedFailures` maps `DbUpdateException` by the inner `PostgresException.SqlState`, never by the message:
      - `23505` (unique violation) → 409;
      - `23503` (foreign key violation) → 400 when inserting or updating (the request references a missing row), 409 when deleting (the row is still referenced).
    - `ImageWriter` throws `ImageValidationException`, mapped to 400 with the validator's message, instead of `InvalidOperationException` (500). Register and profile edit now validate images at binding with `[ImageUpload]`, like books, authors and articles already did.
    - A client abort is logged at Information as a 499, not an error.
    - The rate limiter's 429 is a ProblemDetails, and `Retry-After` is kept.
  - **B-07:** fixed. `ControllerExtensions` has `NoContentOrProblem`, `OkOrProblem` and `CreatedAtRouteOrProblem`. No controller returns `BadRequest(string)` or `{ errorMessage }` any more.
  - **Result kinds:** `Result`/`ResultWith<T>` carry an `ErrorKind` (`BadRequest` by default; `NotFound`, `Forbidden`, `Conflict`). Status changes:
    - not found → **404** (was 400) for articles, authors, books, reviews, notifications, profiles, reading-list entries, and votes on a missing review (was 200, the rest of B-11);
    - modifying someone else's book, author or review → **403** (was 400);
    - someone else's notification → **404**, not 403, because it is private;
    - duplicate review, same reading-list status twice, second check-in of the day, and a taken username/email at registration → **409** (was 400);
    - an invalid year on `GET /ReadingChallenges/{year}` and `/progress` → **400** (was 404).

    Login failures stay 400.
  - **S-11:** fixed. Client-facing messages use a friendly resource name ("The book was not found.", "You are not allowed to modify this review.") with no `…DbModel` type names or IDs. The log templates keep both. Other replaced texts:
    - the review duplicate and invalid-book messages (they contained the user ID);
    - the reading-list status message (it named `All()`);
    - the author gender/nationality messages;
    - the profile-deletion Identity errors (now logged only).
  - **Client:** `processError` reads ProblemDetails. For a 4xx it shows `detail`, else the first validation message from `errors`. Otherwise it uses the caller's fallback: for a 5xx, whose detail is generic, and when there is only a generic `title`. 13 new Vitest cases. `isSessionExpiredError`, `isNotFoundError` and `ErrorsRedirect` are unaffected: no client code branches on 400 vs 403/404/409.
  - **New tests:**
    - `ErrorHandlingIntegration`, through test-only endpoints on the real pipeline and database: a 500 has no internals, a real unique violation → 409, a real FK violation → 400, an image exception → 400, plus 401/403/404/validation ProblemDetails;
    - `ExpectedFailuresUnit`;
    - new `ReviewsIntegration`, `NotificationsIntegration`, `ReadingChallengesIntegration` and `ProfileIntegration` classes;
    - 403 and hidden-unapproved-404 cases in Books and Authors;
    - 409/400 cases in Identity and ReadingLists.

    `TestSeeder` gained `SeedReview` and `SeedNotification`. `ShouldBeProblem(...)` asserts the ProblemDetails shape.
  - **T-07:** fixed (the `AdminServiceIntegration` comment says PostgreSQL).
  - **Verified on a Development server** (dev Postgres + `dotnet run`):
    - the Swagger UI and `swagger.json` load;
    - `/health` is 200;
    - an anonymous call gives a 401 ProblemDetails;
    - a missing book `PUT` gives a 404 with `detail`;
    - bad credentials give a 400 with `detail`;
    - an empty form gives a ValidationProblemDetails;
    - an unknown route gives a 404 ProblemDetails.
  - **B-20 (new, Low):** `Reviews` has no unique index on (`CreatorId`, `BookId`). The duplicate-review check is read-then-write, so two concurrent creates can both succeed, and the new 409 mapping can't catch it without the index. Add a filtered unique index (`WHERE NOT "IsDeleted"`) in a migration.
  - **Note:** the service-level checks for undefined enum values (author gender/nationality, reading-list status) can't be reached over HTTP, because model binding already rejects undefined enum values with a ValidationProblemDetails. They're kept as a second line.
  - **Still open in Phase 2b:** the fallback authorization policy, explicit `[AllowAnonymous]` on Identity and `/health`, rejecting tokens of deleted users (S-06 minimum) and the authorization matrix (step 3). The Swagger UI already runs before authentication, so the fallback policy won't hide it.
- **2026-10-03 (Phase 2b, steps 3–6: authorization hardening and the authorization matrix):**
  - Tests: 218 → 379, all passing, in about 1 m 30 s. The new tests are 155 matrix cases, 4 real-JWT tests and 2 B-20 tests. Both CI jobs pass on a clean copy of the working tree with the same steps and flags as `ci.yml`.
  - **Fallback policy:** every endpoint requires an authenticated user unless it opts out. The anonymous set is unchanged from before the phase, per the approved table:
    - `Books/top`, `Authors/top`, `Profile/top` and `Statistics`;
    - `Articles/{id}` and `Search/articles`;
    - the four `Identity` endpoints, now with an explicit `[AllowAnonymous]` (before, they relied on having no attribute);
    - `/health`, now with `.AllowAnonymous()`.

    Uploaded images stay public: `UseStaticFiles` short-circuits before `UseAuthorization`, and a test GETs `/images/books/1984.jpg` anonymously. The Swagger UI runs before authentication. An anonymous request to an unknown URL now gets a 401 ProblemDetails instead of a 404, which is expected with a fallback policy.
  - **S-06 (partial, the minimum):** fixed. `OnTokenValidated` rejects the token with a 401 when its user no longer exists or is soft-deleted. It's one `AnyAsync` on the ID and `!IsDeleted`. The client's 401 interceptor then logs the user out.
    - **How it's tested:** `DeletedUserTokenIntegration` runs the real JwtBearer pipeline. The factory has `UseTestAuthentication => false`, and the tokens are signed by `/Identity/register`. The tests cover self-deletion through `DELETE /Profile`, a soft delete elsewhere (as the admin delete does), a still-valid user and a malformed token.
    - **Mutation-checked:** with the hook disabled, both deleted-user tests fail.
    - Refresh tokens, short-lived access tokens and role re-validation remain Phase 4.
  - **Authorization matrix (the Phase 2 exit criterion for section 3.2):** `server/BookHub.Tests/Authorization/AuthorizationMatrix.cs` lists all 67 controller actions plus `/health`. 57 are protected, each with `User`, `Owner` (with the wrong-user status) or `Admin` access, and 11 are public. `AuthorizationMatrixIntegration` tests:
    - anonymous → 401 ProblemDetails (57);
    - a non-admin on admin endpoints → 403 (17);
    - the wrong user on owner endpoints → 403 for books, authors and reviews; 404 for notifications and a private profile's reading lists (10);
    - the allowed caller is not rejected (57);
    - every public endpoint works anonymously (11).

    A completeness guard enumerates `EndpointDataSource` at runtime. It fails when a routed endpoint is in neither list, when a listed endpoint no longer exists, or when the `[AllowAnonymous]` endpoints and the public list disagree. It's mutation-checked: removing one matrix row and removing the fallback policy each fail it. The class shares one app host (`AuthorizationMatrixFixture`) and resets the database per test.
  - **F-16:** fixed. Search uses the shared `http` instance instead of the global `axios`, so it goes through the 401 interceptor and the base URL. There's a Vitest regression test.
  - **Client calls vs. the table:** no client code calls a protected endpoint anonymously. The anonymous calls are home tops, statistics, article details and article search, and identity. Books/authors/genres/profiles search and the details pages run only behind `AuthenticatedRoute`, and header notifications load only when authenticated.
  - **B-20:** fixed. A new migration, `AddReviewUniqueIndex`, adds `IX_Reviews_CreatorId_BookId` as a unique index with `WHERE NOT "IsDeleted"`, so a user can review again after deleting their review.
    - EF also drops `IX_Reviews_CreatorId`, because the new index's leading column covers the FK. Every review query goes through the soft-delete filter, so the partial index covers them.
    - The `Up` would fail on a database that already holds duplicate live reviews. There are none in production, which is a fresh database.
    - Tests: a duplicate insert that bypasses the service check → 409 with one row kept; re-reviewing after a delete → 201.
    - `StatisticsIntegration` seeded two live reviews by one user for one book, which the index now rejects. Its seed now soft-deletes the first review before writing the second, and its assertions are unchanged.
    - `has-pending-model-changes` reports no changes.
  - **Small items:** the stale "Returns null…" comment on `IReadingListService.All` is removed. Account enumeration through registration's 409 is in `docs/backlog.md` (open, decision needed; behavior unchanged).
  - **Verified on the dev stack** (`docker compose … --env-file .env.example`, demo data from the DataImporter):
    - anonymous: home tops, statistics, an article, article search, a cover image and `/health` all return 200; book details and books search return 401; an unknown URL returns a 401 ProblemDetails;
    - a newly registered user: book details, search, profile, review create (201, then 409 on a repeat), reading-list add (204); someone else's book edit returns 403; an admin endpoint returns 403;
    - the admin: admin book details and notifications return 200;
    - after a self-delete, the same token gets a 401.
  - **B-21 (new, Low):** `appsettings.json` has empty `JwtSettings:Issuer`/`Audience`, and outside Development both are validated. With `ISSUER`/`AUDIENCE` unset in the environment, tokens carry no issuer or audience, and every authenticated request would get a 401. Compose and `.env.example` set them, and the test factory now does too. Fix by making both `[Required]` in `JwtSettings`, so startup fails fast.
  - **Still open from Phase 2:** T-06 (xUnit1051 is still suppressed), and branch protection requiring the `server` and `client` checks (a GitHub setting).
