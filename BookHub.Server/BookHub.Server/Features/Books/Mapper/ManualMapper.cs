namespace BookHub.Server.Features.Books.Mapper
{
    using Authors.Service.Models;
    using Data.Models;
    using Genre.Service.Models;
    using Microsoft.EntityFrameworkCore;
    using Review.Service.Models;
    using Service.Models;

    public static class ManualMapper
    {
        public static IQueryable<BookDetailsServiceModel> MapToDetailsModel(this DbSet<Book> books, string userId)
            => books
                .Select(b => new BookDetailsServiceModel()
                {
                    Id = b.Id,
                    Title = b.Title,
                    AuthorName = b.Author == null ? null : b.Author.Name,
                    ImageUrl = b.ImageUrl,
                    ShortDescription = b.ShortDescription,
                    AverageRating = b.AverageRating,
                    Genres = b
                        .BooksGenres
                        .Select(bg => new GenreNameServiceModel()
                        {
                            Id = bg.GenreId,
                            Name = bg.Genre.Name
                        })
                        .ToHashSet(),
                    PublishedDate = b.PublishedDate == null ? null : b.PublishedDate.ToString(),
                    RatingsCount = b.RatingsCount,
                    LongDescription = b.LongDescription,
                    CreatorId = b.CreatorId,
                    Author = b.Author == null
                        ? null
                        : new AuthorServiceModel()
                        {
                            Id = b.Author.Id,
                            Name = b.Author.Name,
                            ImageUrl = b.Author.ImageUrl,
                            Biography = b.Author.Biography,
                            BooksCount = b.Author.Books.Count(),
                            AverageRating = b.Author.AverageRating,
                        },
                    Reviews = b
                        .Reviews
                        .OrderByDescending(r => r.CreatedOn)
                        .ThenBy(r => r.CreatedBy == userId)
                        .Select(r => new ReviewServiceModel()
                        {
                            Id = r.Id,
                            Content = r.Content,
                            Rating = r.Rating,
                            Likes = r.Likes,
                            Dislikes = r.Dislikes,
                            CreatorId = r.CreatorId,
                            BookId = r.BookId,
                            CreatedBy = r.CreatedBy!,
                            CreatedOn = r.CreatedOn.ToString(),
                            ModifiedOn = r.ModifiedOn == null ? null : r.ModifiedOn.ToString()
                        })
                        .ToHashSet()
                });

        public static IQueryable<BookServiceModel> MapToServiceModel(this DbSet<Book> books)
            => books
                .Select(b => new BookServiceModel()
                {
                    Id = b.Id,
                    Title = b.Title,
                    AuthorName = b.Author == null ? null : b.Author.Name,
                    ImageUrl = b.ImageUrl,
                    ShortDescription = b.ShortDescription,
                    AverageRating = b.AverageRating,
                    Genres = b
                        .BooksGenres
                        .Select(bg => new GenreNameServiceModel()
                        {
                            Id = bg.GenreId,
                            Name = bg.Genre.Name
                        })
                        .ToHashSet()
                });
    }
}
