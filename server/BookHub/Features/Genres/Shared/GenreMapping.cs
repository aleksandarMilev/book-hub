namespace BookHub.Features.Genre.Shared;

using Books.Service.Models;
using Data.Models;
using Service.Models;

public static class GenreMapping
{
    public static IQueryable<GenreNameServiceModel> ToNameServiceModels(
        this IQueryable<GenreDbModel> dbModels)
        => dbModels.Select(g => new GenreNameServiceModel 
        {
            Id = g.Id,
            Name = g.Name
        });

    public static IQueryable<GenreDetailsServiceModel> ToDetailsServiceModels(
        this IQueryable<GenreDbModel> dbModels)
        => dbModels.Select(g => new GenreDetailsServiceModel
        {
            Id = g.Id,
            Name = g.Name,
            Description = g.Description,
            ImagePath = g.ImagePath,
            TopBooks = g
                .BooksGenres
                .Select(bg => new BookServiceModel()
                {
                    Id = bg.Book.Id,
                    Title = bg.Book.Title,
                    AuthorName = bg.Book.Author == null ? null : bg.Book.Author.Name,
                    ImagePath = bg.Book.ImagePath,
                    ShortDescription = bg.Book.ShortDescription,
                    AverageRating = bg.Book.AverageRating,
                    Genres = bg.Book
                        .BooksGenres
                        .Select(bg => new GenreNameServiceModel
                        {
                            Id = bg.Genre.Id,
                            Name = bg.Genre.Name
                        })
                        .ToHashSet()
                })
                .OrderByDescending(b => b.AverageRating)
                .Take(3)
                .ToHashSet()
        });
}

