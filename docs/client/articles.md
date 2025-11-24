# Articles Feature - **Client Documentation**

This document covers the **client-side** (React) implementation of the Articles feature in BookHub.

---

## Types

Located in `client/src/features/article/types/article.ts`.

```ts
export interface CreateArticle {
  title: string;
  introduction: string;
  content: string;
  image?: File | null | undefined;
}

export interface ArticleDetails {
  id: string;
  views: number;
  title: string;
  introduction: string;
  content: string;
  imagePath: string;
  createdOn: string;
  modifiedOn: string | null;
}
```

Client augments with `readingMinutes`.

---

## API Client

Located in `client/src/features/article/api/api.ts`.

Functions:

- `details(id)`
- `detailsForEdit(id, token)`
- `create(article, token)`
- `edit(id, article, token)`
- `remove(id, token)`

All use shared `processError`.

---

## Hooks

### `useDetails`

- Fetches public/admin details
- Computes reading time:

```ts
const words = data.content.trim().split(/\s+/).length;
const readingMinutes = Math.max(1, Math.round(words / 220));
```

### `useCreate` / `useEdit`

- Sends multipart form
- Shows toast
- Redirects using canonical slug

### `useRemove`

- Two-step confirmation modal
- Deletes then navigates home

### `useDetailsPage`

- Fetches article
- Computes reading time
- Formats timestamps
- Enforces canonical slug route

### `useEditArticlePage`

Loads editable article for admin.

### `useListPage`

Used for search & pagination.

---

## Components

### Create/Edit Flow

- `CreateArticle.tsx`
- `EditArticle.tsx`
- `ArticleForm.tsx`
  - Formik
  - Markdown content entry
  - Localized labels/errors

### Article Details Page

`ArticleDetails.tsx`:

- Breadcrumb
- Title + meta
- Image or placeholder
- Markdown content rendering
- Reading time
- Views count
- Admin edit/delete buttons

### Listing

- `ArticleList.tsx`
- `ArticleListItem.tsx`

Includes pagination, search, empty-state UI.

---

## Validation

### Client Validation

- Yup schema in `articleSchema.ts`
- Dynamically re-generated when language changes
- Localized error messages:
  - `articles:form`
  - `articles:validation`

---

## Localization

Uses i18next strings from:

- `locales/en/articles.json`
- `locales/bg/articles.json`

Namespaces:

- `articles:*`
- sometimes `layout`

---

## Slugs & Routing

Routing format:

```
/articles/{id}/{slug}
```

- Slug only used client-side
- Ensures SEO-friendly URLs
- Redirects to canonical slug when mismatched

Used across list items, details page, and redirects after create/edit.

---

## Known Client Limitations

- No offline caching
- View‑count displayed but not protected
- Images use article title as alt text

---

## Summary

The client provides:

- Complete article CRUD UI
- Localized EN/BG interface
- Markdown rendering
- Canonical slug routing
- Search & pagination
- Toast-based success/error UX
