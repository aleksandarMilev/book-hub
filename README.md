# BookHub

BookHub is a full-stack book community platform for discovering and sharing books, authors, reviews, and articles. It includes user profiles, reading lists, notifications, and admin moderation.

## Highlights

- JWT-based auth with welcome emails and forgot-password reset flow
- Book and author catalogs with approval workflow and top lists
- Genres, statistics dashboard, and full-text search
- Reading lists (To Read, Currently Reading, Read)
- Reading challenges with progress tracking
- Reviews with voting
- Articles (public reading, admin authoring)
- Notifications center (mark read, delete)
- Image uploads for books, authors, articles, and profiles
- Admin tools for approvals and profile management

## Architecture

- `client/` React 18 + Vite SPA
- `server/` ASP.NET Core Web API (.NET 10) with EF Core and Identity
- PostgreSQL 18 (official `postgres:18-alpine` image) with built-in full-text search
- Docker Compose for dev and prod stacks

## Tech Stack

- Frontend: React 18, Vite, TypeScript, React Router, Zustand, Formik/Yup, Bootstrap + MDB, i18next, Axios, Vitest
- Backend: ASP.NET Core 10, EF Core, Identity, JWT auth, Swagger, MailKit, health checks
- Database: PostgreSQL 18 (EF Core via Npgsql), full-text search with `tsvector` + GIN
- Tooling: ESLint, Prettier, Husky, Docker

## Project Structure

- `client/` frontend app
- `server/` API and tests
- `docker-compose.dev.yml` local dev stack
- `docker-compose.prod.yml` production stack
- `.env.example` environment template

## Quick Start (Docker)

1. Copy the env template and adjust values as needed.

```bash
cp .env.example .env
```

2. Start the dev stack.

```bash
docker compose -f docker-compose.dev.yml --env-file .env up --build
```

Services:

- Client: `http://localhost:5173`
- API + Swagger UI (Development only): `http://localhost:8080`
- Health check: `http://localhost:8080/health`

## Local Development (no Docker)

1. Start PostgreSQL 18 locally. The simplest way is the Compose service alone: `docker compose -f docker-compose.dev.yml --env-file .env up -d postgres` (published on `127.0.0.1:5432`). `appsettings.Development.json` already points at it with the `.env.example` credentials.
2. Configure the API connection string and app settings. You can set `ConnectionStrings__DefaultConnection` as an environment variable or edit `server/BookHub/appsettings.Development.json`.
3. Start the API.

```bash
dotnet restore server/BookHub/BookHub.csproj
dotnet run --project server/BookHub/BookHub.csproj
```

4. Start the client.

```bash
cd client
npm install
npm run dev
```

The client uses `VITE_REACT_APP_SERVER_URL` from `client/.env`. If it is not set, the default is `http://localhost:8080`.

## Environment Variables

These are the primary env vars used by the Docker stacks. For local runs you can use the same names, or configure `server/BookHub/appsettings.Development.json`.

| Variable               | Purpose                                                       |
| ---------------------- | ------------------------------------------------------------- |
| `POSTGRES_DB`          | Database name (created on first start of the `postgres` container) |
| `POSTGRES_USER`        | Database user. The API connects as this user for now; a least-privilege role is planned (S-09) |
| `POSTGRES_PASSWORD`    | Password for `POSTGRES_USER`                                  |
| `APP_SECRET`           | JWT signing key (at least 32 bytes; startup fails otherwise) |
| `ISSUER`               | JWT issuer                                                    |
| `AUDIENCE`             | JWT audience                                                  |
| `SMTP_HOST`            | SMTP host used for welcome emails                             |
| `SMTP_PORT`            | SMTP port                                                     |
| `SMTP_USER`            | SMTP username                                                 |
| `SMTP_PASSWORD`        | SMTP password                                                 |
| `SMTP_FROM`            | From address for emails                                       |
| `SMTP_USE_SSL`         | `true` or `false`                                             |
| `CORS_ALLOWED_ORIGINS` | Semicolon-separated list of allowed origins (Production only) |
| `CLIENT_BASE_URL`          | Public client URL used in email links (`AppUrlsSettings__ClientBaseUrl`); required, absolute URL |
| `BOOTSTRAP_ADMIN_ENABLED`  | `true` creates the admin on startup outside Development (idempotent); default `false` |
| `BOOTSTRAP_ADMIN_EMAIL`    | Bootstrap admin email (required when enabled)                 |
| `BOOTSTRAP_ADMIN_PASSWORD` | Bootstrap admin password (required when enabled; must meet the Identity password rules) |
| `BOOTSTRAP_ADMIN_ROLE`     | Bootstrap admin role (required when enabled; must be `Administrator`) |

Optional, mainly for local runs:

- `ConnectionStrings__DefaultConnection` to override the DB connection string
- `VITE_REACT_APP_SERVER_URL` for the client base API URL

## Ports

- Client dev server: `5173`
- API: `8080` (HTTP)
- API: `8081` (HTTPS)
- PostgreSQL: `5432` (dev only, bound to `127.0.0.1`; not published in production)

## API Notes

- Swagger UI is enabled only in Development and is hosted at the API root (`http://localhost:8080`).
- Admin endpoints are under `Administrator/*` and require the `Administrator` role.
- Health check endpoint is `/health`.

## Database, Migrations, and Seeding

- Migrations are applied automatically on startup in Development only.
- The initial migration seeds the "Other" genre, which books created without genres fall back to.
- Demo data is imported on demand by the admin endpoints `POST /Administrator/DataImporter/{all|books|authors|genres|articles|books-genres}/`, which read `server/BookHub/Features/DataImporter/Data/*.json`. Rows that already exist are skipped.
- Search uses PostgreSQL full-text search: a generated `tsvector` column (`simple` config, no stemming, because content is mixed English/Bulgarian) with a GIN index on books, authors, articles, genres and profiles. Each search word is matched as a prefix, and all words must match.
- The database is initialized with the ICU root collation (`POSTGRES_INITDB_ARGS`), which gives linguistic ordering and correct Cyrillic case folding. It only applies when the data volume is first created.

## Default Admin (Development Only)

A default admin role and user are created in Development on startup:

- Email: `admin@mail.com`
- Password: `admin1234`

## Scripts

Client scripts (run from `client/`):

- `npm run dev`
- `npm run build`
- `npm run preview`
- `npm run lint`
- `npm run lint:fix`
- `npm run typecheck`
- `npm run test`
- `npm run test:watch`
- `npm run format`
- `npm run format:check`

Server scripts:

- `dotnet run --project server/BookHub/BookHub.csproj`
- `dotnet test server/BookHub.sln` (Docker must be running: the tests start a PostgreSQL container through Testcontainers)

## Production Notes

- Swagger UI is disabled outside Development.
- `CORS_ALLOWED_ORIGINS` must be set in Production or startup will fail.
- Uploaded files are stored under `server/BookHub/wwwroot` in dev and in the `server_uploads` Docker volume in production.
- The production stack does not auto-apply migrations. Apply migrations as part of your deployment process.
