# Articles Feature - **Server Documentation**

This document covers the **server-side** implementation of the Articles feature in BookHub.

---

## Overview

Articles provide editorial content with:

- Public viewing
- Admin CRUD (create/edit/delete)
- Image upload & validation
- Soft‑delete
- View‑count tracking

---

## Data Model

### `ArticleDbModel`

Located in `Features/Article/Data/Models/ArticleDbModel.cs`.

```csharp
public class ArticleDbModel :
    DeletableEntity<Guid>,
    IImageDdModel
{
    [Required]
    [MaxLength(TitleMaxLength)]
    public string Title { get; set; } = null!;

    [Required]
    [MaxLength(IntroductionMaxLength)]
    public string Introduction { get; set; } = null!;

    [Required]
    [MaxLength(ContentMaxLength)]
    public string Content { get; set; } = null!;

    public string ImagePath { get; set; } = null!;

    public int Views { get; set; }
}
```

### Validation

From `ValidationConstants`:

- **Title:** 10–100 chars
- **Introduction:** 10–500 chars
- **Content:** 100–50,000 chars

### Seeding

`ArticleSeeder.Seed()` seeds example articles for development.

---

## DTOs & Mapping

### Service Models

Located in `Features/Article/Service/Models`.

```csharp
public abstract class ArticleServiceModel
{
    public string Title { get; init; } = null!;
    public string Introduction { get; init; } = null!;
    public string Content { get; init; } = null!;
}
```

`CreateArticleServiceModel` adds an optional `IFormFile Image`.

`ArticleDetailsServiceModel` includes Id, Views, ImagePath, timestamps.

### Web Models

```csharp
public class CreateArticleWebModel
{
    [Required]
    [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
    public string Title { get; init; } = null!;
    ...
    public IFormFile? Image { get; init; }
}
```

### Mapping

Defined in `Features/Article/Shared/Mapping.cs`:

- WebModel → ServiceModel
- ServiceModel → DbModel
- DbModel → DetailsServiceModel

---

## Image Handling

Implemented in `ImageWriter.cs`.

### Rules

- Max **2 MB**
- Allowed extensions: `.jpg`, `.jpeg`, `.png`, `.webp`, `.avif`
- Invalid files → **400 Bad Request**

### Behavior

**Create:**

- Valid image saved under `/wwwroot/images/articles/{guid}.{ext}`
- No image → default path `/images/articles/default.jpg`

**Edit:**

- New image replaces old one
- Old non-default file deleted (best effort)

**Delete:**

- Soft delete only
- Physical files removed only during edit replacement

---

## Service Layer

Located in `ArticlesService.cs`.

### `Details(id, isEditMode, token)`

- If viewing publicly:
  - Increments view count
  - Missing entity → `null` (→ 404)
- Returns `ArticleDetailsServiceModel`

### `Create(model)`

- Maps + validates
- Saves entity
- Handles image
- Returns created details model

### `Edit(id, model)`

- Loads article
- Updates content + image
- Handles deletion of old image
- Saves + logs

### `Delete(id)`

- Loads article
- Soft deletes
- Saves + logs

---

## Web API

### Public Controller

```csharp
[AllowAnonymous]
[HttpGet(Id, Name = DetailsRouteName)]
public async Task<ActionResult<ArticleDetailsServiceModel>> Details(Guid id)
```

- Returns 200 or 404
- Used by frontend for public view

### Admin Controller

Routes:

- `GET /admin/articles/{id}`
- `POST /admin/articles`
- `PUT /admin/articles/{id}`
- `DELETE /admin/articles/{id}`

Authorization enforced via `AdminApiController`.

---

## Known Server Limitations

- View count not protected from multiple refreshes
- Image alt text minimal (title only)

---

## Summary

The server provides:

- Well-validated CRUD pipeline
- Centralized image handling
- Soft-delete
- Clean mapping structure
- Localized & well-structured endpoints
