namespace BookHub.Features.ReadingList.Shared;

using Books.Service.Models;
using Genre.Service.Models;
using ReadingList.Data.Models;
using ReadingList.Service.Models;
using ReadingList.Web.Models;

public static class ReadingListMapping
{
    public static ReadingListServiceModel ToServiceModel(
        this ReadingListWebModel webModel)
        => new()
        {
            BookId = webModel.BookId,
            Status = webModel.Status,
        };

    public static IQueryable<BookServiceModel> ToBookServiceModels(
        this IQueryable<ReadingList> readingLists)
        => readingLists
            .Select(rl => new BookServiceModel()
            {
                Id = rl.Book.Id,
                Title = rl.Book.Title,
                AuthorName = rl.Book.Author == null ? null : rl.Book.Author.Name,
                ImagePath = rl.Book.ImagePath,
                ShortDescription = rl.Book.ShortDescription,
                AverageRating = rl.Book.AverageRating,
                Genres = rl.Book
                    .BooksGenres
                    .Select(bg => new GenreNameServiceModel
                    {
                        Id = bg.GenreId,
                        Name = bg.Genre.Name
                    })
                    .ToHashSet()
            });
}
