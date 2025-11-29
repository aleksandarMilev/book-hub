namespace BookHub.Features.Book.Shared;

using System.Linq.Expressions;
using Service.Models;
using Data.Models;

using BookHub.Features.Genre.Shared;

public static class BookMapping
{
    private static readonly string UnknownAuthor = "Unknown";

    public static Func<BookDbModel, BookServiceModel> ToServiceModelExpression =>
        dbModel => new()
        {
            Id = dbModel.Id,
            Title = dbModel.Title,
            AuthorName = dbModel.Author != null ? dbModel.Author.Name : UnknownAuthor,
            ImageUrl = dbModel.ImageUrl,
            ShortDescription = dbModel.ShortDescription,
            AverageRating = dbModel.AverageRating,
            Genres = dbModel
                .BooksGenres
                .Select(GenreMapping.ToNameServiceModelExpression),
        };
}

