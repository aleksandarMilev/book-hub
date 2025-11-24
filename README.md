# BookHub

---

## Articles

The Articles feature enables BookHub to provide editorial content with:

- Public article viewing\
- Admin-only create/edit/delete\
- Image upload and validation\
- View‑count tracking\
- Localized UI (EN/BG)\
- Markdown-based content\
- SEO-friendly canonical slug routing\
- Search + pagination

---

## Data & Validation

### Server Model

- Title: 10--100 chars\
- Introduction: 10--500 chars\
- Content: 100--50,000 chars\
- Soft‑delete support\
- View count stored server-side\
- ImagePath stored with per-article file

### Client Types

- `CreateArticle` (title, introduction, content, optional image)\
- `ArticleDetails` extends server data + computed `readingMinutes`

---

## API & Services

### Server

- Separate public and admin controllers\
- Public: GET article details (+view count increment)\
- Admin: GET for edit, POST create, PUT edit, DELETE soft-delete

### Image Handling

- Max 2 MB\
- Allowed: JPG, JPEG, PNG, WEBP, AVIF\
- Invalid → 400 Bad Request\
- New image replaces old; default used when none provided

---

## Client Functionality

### API Client

- `details(id)`\
- `detailsForEdit(id, token)`\
- `create(article, token)`\
- `edit(id, article, token)`\
- `remove(id, token)`

### Hooks

- `useDetails` --- fetches article + computes reading time\
- `useCreate` / `useEdit` --- multipart form submission + toasts +
  canonical redirects\
- `useRemove` --- confirmation modal + delete\
- `useDetailsPage` --- details fetch, timestamp formatting, slug
  enforcement\
- `useListPage` --- search and pagination

### Components

- **Form Flow:** ArticleForm + CreateArticle/EditArticle pages\
- **Details Page:** markdown rendering, breadcrumb, image fallback,
  admin actions\
- **Listing:** search, pagination, empty states

### Validation

- Yup schema regenerated on language change\
- Localized messages (i18next `articles:*`)

### Localization

- Full EN/BG support\
- Uses `articles.json` namespaces

---

## Routing (Client)

SEO-friendly:

    /articles/{id}/{slug}

- Slug used only client-side\
- Mismatches → navigate to canonical

---

## Limitations (Both Sides)

**Client** - No offline caching\

- View count is vulnerable to refresh spamming\
- Image alt = title

**Server** - View count also unprotected\

- Minimal alt text\
- Soft-delete leaves orphan images until next replacement

---
