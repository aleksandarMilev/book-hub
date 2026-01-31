# BookHub

BookHub is a full-stack book community platform for discovering and sharing books, authors, reviews, and articles, with user profiles, reading lists, chat, notifications, and admin moderation.

## Highlights

- Account registration/login with JWT auth and welcome emails
- Book and author catalogs with approval workflow and top lists
- Genres, search, and statistics dashboards
- Reading lists (To Read, Currently Reading, Read)
- Reviews with voting
- Articles (public reading, admin authoring)
- Private chats with invitations and message history
- Notifications center (mark read, delete)
- Image uploads for books, authors, articles, chats, and profiles
- Admin tools for approvals and profile management

## Tech stack

- **Frontend:** React 18 + Vite, TypeScript, React Router, Zustand, Formik/Yup, Bootstrap/MDB, i18next, Axios, Vitest
- **Backend:** ASP.NET Core (.NET 10), EF Core, Identity, JWT auth, Swagger, MailKit, health checks
- **Database:** SQL Server 2022 with Full-Text Search
- **Containers:** Docker + Docker Compose (dev/prod)

## Project structure

- `client/` — React + Vite frontend
- `server/` — ASP.NET Core Web API
- `sqlserver/` — SQL Server image with Full-Text Search
- `docker-compose.dev.yml` — local development stack
- `docker-compose.prod.yml` — production stack
- `.env.example` — environment template

## Getting started

### Prerequisites

- Docker Desktop (recommended for fastest setup)
- Node.js 20+ (frontend tooling)
- .NET 10 SDK (for running the API outside Docker)

### 1) Configure environment

Copy the template and adjust values as needed:

```bash
cp .env.example .env
```

The most important settings:

- `SA_PASSWORD` — SQL Server SA password
- `DB_NAME` — database name
- `APP_SECRET` — JWT signing key (>= 16 chars)
- `ISSUER`, `AUDIENCE` — JWT issuer/audience
- `SMTP_*` — SMTP settings used for welcome emails
- `CORS_ALLOWED_ORIGINS` — semicolon-separated list of allowed origins (prod)

### 2) Run with Docker (recommended)

```bash
docker compose -f docker-compose.dev.yml --env-file .env up --build
```

Services:

- Client: `http://localhost:5173`
- API + Swagger UI: `http://localhost:8080`
- Health check: `http://localhost:8080/health`

### 3) Run locally (without Docker)

1. Start SQL Server locally (ensure Full-Text Search is available if you use search).
2. Configure environment variables or `server/BookHub/appsettings.Development.json`.
3. Start the API:

```bash
dotnet restore server/BookHub/BookHub.csproj
dotnet run --project server/BookHub/BookHub.csproj
```

4. Start the client:

```bash
cd client
npm install
npm run dev
```

The client reads `VITE_REACT_APP_SERVER_URL` from `client/.env`. If unset, it defaults to `http://localhost:8080`.

## Default admin (development only)

In Development, the API auto-creates an admin role and user:

- Email: `admin@mail.com`
- Password: `admin1234`

This only runs in Development on startup.

## Data seeding

Seed data is provided for books, authors, genres, and articles in:

- `server/BookHub/Features/**/Data/Seed/*.json`

Migrations run automatically in Development (`UseMigrations()`), which applies the seed data.

## API notes

- Base API URL: `http://localhost:8080`
- Swagger UI: `http://localhost:8080`
- Swagger JSON: `http://localhost:8080/swagger/v1/swagger.json`
- Admin endpoints are under the `Administrator` area (e.g., `/Administrator/Books`)

## Client scripts

Run from `client/`:

```bash
npm run dev
npm run build
npm run preview
npm run lint
npm run typecheck
npm run test
```

## Production

To build and run the production stack:

```bash
docker compose -f docker-compose.prod.yml --env-file .env up --build
```

Notes:

- Client is served on port `80`
- API listens on ports `8080` (HTTP) and `8081` (HTTPS)
- Uploaded images are stored in a Docker volume (`server_uploads`)

## Troubleshooting

- **Registration fails:** the welcome email is required; check `SMTP_*` settings.
- **CORS errors in production:** ensure `CORS_ALLOWED_ORIGINS` is set and uses `;` separators.
- **Search not working:** the SQL Server image includes Full-Text Search; ensure your SQL Server has it enabled.
