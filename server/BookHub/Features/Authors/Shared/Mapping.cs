namespace BookHub.Features.Authors.Shared;

using Data.Models;
using Service.Models;
using Web.Models;

public static class Mapping
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
        };

    public static AuthorServiceModel ToServiceModel(
        this AuthorDbModel dbModel)
        => new()
        {
            Id = dbModel.Id,
            Name = dbModel.Name,
            ImagePath = dbModel.ImagePath,
            Biography = dbModel.Biography,
            //BooksCount = dbModel.Books.Count,
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
            //BooksCount = dbModel.Books.Count,
            PenName = dbModel.PenName,
            AverageRating = dbModel.AverageRating,
            Nationality = dbModel.Nationality,
            Gender = dbModel.Gender,
            BornAt = dbModel.BornAt is not null ? dbModel.BornAt.ToString() : null,
            DiedAt = dbModel.DiedAt is not null ? dbModel.DiedAt.ToString() : null,
            CreatorId = dbModel.CreatorId,
            IsApproved = dbModel.IsApproved,
            //TopBooks = dbModel.TopBooks,
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
        BornAt = ParseDateTime(serviceModel.BornAt),
        DiedAt = ParseDateTime(serviceModel.DiedAt)
    };

    private static DateTime? ParseDateTime(string? dateTimeString)
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
}
