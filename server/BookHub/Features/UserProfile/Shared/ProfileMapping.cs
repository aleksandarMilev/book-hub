namespace BookHub.Features.UserProfile.Shared;

using Data.Models;
using Service.Models;
using Web.Models;

using static BookHub.Common.Utils;

public static class ProfileMapping
{
    public static IQueryable<ProfileServiceModel> ToServiceModels(
        this IQueryable<UserProfile> dbModels)
        => dbModels.Select(p => new ProfileServiceModel
        {
            Id = p.UserId,
            FirstName = p.FirstName,
            LastName = p.LastName,
            ImagePath = p.ImagePath,
            IsPrivate = p.IsPrivate,
            DateOfBirth = p.DateOfBirth,
            SocialMediaUrl = p.SocialMediaUrl,
            Biography = p.Biography,
            CreatedBooksCount = p.CreatedBooksCount,
            CreatedAuthorsCount = p.CreatedAuthorsCount,
            ReadBooksCount = p.ReadBooksCount,
            ReviewsCount = p.ReviewsCount,
            ToReadBooksCount = p.ToReadBooksCount,
            CurrentlyReadingBooksCount = p.CurrentlyReadingBooksCount,
        });

    public static ProfileServiceModel ToServiceModel(
        this UserProfile dbModel)
        => new()
        {
            Id = dbModel.UserId,
            FirstName = dbModel.FirstName,
            LastName = dbModel.LastName,
            ImagePath = dbModel.ImagePath,
            IsPrivate = dbModel.IsPrivate,
            DateOfBirth = dbModel.DateOfBirth,
            SocialMediaUrl = dbModel.SocialMediaUrl,
            Biography = dbModel.Biography,
            CreatedBooksCount = dbModel.CreatedBooksCount,
            CreatedAuthorsCount = dbModel.CreatedAuthorsCount,
            ReadBooksCount = dbModel.ReadBooksCount,
            ReviewsCount = dbModel.ReviewsCount,
            ToReadBooksCount = dbModel.ToReadBooksCount,
            CurrentlyReadingBooksCount = dbModel.CurrentlyReadingBooksCount,
        };

    public static PrivateProfileServiceModel ToPrivateServiceModel(
        this ProfileServiceModel serviceModel)
        => new()
        {
            Id = serviceModel.Id,
            FirstName = serviceModel.FirstName,
            LastName = serviceModel.LastName,
            ImagePath = serviceModel.ImagePath,
            IsPrivate = serviceModel.IsPrivate,
        };

    public static UserProfile ToDbModel(
        this CreateProfileServiceModel serviceModel)
        => new()
        {
            FirstName = serviceModel.FirstName,
            LastName = serviceModel.LastName,
            DateOfBirth = serviceModel.DateOfBirth,
            SocialMediaUrl = serviceModel.SocialMediaUrl,
            Biography = serviceModel.Biography,
            IsPrivate = serviceModel.IsPrivate,
        };

    public static void UpdateDbModel(
        this CreateProfileServiceModel serviceModel,
        UserProfile dbModel)
    {
        dbModel.FirstName = serviceModel.FirstName;
        dbModel.LastName = serviceModel.LastName;
        dbModel.DateOfBirth = serviceModel.DateOfBirth;
        dbModel.SocialMediaUrl = serviceModel.SocialMediaUrl;
        dbModel.Biography = serviceModel.Biography;
        dbModel.IsPrivate = serviceModel.IsPrivate;
    }

    public static CreateProfileServiceModel ToCreateServiceModel(
        this CreateProfileWebModel webModel)
        => new()
        {
            FirstName = webModel.FirstName,
            LastName = webModel.LastName,
            DateOfBirth = webModel.DateOfBirth,
            SocialMediaUrl = webModel.SocialMediaUrl,
            Biography = webModel.Biography,
            IsPrivate = webModel.IsPrivate,
            RemoveImage = webModel.RemoveImage,
            Image = webModel.Image
        };
}
