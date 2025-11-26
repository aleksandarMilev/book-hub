# Identity Feature - **Server Documentation**

This document summarizes the **server-side** implementation of the Identity feature in BookHub.

---

## Overview

- ASP.NET Core Web API with **JWT-only** authentication (no cookies).
- Uses `AddIdentityCore<User>` + ASP.NET Core Identity for users, passwords and lockout.
- Login & register endpoints return a JWT token on success.
- Errors are returned as `400 Bad Request` with a JSON `{ errorMessage }` payload.

---

## Configuration

**Location:** `BookHub/Program.cs` + `Infrastructure/Extensions/ServiceCollectionExtensions.cs`

- `AddIdentity`:

  - `AddIdentityCore<User>` with:
    - Unique email required.
    - Lockout after `MaxFailedLoginAttempts` for `AccountLockoutTimeSpan` minutes.
    - Dev: relaxed password rules, Prod: stronger password rules.
  - `AddRoles<IdentityRole>()`, `AddEntityFrameworkStores<BookHubDbContext>()`, `AddDefaultTokenProviders()`.

- `AddJwtAuthentication`:

  - Reads `JwtSettings` (Secret, Issuer, Audience) from configuration.
  - Registers JWT bearer as the **default** auth scheme.
  - Dev:
    - Validates signing key and lifetime, skips issuer/audience.
  - Non‑Dev:
    - Validates signing key, issuer, audience and lifetime (with small clock skew).

- Middleware order:
  - `UseRouting() -> UseStaticFiles() -> UseAllowedCors() -> UseAuthentication() -> UseAuthorization() -> UseAppEndpoints()`.

---

## Data & Seeding

**User entity:** `Features/Identity/Data/Models/User.cs`

- Inherits from `IdentityUser` and `IDeletableEntity`.
- Adds soft‑delete metadata and relations to other domain entities (books, reviews, chats, etc.).

**Seeding:**

- `UsersSeeder` seeds a few static users (ID, UserName, Email) for domain data usage (not login).
- `UseAdminRole()` (Development only) creates:
  - Admin role (`AdminRoleName`).
  - Admin user `admin@mail.com` with password `admin1234` and assigns the admin role.

---

## Identity Service

**Location:** `Features/Identity/Service/IdentityService.cs`

### Interface

```csharp
Task<ResultWith<string>> Register(string email, string username, string password);
Task<ResultWith<string>> Login(string credentials, string password, bool rememberMe);
```

- `ResultWith<string>.Data` holds the JWT token on success.
- On failure, `ErrorMessage` holds a user‑friendly description.

### Register

- Creates an `Identity` user using `UserManager<User>.CreateAsync`.
- On success:
  - Generates JWT token.
  - Logs registration.
  - Sends welcome email.
- On failure:
  - Collects Identity errors into a single `errorMessage`.

### Login

- Accepts **username or email** via `credentials`.
- Checks lockout state and password validity.
- On success:
  - Resets failed count.
  - Checks if the user is in the admin role.
  - Generates JWT token:
    - Shorter expiration by default.
    - Longer expiration if `rememberMe` is true.
- On failure:
  - Returns messages like:
    - `Invalid log in attempt!`
    - `Account is locked. Try again later.`
    - `Account locked due to multiple failed attempts.`

### JWT Claims

Generated token includes:

- `nameid` (NameIdentifier) → user ID.
- `unique_name` (Name) → username.
- `email` → email.
- `role` (only present if the user is an admin).

---

## Web API

**Routes:** `Features/Identity/Web/ApiRoutes.cs`

- `POST /identity/register` (actual prefix depends on `ApiController` base route).
- `POST /identity/login`.

**Request Models:**

```csharp
// Register
public class RegisterRequestModel {
    [Required] public string Username { get; init; } = null!;
    [Required] public string Email { get; init; } = null!;
    [Required] public string Password { get; init; } = null!;
}

// Login
public class LoginRequestModel {
    [Required] public string Credentials { get; init; } = null!;
    public bool RememberMe { get; init; }
    [Required] public string Password { get; init; } = null!;
}
```

**Response Model (success):**

```csharp
public class LoginResponseModel {
    public string Token { get; init; }
}
```

**Controller:** `Features/Identity/Web/IdentityController.cs`

- Calls `IIdentityService` and then uses an extension method `OkOrBadRequest` to shape responses:
  - On success: `200 OK` with `{ token }`.
  - On failure: `400 Bad Request` with `{ errorMessage }`.

This error shape is consumed directly by the client `processError` helper.

---

## Summary

The server identity feature provides:

- JWT‑based authentication only (no cookies, no redirects).
- ASP.NET Core Identity for secure password and lockout management.
- Simple, well‑defined login/register endpoints that return tokens.
- Consistent error payloads for easy client‑side handling.
