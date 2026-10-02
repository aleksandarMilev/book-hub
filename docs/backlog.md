# Backlog

Features that are deliberately out of scope for now. Each entry says what existed before, why it was removed, and what a rebuild must meet.

## Chat (nice-to-have, post-refactor)

**Status:** removed in Phase 0 (2026-10). Not to be rebuilt until the refactor is done.

**What the old feature did**

- Private, named chats created by a user, each with an optional image (stored under `wwwroot/images/chats`).
- Invitations: the creator invited users from their profile page. Invitees accepted or rejected, and every step produced a notification (`ResourceType.Chat`).
- Message history: members posted messages (up to 5,000 characters), loaded with cursor-based paging, and edited or deleted their own messages. The creator could edit or delete the chat and remove members.
- Full-text search over chat names, limited to chats the caller had joined.
- Client: list, details, create/edit pages, a `ChatRoute` guard, and a `chats` i18n namespace in en and bg.

**Why it was removed**

It carried most of the open authorization bugs found in the [October 2026 code review](review/2026-10-code-review.md):

- **S-01:** `access/{userId}` and `invited/{userId}` took the user ID from the route, so any user could probe anyone's membership.
- **S-02:** invitation accept/reject trusted the chat creator ID and chat name from the request body, so any user could send a notification with chosen text to any user.
- **S-03:** invitees who hadn't accepted could read the participants and the last 20 messages.
- **S-05:** `ChatMessageController` had no `[Authorize]`.
- **F-10:** the client guard rendered the protected page while the access check was still loading, with no abort or error handling.
- **F-13:** it wasn't real-time. `@microsoft/signalr` was installed but never used, and new messages appeared only on refetch.

It also had no tests (T-03).

**Where the old code is**

Git tag `chat-before-removal`. The `RemoveChat` migration dropped the `Chats`, `ChatMessages` and `ChatsUsers` tables, the full-text index on `Chats`, and all chat notifications.

**Requirements for a rebuild**

- **Real-time with SignalR:** a hub for message delivery and invitation events, authenticated with the same JWT as the API.
- **Authorization from claims only:** never take the acting user's ID, a creator ID or a chat name from the route or body. Load the chat from the database and derive everything else from it.
- **Invitees can't read before accepting:** a pending invitee sees an invitation preview at most (chat name and inviter), never participants or messages. This applies to the hub as well as the REST endpoints.
- **Integration tests for every endpoint and hub method**, covering three roles:
  - **member** (accepted): allowed;
  - **invitee** (pending): only the preview and accept/reject;
  - **outsider:** 403/404, with no information about whether the chat exists.
- Every controller and the hub require authentication explicitly.

## Welcome email outbox (post-Postgres)

**Status:** open, from Phase 1a (B-02).

The welcome email is sent in the background through an in-memory, bounded queue (`Features/Emails/WelcomeEmailQueue`). The in-memory email queue loses pending emails on restart. Consider an outbox table after the Postgres migration, so queued emails survive restarts and failed sends can be retried.

## Seed the "Other" genre (Postgres migration)

**Status:** done in Phase 1.5 (2026-10).

Books created without genres get the "Other" genre (`52e607d4-c347-440a-8d55-cf2e01d88a6c`) only if it exists. Before Phase 1.5 it only existed after an admin ran the DataImporter. The `InitialPostgres` migration now seeds it (`GenreConfiguration.HasData`, same ID and values as `genres.json`), and importing `genres.json` skips it as an existing row.

## Phase 2: test framework upgrades

**Status:** done in Phase 2a (2026-10). Deferred from Phase 1c (T-05).

The major upgrades in `server/BookHub.Tests/BookHub.Tests.csproj`:

- `xunit` 2.9.3 → `xunit.v3.mtp-off` 4.0.1. This is the `xunit.v3` package with Microsoft Testing Platform turned off. The default `xunit.v3` 4.x enables MTP v2, which fails `dotnet test` in VSTest mode on the .NET 10 SDK, so it would need a `"test": { "runner": "Microsoft.Testing.Platform" }` entry in `global.json`. Moving to MTP is a separate decision.
- `xunit.runner.visualstudio` 3.1.5 → 4.0.0 (co-released with xunit.v3 4.0)
- `NSubstitute` 5.3.0 → 6.2.0
- `coverlet.collector` 6.0.4 → 10.1.0 (still the VSTest data collector)
- `FluentAssertions` 8.8.0 → 8.11.0

None of them has an open advisory.

**Follow-up:** the new analyzer rule xUnit1051 ("pass `TestContext.Current.CancellationToken`") has about 300 hits and is suppressed in the csproj. Adopt it while the tests are restructured in Phase 2b/2c, then remove the `NoWarn`.
