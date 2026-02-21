# BookHub

BookHub is a full-stack book community platform for discovering and sharing books, authors, reviews, and articles. It includes user profiles, reading lists, chats, notifications, and admin moderation.

## Highlights

- JWT-based auth with welcome emails and forgot-password reset flow
- Book and author catalogs with approval workflow and top lists
- Genres, statistics dashboard, and full-text search
- Reading lists (To Read, Currently Reading, Read)
- Reading challenges with progress tracking
- Reviews with voting
- Articles (public reading, admin authoring)
- Private chats with invitations and message history
- Notifications center (mark read, delete)
- Image uploads for books, authors, articles, chats, and profiles
- Admin tools for approvals and profile management

## Architecture

- `client/` React 18 + Vite SPA
- `server/` ASP.NET Core Web API (.NET 10) with EF Core and Identity
- `sqlserver/` SQL Server 2022 image with Full-Text Search enabled
- Docker Compose for dev and prod stacks

## Tech Stack

- Frontend: React 18, Vite, TypeScript, React Router, Zustand, Formik/Yup, Bootstrap + MDB, i18next, Axios, Vitest
- Backend: ASP.NET Core 10, EF Core, Identity, JWT auth, Swagger, MailKit, health checks
- Database: SQL Server 2022 + Full-Text Search
- Tooling: ESLint, Prettier, Husky, Docker

## Project Structure

- `client/` frontend app
- `server/` API and tests
- `sqlserver/` SQL Server Docker image with FTS
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

1. Start SQL Server locally. Full-Text Search is required for the search endpoints.
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
| `SA_PASSWORD`          | SQL Server `sa` password for the Docker image                 |
| `DB_NAME`              | Database name used in the connection string                   |
| `APP_SECRET`           | JWT signing key (16+ characters recommended)                  |
| `ISSUER`               | JWT issuer                                                    |
| `AUDIENCE`             | JWT audience                                                  |
| `SMTP_HOST`            | SMTP host used for welcome emails                             |
| `SMTP_PORT`            | SMTP port                                                     |
| `SMTP_USER`            | SMTP username                                                 |
| `SMTP_PASSWORD`        | SMTP password                                                 |
| `SMTP_FROM`            | From address for emails                                       |
| `SMTP_USE_SSL`         | `true` or `false`                                             |
| `CORS_ALLOWED_ORIGINS` | Semicolon-separated list of allowed origins (Production only) |

Optional, mainly for local runs:

- `ConnectionStrings__DefaultConnection` to override the DB connection string
- `VITE_REACT_APP_SERVER_URL` for the client base API URL

## Ports

- Client dev server: `5173`
- API: `8080` (HTTP)
- API: `8081` (HTTPS)
- SQL Server: `1433`

## API Notes

- Swagger UI is enabled only in Development and is hosted at the API root (`http://localhost:8080`).
- Admin endpoints are under `Administrator/*` and require the `Administrator` role.
- Health check endpoint is `/health`.

## Database, Migrations, and Seeding

- Migrations are applied automatically on startup in Development only.
- Seed data is loaded from JSON files in `server/BookHub/Features/*/Data/Seed/*.json`.
- The Docker SQL Server image includes Full-Text Search for the search feature.

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
- `dotnet test server/BookHub.sln`

## Production Notes

- Swagger UI is disabled outside Development.
- `CORS_ALLOWED_ORIGINS` must be set in Production or startup will fail.
- Uploaded files are stored under `server/BookHub/wwwroot` in dev and in the `server_uploads` Docker volume in production.
- The production stack does not auto-apply migrations. Apply migrations as part of your deployment process.

## Troubleshooting

- Registration fails: welcome emails are required and SMTP must be configured correctly.
- CORS errors in Production: ensure `CORS_ALLOWED_ORIGINS` is set and uses `;` separators.
- Search not working: ensure SQL Server Full-Text Search is installed and enabled.
