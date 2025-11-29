namespace BookHub.Features.Genre.Shared;

using System.Linq.Expressions;
using Service.Models;
using BookHub.Data.Models.Shared.BookGenre;

public static class GenreMapping
{
    public static Func<BookGenre, GenreNameServiceModel> ToNameServiceModelExpression =>
        dbModel => new()
        {
            Id = dbModel.GenreId,
            Name = dbModel.Genre.Name
        };
}

