# BookHub

BookHub is a full-stack web application for discovering, organizing, and discussing books.  
It includes rich editorial content (Articles) and a JWT-based Identity system for secure authentication.

---

## Table of Contents

- [Articles](#articles)
  - [Articles - Data & Validation](#articles---data--validation)
  - [Articles - API & Services](#articles---api--services)
  - [Articles - Client Functionality](#articles---client-functionality)
  - [Articles - Routing (Client)](#articles---routing-client)
  - [Articles - Limitations](#articles---limitations)
- [Identity](#identity)
  - [Identity - Server](#identity---server)
  - [Identity - Client](#identity---client)
  - [Identity - Limitations](#identity---limitations)

---

## Articles

The Articles feature enables BookHub to provide editorial content with:

- Public article viewing
- Admin-only create/edit/delete
- Image upload and validation
- View-count tracking
- Localized UI (EN/BG)
- Markdown-based content
- SEO-friendly canonical slug routing
- Search + pagination

---

## Articles - Data & Validation

### Server Model

- Title: 10–100 chars
- Introduction: 10–500 chars
- Content: 100–50,000 chars
- Soft-delete support
- View count stored server-side
- ImagePath stored with per-article file

### Client Types

- `CreateArticle` (title, introduction, content, optional image)
- `ArticleDetails` extends server data + computed `readingMinutes`

---

## Articles - API & Services

### Server

- Separate public and admin controllers
- Public: GET article details (+view count increment)
- Admin: GET for edit, POST create, PUT edit, DELETE soft-delete

### Image Handling

- Max 2 MB
- Allowed: JPG, JPEG, PNG, WEBP, AVIF
- Invalid → 400 Bad Request
- New image replaces old; default used when none provided

---

## Articles - Client Functionality

### API Client

- `details(id)`
- `detailsForEdit(id, token)`
- `create(article, token)`
- `edit(id, article, token)`
- `remove(id, token)`

### Hooks

- `useDetails` — fetches article + computes reading time
- `useCreate` / `useEdit` — multipart form submission + toasts + canonical redirects
- `useRemove` — confirmation modal + delete
- `useDetailsPage` — details fetch, timestamp formatting, slug enforcement
- `useListPage` — search and pagination

### Components

- Form flow: `ArticleForm` + `CreateArticle` / `EditArticle` pages
- Details page: markdown rendering, breadcrumb, image fallback, admin actions
- Listing: search, pagination, empty states

### Validation & Localization

- Yup schema regenerated on language change
- Localized messages (i18next `articles:*`)
- Full EN/BG support via `articles.json` namespaces

---

## Articles - Routing (Client)

SEO-friendly:

```text
/articles/{id}/{slug}
```

- Slug used only client-side
- Mismatches → navigate to canonical

---

## Articles - Limitations

**Client**

- No offline caching
- View count vulnerable to refresh spamming
- Image alt text = title

**Server**

- View count also unprotected
- Minimal alt text
- Soft-delete can leave orphan images until next replacement

---

## Identity

The Identity feature provides user authentication and authorization using JWTs and ASP.NET Core Identity.

- JWT-only auth (no cookies)
- Registration and login endpoints
- Account lockout on repeated failures
- Admin seeding in development
- Localized error messages and UI
- Persistent auth state on the client

---

## Identity - Server

### Configuration

- Uses `AddIdentityCore<User>` with:
  - Unique email required
  - Lockout after `MaxFailedLoginAttempts` for `AccountLockoutTimeSpan` minutes
  - Relaxed password rules in Development, stronger rules otherwise
- Uses `AddJwtAuthentication` with:
  - Secret, Issuer, Audience from `JwtSettings`
  - JWT bearer as the default authentication scheme
  - In Development: validates signature and lifetime, skips issuer/audience
  - In non-Development: validates signature, issuer, audience, and lifetime

### Data Model & Seeding

- `User` entity:
  - Inherits `IdentityUser` and soft-delete metadata
  - Holds relations to domain entities (books, reviews, chats, etc.)
- Seeding:
  - `UsersSeeder` creates sample users (for related data)
  - `UseAdminRole` (Development only) creates:
    - Admin role
    - Admin user `admin@mail.com` with password `admin1234`

### Identity Service

- `Register(email, username, password)`:
  - Creates user via `UserManager`
  - On success: generates JWT, logs, sends welcome email
  - On failure: aggregates Identity errors into a single message
- `Login(credentials, password, rememberMe)`:
  - Accepts username or email
  - Checks lockout and password
  - On success:
    - Resets failed count
    - Checks admin role
    - Generates JWT with expiration based on `rememberMe`
  - On failure: returns friendly error messages

### JWT

Generated token includes:

- `nameid` — user ID
- `unique_name` — username
- `email` — user email
- `role` — present only if user is admin

### Web API

- Routes:
  - `POST /identity/register`
  - `POST /identity/login`
- Request models:
  - `RegisterRequestModel` (Username, Email, Password)
  - `LoginRequestModel` (Credentials, Password, RememberMe)
- Response model:
  - `LoginResponseModel` with `Token`
- Controller uses an `OkOrBadRequest` extension:
  - Success → `200 OK` with `{ token }`
  - Failure → `400 Bad Request` with `{ errorMessage }`

---

## Identity - Client

### Types

- `LoginResponse` with `token`
- `RegisterRequest` / `LoginRequest`
- `DecodedToken`:
  - `nameid`, `unique_name`, `email`, optional `role`
- `RegisterFormValues` extends `RegisterRequest` with `confirmPassword`

### HTTP & Error Handling

- Shared HTTP utilities in `shared/api/http.ts`
- `processError`:
  - Re-throws canceled requests
  - Reads `errorMessage` (or `message` / `title`) from server responses
  - Falls back to localized error text when no server message exists

### Identity API Client

- `register(username, email, password, signal?)`:
  - Calls `POST /identity/register`
  - Returns `{ token }` or throws error
- `login(credentials, password, rememberMe, signal?)`:
  - Calls `POST /identity/login`
  - Returns `{ token }` or throws error

### Auth Store Integration

- Uses Zustand store for auth:
  - `userId`, `username`, `email`, `token`, `isAdmin`, `hasProfile`, `isAuthenticated`
- State persisted via `localStorage`
- Identity hooks populate and clear this store

### Hooks

- `useLogin`:
  - Calls login API
  - Decodes JWT
  - Fills auth store (`User` object)
  - Derives `hasProfile` from `profileApi.hasProfile(token)`
  - Shows localized welcome toast
  - On error: throws generic `loginFailed` message
- `useRegister`:
  - Calls register API
  - Decodes JWT
  - Fills auth store with `hasProfile = false`
  - Shows welcome toast
  - On error: rethrows error so forms can display server message

### Forms & Components

- **Login:**
  - `Login.tsx`, `useLoginFormik`, `loginSchema`
  - Fields: `credentials`, `password`, `rememberMe`
  - Yup validation for required fields
  - Error shown under `credentials` using `loginFailed` text
- **Register:**
  - `Register.tsx`, `useRegisterFormik`, `registerSchema`
  - Fields: `username`, `email`, `password`, `confirmPassword`
  - Yup validation including email format and password confirmation
  - Server or fallback error shown under `username`
- **Logout:**
  - Clears auth store
  - Shows goodbye toast (if username existed)
  - Navigates to home

### Localization

- Uses `identity` i18next namespace:
  - Form labels
  - Validation messages
  - Toast texts (welcome, goodbye, errors)
- Supports EN/BG in the same way as Articles.

---

## Identity - Limitations

- Login errors intentionally use generic messages (no detailed reason).
- JWT revocation/blacklisting is not implemented (relies on expiration).
- `hasProfile` is fetched separately after login (extra request).
