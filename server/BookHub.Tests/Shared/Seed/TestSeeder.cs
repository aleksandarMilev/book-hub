namespace BookHub.Tests.Shared.Seed;

using BookHub.Data;
using BookHub.Data.Models.Shared.BookGenre.Models;
using Features.Authors.Data.Models;
using Features.Authors.Shared;
using Features.Books.Data.Models;
using Features.Genres.Data.Models;
using Features.Identity.Data.Models;
using Features.Notifications.Data.Models;
using Features.Notifications.Shared;
using Features.Reviews.Data.Models;
using Features.UserProfile.Data.Models;
using Microsoft.EntityFrameworkCore;

// Seed helpers for integration tests. Each one writes through the DbContext (so the audit
// fields and soft-delete behavior apply), saves, and returns the entity. Pass the context
// from BookHubWebApplicationFactory.WithData or from a TestDatabase.
public static class TestSeeder
{
    // The "Other" genre is seeded by the initial migration, so every test database has it.
    public static readonly Guid OtherGenreId = new("52e607d4-c347-440a-8d55-cf2e01d88a6c");

    public static async Task<UserDbModel> SeedUser(
        this BookHubDbContext data,
        string id,
        string username,
        string? email = null)
    {
        var existing = await data
            .Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Id == id);

        if (existing is not null)
        {
            return existing;
        }

        var actualEmail = email ?? $"{username}@test.local";
        var user = new UserDbModel
        {
            Id = id,
            UserName = username,
            NormalizedUserName = username.ToUpperInvariant(),
            Email = actualEmail,
            NormalizedEmail = actualEmail.ToUpperInvariant(),
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("N"),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
        };

        data.Users.Add(user);
        await data.SaveChangesAsync();

        return user;
    }

    public static async Task<UserProfile> SeedProfile(
        this BookHubDbContext data,
        string userId,
        string firstName = "First",
        string lastName = "Last",
        bool isPrivate = false)
    {
        await data.SeedUser(userId, $"user-{userId}");

        var profile = new UserProfile
        {
            UserId = userId,
            FirstName = firstName,
            LastName = lastName,
            ImagePath = "/images/profiles/test.jpg",
            DateOfBirth = new DateTime(2000, 1, 1),
            IsPrivate = isPrivate,
        };

        data.Profiles.Add(profile);
        await data.SaveChangesAsync();

        return profile;
    }

    public static async Task<GenreDbModel> SeedGenre(
        this BookHubDbContext data,
        string name,
        Guid? id = null)
    {
        var genre = new GenreDbModel
        {
            Id = id ?? Guid.NewGuid(),
            Name = name,
            ImagePath = "/images/genres/test.jpg",
            Description = "A genre description that is long enough.",
        };

        data.Genres.Add(genre);
        await data.SaveChangesAsync();

        return genre;
    }

    public static async Task<AuthorDbModel> SeedAuthor(
        this BookHubDbContext data,
        string name,
        string? penName = null,
        bool isApproved = true,
        string? creatorId = null,
        double averageRating = 0)
    {
        var author = new AuthorDbModel
        {
            Id = Guid.NewGuid(),
            Name = name,
            PenName = penName,
            Biography = new string('b', 120),
            Gender = Gender.Other,
            Nationality = Nationality.Bulgaria,
            ImagePath = "/images/authors/test.jpg",
            IsApproved = isApproved,
            CreatorId = creatorId,
            AverageRating = averageRating,
        };

        data.Authors.Add(author);
        await data.SaveChangesAsync();

        return author;
    }

    public static async Task<BookDbModel> SeedBook(
        this BookHubDbContext data,
        string title,
        string shortDescription = "A valid short description",
        bool isApproved = true,
        string? creatorId = null,
        Guid? authorId = null,
        double averageRating = 0,
        params Guid[] genreIds)
    {
        var book = new BookDbModel
        {
            Id = Guid.NewGuid(),
            Title = title,
            ShortDescription = shortDescription,
            LongDescription = new string('l', 200),
            ImagePath = "/images/books/test.jpg",
            AverageRating = averageRating,
            IsApproved = isApproved,
            CreatorId = creatorId,
            AuthorId = authorId,
        };

        foreach (var genreId in genreIds.Length == 0 ? [OtherGenreId] : genreIds.Distinct())
        {
            book.BooksGenres.Add(new BookGenreDbModel
            {
                BookId = book.Id,
                GenreId = genreId,
            });
        }

        data.Books.Add(book);
        await data.SaveChangesAsync();

        return book;
    }

    // Seeds the review row only: the book's and author's rating counters are left as they are.
    public static async Task<ReviewDbModel> SeedReview(
        this BookHubDbContext data,
        Guid bookId,
        string creatorId,
        int rating = 4)
    {
        await data.SeedUser(creatorId, $"user-{creatorId}");

        var review = new ReviewDbModel
        {
            Id = Guid.NewGuid(),
            Content = "A seeded review",
            Rating = rating,
            BookId = bookId,
            CreatorId = creatorId,
        };

        data.Reviews.Add(review);
        await data.SaveChangesAsync();

        return review;
    }

    public static async Task<NotificationDbModel> SeedNotification(
        this BookHubDbContext data,
        string receiverId,
        string message = "A seeded notification")
    {
        await data.SeedUser(receiverId, $"user-{receiverId}");

        var notification = new NotificationDbModel
        {
            Id = Guid.NewGuid(),
            Message = message,
            ReceiverId = receiverId,
            ResourceId = Guid.NewGuid(),
            ResourceType = ResourceType.Book,
        };

        data.Notifications.Add(notification);
        await data.SaveChangesAsync();

        return notification;
    }
}
