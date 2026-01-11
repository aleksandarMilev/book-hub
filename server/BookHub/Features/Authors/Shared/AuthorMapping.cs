namespace BookHub.Features.Authors.Shared;

using Books.Service.Models;
using Data.Models;
using Genre.Service.Models;
using Service.Models;
using Web.User.Models;

using static Common.Utils;

public static class AuthorMapping
{
    public static CreateAuthorServiceModel ToCreateServiceModel(
        this CreateAuthorWebModel webModel)
        => new()
        {
            Name = webModel.Name,
            Image = webModel.Image,
            Biography = webModel.Biography,
            PenName = webModel.PenName,
            Gender = webModel.Gender,
            BornAt = webModel.BornAt,
            DiedAt = webModel.DiedAt,
            Nationality = webModel.Nationality
        };

    public static IQueryable<AuthorNamesServiceModel> ToNamesServiceModels(
        this IQueryable<AuthorDbModel> authors)
        => authors.Select(a => new AuthorNamesServiceModel 
        {
            Id = a.Id,
            Name = a.Name,
        });

    public static AuthorServiceModel ToServiceModel(
        this AuthorDbModel dbModel)
        => new()
        {
            Id = dbModel.Id,
            Name = dbModel.Name,
            ImagePath = dbModel.ImagePath,
            Biography = dbModel.Biography,
            BooksCount = dbModel.Books.Count,
            AverageRating = dbModel.AverageRating,
        };

    public static IQueryable<AuthorServiceModel> ToServiceModels(
        this IQueryable<AuthorDbModel> authors)
        => authors.Select(a => new AuthorServiceModel
        {
            Id = a.Id,
            Name = a.Name,
            ImagePath = a.ImagePath,
            Biography = a.Biography,
            BooksCount = a.Books.Count,
            AverageRating = a.AverageRating,
        });

    public static AuthorDetailsServiceModel ToDetailsServiceModel(
        this AuthorDbModel dbModel)
        => new()
        {
            Id = dbModel.Id,
            Name = dbModel.Name,
            ImagePath = dbModel.ImagePath,
            Biography = dbModel.Biography,
            BooksCount = dbModel.Books.Count,
            PenName = dbModel.PenName,
            AverageRating = dbModel.AverageRating,
            Nationality = dbModel.Nationality,
            Gender = dbModel.Gender,
            BornAt = DateTimeToString(dbModel.BornAt),
            DiedAt = DateTimeToString(dbModel.DiedAt),
            CreatorId = dbModel.CreatorId,
            IsApproved = dbModel.IsApproved,
            TopBooks = dbModel
                .Books
                .Take(3)
                .Select(b => new BookServiceModel
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
                })
                .ToList(),
        };

    public static IQueryable<AuthorDetailsServiceModel> ToDetailsServiceModels(
        this IQueryable<AuthorDbModel> authors)
        => authors.Select(a => new AuthorDetailsServiceModel
        {
            Id = a.Id,
            Name = a.Name,
            ImagePath = a.ImagePath,
            Biography = a.Biography,
            BooksCount = a.Books.Count,
            PenName = a.PenName,
            AverageRating = a.AverageRating,
            Nationality = a.Nationality,
            Gender = a.Gender,
            BornAt = DateTimeToString(a.BornAt),
            DiedAt = DateTimeToString(a.DiedAt),
            CreatorId = a.CreatorId,
            IsApproved = a.IsApproved,
            TopBooks = a
                .Books
                .Take(3)
                .Select(b => new BookServiceModel
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
                })
                .ToList(),
        });

    public static AuthorDbModel ToDbModel(
        this CreateAuthorServiceModel serviceModel)
        => new()
    {
        Name = serviceModel.Name,
        Biography = serviceModel.Biography,
        PenName = serviceModel.PenName,
        Nationality = serviceModel.Nationality,
        Gender = serviceModel.Gender,
        BornAt = StringToDateTime(serviceModel.BornAt),
        DiedAt = StringToDateTime(serviceModel.DiedAt)
    };

    public static void UpdateDbModel(
        this CreateAuthorServiceModel serviceModel,
        AuthorDbModel dbModel)
    {
        dbModel.Name = serviceModel.Name;
        dbModel.Biography = serviceModel.Biography;
        dbModel.PenName = serviceModel.PenName;
        dbModel.Nationality = serviceModel.Nationality;
        dbModel.Gender = serviceModel.Gender;
        dbModel.BornAt = StringToDateTime(serviceModel.BornAt);
        dbModel.DiedAt = StringToDateTime(serviceModel.DiedAt);
    }
}
