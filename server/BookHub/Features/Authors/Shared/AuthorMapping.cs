namespace BookHub.Features.Authors.Shared;

using System.Linq.Expressions;
using Web.User.Models;
using Data.Models;
using Features.Book.Shared;
using Service.Models;

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

    public static Expression<Func<AuthorDbModel, AuthorNamesServiceModel>> ToNamesServiceModelExpression =>
        dbModel => new()
        {
            Id = dbModel.Id,
            Name = dbModel.Name,
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

    public static AuthorServiceModel ToServiceModel(
        this AuthorDbModel dbModel)
        => new()
        {
            Id = dbModel.Id,
            Name = dbModel.Name,
            ImagePath = dbModel.ImagePath,
            Biography = dbModel.Biography,
            BooksCount = dbModel.Books.Count(),
            AverageRating = dbModel.AverageRating,
        };

    public static Expression<Func<AuthorDbModel, AuthorServiceModel>> ToServiceModelExpression =>
        dbModel => new()
        {
            Id = dbModel.Id,
            Name = dbModel.Name,
            ImagePath = dbModel.ImagePath,
            Biography = dbModel.Biography,
            BooksCount = dbModel.Books.Count(),
            AverageRating = dbModel.AverageRating,
        };

    public static AuthorDetailsServiceModel ToDetailsServiceModel(
        this AuthorDbModel dbModel)
        => new()
        {
            Id = dbModel.Id,
            Name = dbModel.Name,
            ImagePath = dbModel.ImagePath,
            Biography = dbModel.Biography,
            BooksCount = dbModel.Books.Count(),
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
                .Select(BookMapping.ToServiceModelExpression)
        };

    public static Expression<Func<AuthorDbModel, AuthorDetailsServiceModel>> ToDetailsServiceModelExpression =>
        dbModel => new()
        {
            Id = dbModel.Id,
            Name = dbModel.Name,
            ImagePath = dbModel.ImagePath,
            Biography = dbModel.Biography,
            BooksCount = dbModel.Books.Count(),
            PenName = dbModel.PenName,
            AverageRating = dbModel.AverageRating,
            Nationality = dbModel.Nationality,
            Gender = dbModel.Gender,
            BornAt = dbModel.BornAt != null ? dbModel.BornAt.ToString() : null,
            DiedAt = dbModel.DiedAt != null ? dbModel.DiedAt.ToString() : null,
            CreatorId = dbModel.CreatorId,
            IsApproved = dbModel.IsApproved,
            TopBooks = dbModel
                .Books
                .Take(3)
                .Select(BookMapping.ToServiceModelExpression),
        };

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

    private static DateTime? StringToDateTime(string? dateTimeString)
    {
        if (string.IsNullOrEmpty(dateTimeString))
        {
            return null;
        }

        if (DateTime.TryParse(dateTimeString, out DateTime result))
        {
            return result;
        }

        return null;
    }

    private static string? DateTimeToString(DateTime? dateTime)
        => dateTime.HasValue ? dateTime.Value.ToString("O") : null;
}
