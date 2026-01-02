# pragma warning disable CA1873

namespace BookHub.Features.UserProfile.Service;

using BookHub.Data;
using BookHub.Features.Identity.Data.Models;
using Data.Models;
using Infrastructure.Services.CurrentUser;
using Infrastructure.Services.ImageWriter;
using Infrastructure.Services.Result;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using Shared;

using static Common.Constants.ErrorMessages;
using static Shared.Constants.Paths;

public class ProfileService(
    BookHubDbContext data,
    UserManager<UserDbModel> userManager,
    ICurrentUserService userService,
    IImageWriter imageWriter,
    ILogger<ProfileService> logger) : IProfileService
{
    public async Task<IEnumerable<ProfileServiceModel>> TopThree(
        CancellationToken cancellationToken = default)
        => await data
            .Profiles
            .OrderByDescending(p =>
                p.CreatedBooksCount +
                p.CreatedAuthorsCount +
                p.ReviewsCount)
            .Take(3)
            .ToServiceModels()
            .ToListAsync(cancellationToken);

    public async Task<ProfileServiceModel?> Mine(
        CancellationToken cancellationToken = default)
        => await data
            .Profiles
            .ToServiceModels()
            .FirstOrDefaultAsync(
                p => p.Id == userService.GetId(),
                cancellationToken);

    public async Task<IProfileServiceModel?> OtherUser(
        string id,
        CancellationToken cancellationToken = default)
    {
        var dbModel = await data
            .Profiles
            .ToServiceModels()
            .FirstOrDefaultAsync(
                p => p.Id == id,
                cancellationToken);

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

    public async Task<ProfileServiceModel> Create(
        CreateProfileServiceModel serviceModel,
        string userId,
        CancellationToken cancellationToken = default)
    {
        var dbModel = serviceModel.ToDbModel();
        dbModel.UserId = userId;

        await imageWriter.Write(
           ProfilesImagePathPrefix,
           dbModel,
           serviceModel,
           DefaultImagePath,
           cancellationToken);

        data.Add(dbModel);
        await data.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Profile for user with id {UserId} was created.",
            userId);

        return dbModel.ToServiceModel();
    }

    public async Task<Result> Edit(
    CreateProfileServiceModel serviceModel,
    CancellationToken cancellationToken = default)
    {
        var userId = userService.GetId()!;
        var dbModel = await this.GetDbModel(
            userId,
            cancellationToken);

        if (dbModel is null)
        {
            return this.LogAndReturnNotFoundMessage(userId);
        }

        if (dbModel.UserId != userId)
        {
            return this.LogAndReturnUnauthorizedMessage(userId, dbModel.UserId);
        }

        var oldImagePath = dbModel.ImagePath;
        var isNewImageUploaded = serviceModel.Image is not null;
        var shouldRemoveImage = serviceModel.RemoveImage;

        serviceModel.UpdateDbModel(dbModel);

        string? pathToDeleteAfterSave = null;
        if (shouldRemoveImage)
        {
            dbModel.ImagePath = DefaultImagePath;
            pathToDeleteAfterSave = oldImagePath;
        }
        else
        {
            await imageWriter.Write(
                ProfilesImagePathPrefix,
                dbModel,
                serviceModel,
                null,
                cancellationToken);

            if (isNewImageUploaded)
            {
                pathToDeleteAfterSave = oldImagePath;
            }
        }

        await data.SaveChangesAsync(cancellationToken);
        if (!string.IsNullOrWhiteSpace(pathToDeleteAfterSave))
        {
            try
            {
                imageWriter.Delete(
                    ProfilesImagePathPrefix,
                    pathToDeleteAfterSave,
                    DefaultImagePath);
            }
            catch (Exception exception)
            {
                logger.LogWarning(
                    exception,
                    "Profile image delete failed for user {UserId}. Path: {Path}",
                    userId,
                    pathToDeleteAfterSave);
            }
        }

        logger.LogInformation(
            "Profile for user with id {UserId} was updated.",
            userId);

        return true;
    }

    public async Task<Result> Delete(
        string? userToDeleteId = null,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = userService.GetId()!;
        userToDeleteId ??= currentUserId;

        var dbModel = await this.GetDbModel(
            userToDeleteId,
            cancellationToken);

        if (dbModel is null)
        {
            return this.LogAndReturnNotFoundMessage(userToDeleteId);
        }

        var isNotCurrentUserProfile = dbModel.UserId != currentUserId;
        var userIsNotAdmin = !userService.IsAdmin();

        if (isNotCurrentUserProfile && userIsNotAdmin)
        {
            return this.LogAndReturnUnauthorizedMessage(currentUserId, dbModel.UserId);
        }

        var imagePathToDelete = dbModel.ImagePath;

        await using var transaction = await data.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            data.Remove(dbModel);
            await data.SaveChangesAsync(cancellationToken);

            var user = await userManager.FindByIdAsync(dbModel.UserId);
            if (user is not null)
            {
                var identityResult = await userManager.DeleteAsync(user);
                if (!identityResult.Succeeded)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return string.Join("; ", identityResult.Errors.Select(e => e.Description));
                }
            }

            await transaction.CommitAsync(cancellationToken);

            try
            {
                imageWriter.Delete(
                    ProfilesImagePathPrefix,
                    imagePathToDelete,
                    DefaultImagePath);
            }
            catch (Exception exception)
            {
                logger.LogWarning(
                    exception,
                    "Profile image delete failed for user {UserId}. Path: {Path}",
                    userToDeleteId,
                    imagePathToDelete);
            }

            logger.LogInformation(
                "Profile with id: {UserId} was deleted.",
                userToDeleteId);

            return true;
        }
        catch (Exception exception)
        {
            await transaction.RollbackAsync(cancellationToken);

            logger.LogError(
                exception,
                "Error deleting profile/user for {UserId}",
                userToDeleteId);

            return "Delete failed.";
        }
    }

    public async Task IncrementCreatedBooksCount(
        string userId,
        CancellationToken cancellationToken = default)
        => await data
            .Profiles
            .Where(p => p.UserId == userId)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(
                    profile => profile.CreatedBooksCount,
                    profile => profile.CreatedBooksCount + 1),
                cancellationToken);

    public async Task IncrementCreatedAuthorsCount(
        string userId,
        CancellationToken cancellationToken = default)
        => await data
            .Profiles
            .Where(p => p.UserId == userId)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(
                    profile => profile.CreatedAuthorsCount,
                    profile => profile.CreatedAuthorsCount + 1),
                cancellationToken);

    public async Task IncrementWrittenReviewsCount(
        string userId,
        CancellationToken cancellationToken = default)
        => await data
            .Profiles
            .Where(p => p.UserId == userId)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(
                    profile => profile.ReviewsCount,
                    profile => profile.ReviewsCount + 1),
                cancellationToken);

    public async Task IncrementCurrentlyReadingBooksCount(
        string userId,
        CancellationToken cancellationToken = default)
        => await data
            .Profiles
            .Where(p => p.UserId == userId)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(
                    profile => profile.CurrentlyReadingBooksCount,
                    profile => profile.CurrentlyReadingBooksCount + 1),
                cancellationToken);

    public async Task IncrementToReadBooksCount(
        string userId,
        CancellationToken cancellationToken = default)
        => await data
            .Profiles
            .Where(p => p.UserId == userId)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(
                    profile => profile.ToReadBooksCount,
                    profile => profile.ToReadBooksCount + 1),
                cancellationToken);

    public async Task IncrementReadBooksCount(
        string userId,
        CancellationToken cancellationToken = default)
        => await data
            .Profiles
            .Where(p => p.UserId == userId)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(
                    profile => profile.ReadBooksCount,
                    profile => profile.ReadBooksCount + 1),
                cancellationToken);

    public async Task DecrementCurrentlyReadingBooksCount(
        string userId,
        CancellationToken cancellationToken = default)
        => await data
            .Profiles
            .Where(p => p.UserId == userId)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(
                    profile => profile.CurrentlyReadingBooksCount,
                    profile => profile.CurrentlyReadingBooksCount - 1),
                cancellationToken);

    public async Task DecrementToReadBooksCount(
        string userId,
        CancellationToken cancellationToken = default)
         => await data
            .Profiles
            .Where(p => p.UserId == userId)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(
                    profile => profile.ToReadBooksCount,
                    profile => profile.ToReadBooksCount - 1),
                cancellationToken);

    public async Task DecrementReadBooksCount(
        string userId,
        CancellationToken cancellationToken = default)
         => await data
            .Profiles
            .Where(p => p.UserId == userId)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(
                    profile => profile.ReadBooksCount,
                    profile => profile.ReadBooksCount - 1),
                cancellationToken);

    private async Task<UserProfile?> GetDbModel(
        string id,
        CancellationToken cancellationToken = default)
        => await data
            .Profiles
            .FindAsync([id], cancellationToken);

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
