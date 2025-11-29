namespace BookHub.Features.Book.Mapper
{
    using Authors.Service.Models;
    using Data.Models;
    using Genre.Service.Models;
    using Review.Service.Models;
    using Service.Models;

    public static class ManualMapper
    {
        public static IQueryable<BookDetailsServiceModel> MapToDetailsModel(
            this IQueryable<BookDbModel> books,
            string userId)
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
                       IsApproved = b.IsApproved,
                       Author = b.Author == null
                           ? null
                           : new AuthorServiceModel()
                           {
                               Id = b.Author.Id,
                               Name = b.Author.Name,
                               ImagePath = b.Author.ImagePath,
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
                               CreatorId = r.CreatorId,
                               BookId = r.BookId,
                               CreatedBy = r.CreatedBy!,
                               CreatedOn = r.CreatedOn.ToString(),
                               Upvotes = r.Votes.Where(v => v.IsUpvote).Count(),
                               Downvotes = r.Votes.Where(v => !v.IsUpvote).Count(),
                               ModifiedOn = r.ModifiedOn == null ? null : r.ModifiedOn.ToString()
                           })
                           .Take(5)
                           .ToHashSet(),
                       MoreThanFiveReviews = b.Reviews.Count() > 5,
                       ReadingStatus = b
                            .ReadingLists
                            .Where(rl => rl.BookId == b.Id && rl.UserId == userId)
                            .Select(rl => Convert.ToString(rl.Status))
                            .FirstOrDefault()
                   });

        public static IQueryable<BookServiceModel> ToServiceModel(this IQueryable<BookDbModel> books)
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
