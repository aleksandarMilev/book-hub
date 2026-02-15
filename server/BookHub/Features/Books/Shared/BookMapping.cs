namespace BookHub.Features.Books.Shared;

using Authors.Service.Models;
using Data.Models;
using Genres.Service.Models;
using Reviews.Service.Models;
using Service.Models;
using System.Text.Json;
using Web.User.Models;

using static BookHub.Common.Utils;

public static class BookMapping
{
    public static IQueryable<BookDetailsServiceModel> ToDetailsServiceModels(
        this IQueryable<BookDbModel> books,
        string userId)
           => books
               .Select(b => new BookDetailsServiceModel()
               {
                   Id = b.Id,
                   Title = b.Title,
                   AuthorName = b.Author != null ? b.Author.Name : null,
                   ImagePath = b.ImagePath,
                   ShortDescription = b.ShortDescription,
                   AverageRating = b.AverageRating,
                   Genres = b
                       .BooksGenres
                       .Where(bg => bg.Genre != null)
                       .Select(bg => new GenreNameServiceModel
                       {
                           Id = bg.GenreId,
                           Name = bg.Genre.Name
                       })
                       .ToHashSet(),
                   PublishedDate = DateTimeToString(b.PublishedDate),
                   RatingsCount = b.RatingsCount,
                   LongDescription = b.LongDescription,
                   Pages = b.Pages,
                   CreatorId = b.CreatorId,
                   IsApproved = b.IsApproved,
                   Author = b.Author == null
                       ? null
                       : new AuthorServiceModel
                       {
                           Id = b.Author.Id,
                           Name = b.Author.Name!,
                           ImagePath = b.Author.ImagePath,
                           Biography = b.Author.Biography,
                           BooksCount = b.Author.Books.Count,
                           AverageRating = b.Author.AverageRating,
                       },
                   Reviews = b
                       .Reviews
                       .OrderByDescending(r => r.CreatedOn)
                       .ThenBy(r => r.CreatedBy == userId)
                       .Select(r => new ReviewServiceModel
                       {
                           Id = r.Id,
                           Content = r.Content,
                           Rating = r.Rating,
                           CreatorId = r.CreatorId,
                           BookId = r.BookId,
                           CreatedBy = r.CreatedBy!,
                           CreatedOn = r.CreatedOn.ToString(),
                           Upvotes = r.Votes.Where(v => v.IsUpvote).Count(),
                           Downvotes = r.Votes.Where(v => !v.IsUpvote).Count(),
                           ModifiedOn = DateTimeToString(r.ModifiedOn)
                       })
                       .Take(5)
                       .ToHashSet(),
                   MoreThanFiveReviews = b.Reviews.Count > 5,
                   ReadingStatus = b
                        .ReadingLists
                        .Where(rl => rl.BookId == b.Id && rl.UserId == userId)
                        .Select(rl => Convert.ToString(rl.Status))
                        .FirstOrDefault()
               });

    public static BookDetailsServiceModel ToDetailsServiceModel(
        this BookDbModel dbModel,
        string userId)
           => new()
           {
               Id = dbModel.Id,
               Title = dbModel.Title,
               AuthorName = dbModel.Author?.Name,
               ImagePath = dbModel.ImagePath,
               ShortDescription = dbModel.ShortDescription,
               AverageRating = dbModel.AverageRating,
               Genres = dbModel
                    .BooksGenres
                    .Select(bg => new GenreNameServiceModel
                    {
                        Id = bg.GenreId,
                        Name = bg.Genre.Name
                    })
                    .ToHashSet(),
               PublishedDate = DateTimeToString(dbModel.PublishedDate),
               RatingsCount = dbModel.RatingsCount,
               LongDescription = dbModel.LongDescription,
               Pages = dbModel.Pages,
               CreatorId = dbModel.CreatorId,
               IsApproved = dbModel.IsApproved,
               Author = dbModel.Author == null
                    ? null
                    : new AuthorServiceModel
                    {
                        Id = dbModel.Author.Id,
                        Name = dbModel.Author.Name!,
                        ImagePath = dbModel.Author.ImagePath,
                        Biography = dbModel.Author.Biography,
                        BooksCount = dbModel.Author.Books.Count,
                        AverageRating = dbModel.Author.AverageRating,
                    },
               Reviews = dbModel
                    .Reviews
                    .OrderByDescending(r => r.CreatedOn)
                    .ThenBy(r => r.CreatedBy == userId)
                    .Select(r => new ReviewServiceModel
                    {
                        Id = r.Id,
                        Content = r.Content,
                        Rating = r.Rating,
                        CreatorId = r.CreatorId,
                        BookId = r.BookId,
                        CreatedBy = r.CreatedBy!,
                        CreatedOn = r.CreatedOn.ToString(),
                        Upvotes = r.Votes.Where(v => v.IsUpvote).Count(),
                        Downvotes = r.Votes.Where(v => !v.IsUpvote).Count(),
                        ModifiedOn = DateTimeToString(r.ModifiedOn)
                    })
                    .Take(5)
                    .ToHashSet(),
               MoreThanFiveReviews = dbModel.Reviews.Count > 5,
               ReadingStatus = dbModel
                    .ReadingLists
                    .Where(rl => rl.BookId == dbModel.Id && rl.UserId == userId)
                    .Select(rl => Convert.ToString(rl.Status))
                    .FirstOrDefault()
           };

    public static IQueryable<BookServiceModel> ToServiceModels(
        this IQueryable<BookDbModel> books)
        => books
            .Select(b => new BookServiceModel()
            {
                Id = b.Id,
                Title = b.Title,
                AuthorName = b.Author == null ? null : b.Author.Name,
                ImagePath = b.ImagePath,
                ShortDescription = b.ShortDescription,
                AverageRating = b.AverageRating,
                Genres = b
                    .BooksGenres
                    .Select(bg => new GenreNameServiceModel
                    {
                        Id = bg.GenreId,
                        Name = bg.Genre.Name
                    })
                    .ToHashSet()
            });

    public static CreateBookServiceModel ToCreateServiceModel(
        this CreateBookWebModel webModel)
        => new()
        {
            Title = webModel.Title,
            Image = webModel.Image,
            AuthorId = webModel.AuthorId,
            ShortDescription = webModel.ShortDescription,
            LongDescription = webModel.LongDescription,
            Pages = webModel.Pages,
            PublishedDate = webModel.PublishedDate,
            Genres = webModel.Genres
        };

    public static BookDbModel ToDbModel(
        this CreateBookServiceModel serviceModel)
        => new()
        {
            Title = serviceModel.Title,
            ShortDescription = serviceModel.ShortDescription,
            LongDescription = serviceModel.LongDescription,
            Pages = serviceModel.Pages,
            PublishedDate = serviceModel.PublishedDate,
        };

    public static void UpdateDbModel(
        this CreateBookServiceModel serviceModel,
        BookDbModel dbModel)
        {
            dbModel.Title = serviceModel.Title;
            dbModel.ShortDescription = serviceModel.ShortDescription;
            dbModel.LongDescription = serviceModel.LongDescription;
            dbModel.Pages = serviceModel.Pages;
            dbModel.PublishedDate = serviceModel.PublishedDate;
        }

    public static void UpdatePendingDbModel(
        this CreateBookServiceModel serviceModel,
        BookEditDbModel pendingDbModel)
    {
        pendingDbModel.Title = serviceModel.Title;
        pendingDbModel.ShortDescription = serviceModel.ShortDescription;
        pendingDbModel.LongDescription = serviceModel.LongDescription;
        pendingDbModel.Pages = serviceModel.Pages;
        pendingDbModel.PublishedDate = serviceModel.PublishedDate;
        pendingDbModel.AuthorId = serviceModel.AuthorId;

        var genreIds = (serviceModel.Genres ?? [])
            .Where(id => id != Guid.Empty)
            .Distinct()
            .ToList();

        pendingDbModel.GenresJson = JsonSerializer.Serialize(genreIds);
    }

    public static void ApplyPendingToBook(
        this BookEditDbModel pendingDbModel,
        BookDbModel dbModel)
    {
        dbModel.Title = pendingDbModel.Title;
        dbModel.ShortDescription = pendingDbModel.ShortDescription;
        dbModel.LongDescription = pendingDbModel.LongDescription;
        dbModel.Pages = pendingDbModel.Pages;
        dbModel.PublishedDate = pendingDbModel.PublishedDate;
        dbModel.ImagePath = pendingDbModel.ImagePath;
        dbModel.AuthorId = pendingDbModel.AuthorId;
    }

    public static ICollection<Guid> GetPendingGenres(
        this BookEditDbModel pendingDbModel)
    {
        try
        {
            return JsonSerializer
                    .Deserialize<ICollection<Guid>>(pendingDbModel.GenresJson)
                    ?? new HashSet<Guid>();
        }
        catch
        {
            return new HashSet<Guid>();
        }
    }
}
