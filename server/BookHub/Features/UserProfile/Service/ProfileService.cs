namespace BookHub.Features.UserProfile.Service;

using System.Diagnostics;
using BookHub.Data;
using Data.Models;
using Infrastructure.Services.CurrentUser;
using Infrastructure.Services.ImageWriter;
using Infrastructure.Services.Result;
using Microsoft.EntityFrameworkCore;
using Models;
using Shared;

using static Common.Constants.ErrorMessages;
using static Shared.Constants.Validation;
using static Shared.Constants.Paths;
public class ProfileService(
    BookHubDbContext data,
    ICurrentUserService userService,
    IImageWriter imageWriter,
    ILogger<ProfileService> logger) : IProfileService
{
    public async Task<IEnumerable<ProfileServiceModel>> TopThree(
        CancellationToken token = default)
        => await data
            .Profiles
            .OrderByDescending(p =>
                p.CreatedBooksCount +
                p.CreatedAuthorsCount +
                p.ReviewsCount)
            .Take(3)
            .ToServiceModels()
            .ToListAsync(token);

    public async Task<ProfileServiceModel?> Mine(
        CancellationToken token = default)
        => await data
            .Profiles
            .ToServiceModels()
            .FirstOrDefaultAsync(
                p => p.Id == userService.GetId(),
                token);

    public async Task<IProfileServiceModel?> OtherUser(
        string id,
        CancellationToken token = default)
    {
        var dbModel = await data
            .Profiles
            .ToServiceModels()
            .FirstOrDefaultAsync(
                p => p.Id == id,
                token);

        if (dbModel is null)
        {
            return null;
        }

        if (dbModel.IsPrivate)
        {
            return dbModel.ToPrivateServiceModel();
        }

        return dbModel;
    }

    public async Task<bool> HasProfile(
        CancellationToken token = default)
         => await data
            .Profiles
            .AsNoTracking()
            .AnyAsync(
                p => p.UserId == userService.GetId(),
                token);

    public async Task<bool> HasMoreThanFiveCurrentlyReading(
        string userId,
        CancellationToken token = default)
        => await data
            .Profiles
            .Where(p => p.UserId == userId)
            .Select(p => p.CurrentlyReadingBooksCount)
            .FirstOrDefaultAsync(token) == CurrentlyReadingBooksMaxCount;
     
    public async Task<ProfileServiceModel> Create(
        CreateProfileServiceModel serviceModel,
        CancellationToken token = default)
    {
        var userId = userService.GetId()!;
        var dbModel = serviceModel.ToDbModel();
        dbModel.UserId = userId;

        await imageWriter.Write(
           ProfilesImagePathPrefix,
           dbModel,
           serviceModel,
           DefaultImagePath,
           token);

        data.Add(dbModel);
        await data.SaveChangesAsync(token);

        logger.LogInformation(
            "Profile for user with id {UserId} was created.",
            userId);

        return dbModel.ToServiceModel();
    }

    public async Task<Result> Edit(
        CreateProfileServiceModel serviceModel,
        CancellationToken token = default)
    {
        var userId = userService.GetId()!;
        var dbModel = await this.GetDbModel(userId, token);

        if (dbModel is null)
        {
            return this.LogAndReturnNotFoundMessage(userId);
        }

        var isNotCurrentUserProfile = dbModel.UserId != userId;
        if (isNotCurrentUserProfile)
        {
            return this.LogAndReturnUnauthorizedMessage(
                userId,
                dbModel.UserId);
        }

        var oldImagePath = dbModel.ImagePath;
        var isNewImageUploaded = serviceModel.Image is not null;

        serviceModel.UpdateDbModel(dbModel);

        await imageWriter.Write(
            ProfilesImagePathPrefix,
            dbModel,
            serviceModel,
            null,
            token);

        if (isNewImageUploaded)
        {
            imageWriter.Delete(
                nameof(UserProfile),
                oldImagePath,
                DefaultImagePath);
        }

        await data.SaveChangesAsync(token);

        logger.LogInformation(
            "Profile for user with id {UserId} was updated.",
            userId);

        return true;
    }

    public async Task<Result> Delete(
        string? userToDeleteId = null,
        CancellationToken token = default)
    {
        var currentUserId = userService.GetId()!;
        userToDeleteId ??= currentUserId;

        var dbModel = await this.GetDbModel(userToDeleteId, token);
        if (dbModel is null)
        {
            return this.LogAndReturnNotFoundMessage(userToDeleteId);
        }

        var isNotCurrentUserProfile = dbModel.UserId != currentUserId;
        var userIsNotAdmin = !userService.IsAdmin();

        if (isNotCurrentUserProfile && userIsNotAdmin)
        {
            return this.LogAndReturnUnauthorizedMessage(
                currentUserId,
                dbModel.UserId);
        }

        data.Remove(dbModel);
        await data.SaveChangesAsync(token);

        logger.LogInformation(
            "Profile with id: {UserId} was deleted.",
            userToDeleteId);

        return true;
    }

    public async Task<Result> UpdateCount(
        string userId,
        string propName,
        Func<int, int> updateFunc,
        CancellationToken token = default)
    {
        var profile = await this.GetDbModel(userId, token);
        if (profile is null)
        {
            return this.LogAndReturnNotFoundMessage(userId);
        }

        var property = typeof(UserProfile).GetProperty(propName);
        if (property is null)
        {
            var methodInfo = new StackTrace().GetFrame(1)?.GetMethod();
            var className = methodInfo?.ReflectedType?.Name;

            logger.LogWarning(
                "{Class}.{Method}() attempted to update property \"${Property}\" on UserProfile with Id: {UserId} but such property does not exist.",
                className,
                methodInfo,
                propName,
                userId);

            return string.Format(
                "{0}.{1}() attempted to update property \"${2}\" on UserProfile with Id: {3} but such property does not exist.",
                className,
                methodInfo,
                propName,
                userId);
        }

        var currentValue = (int)property.GetValue(profile)!;
        var updatedValue = updateFunc(currentValue);

        property.SetValue(profile, updatedValue);

        await data.SaveChangesAsync(token);

        logger.LogInformation(
            "\"${Property}\" on UserProfile with Id: {UserId} was updated successfully.",
                propName,
                userId);

        return true;
    }

    private async Task<UserProfile?> GetDbModel(
        string id,
        CancellationToken token = default)
        => await data
            .Profiles
            .FindAsync([id], token);

    private string LogAndReturnNotFoundMessage(string id)
    {
        logger.LogWarning(
            DbEntityNotFoundTemplate,
            nameof(UserProfile),
            id);

        return string.Format(
            DbEntityNotFound,
            nameof(UserProfile),
            id);
    }

    private string LogAndReturnUnauthorizedMessage(
        string currentUserId,
        string userToDeleteId)
    {
        logger.LogWarning(
            UnauthorizedMessageTemplate,
            currentUserId,
            nameof(UserProfile),
            userToDeleteId);

        return string.Format(
            UnauthorizedMessage,
            currentUserId,
            nameof(UserProfile),
            userToDeleteId);
    }
}
