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

        serviceModel.UpdateDbModel(dbModel);

        var willDeleteOld =
            !string.IsNullOrWhiteSpace(oldImagePath) &&
            !string.Equals(
                oldImagePath,
                DefaultImagePath,
                StringComparison.OrdinalIgnoreCase);

        if (serviceModel.RemoveImage)
        {
            dbModel.ImagePath = DefaultImagePath;
        }
        else
        {
            await imageWriter.Write(
                ProfilesImagePathPrefix,
                dbModel,
                serviceModel,
                defaultImagePath: null,
                token: cancellationToken);

            willDeleteOld &= serviceModel.Image is not null;
        }

        await data.SaveChangesAsync(cancellationToken);

        var shouldDeleteOldImage = 
            willDeleteOld &&
            !string.Equals(
                oldImagePath,
                dbModel.ImagePath,
                StringComparison.OrdinalIgnoreCase);

        if (shouldDeleteOldImage)
        {
            var deleted = imageWriter.Delete(
                ProfilesImagePathPrefix,
                oldImagePath,
                DefaultImagePath);

            if (!deleted)
            {
                logger.LogWarning(
                    "Profile updated but old image was not deleted. UserId={UserId}, OldImagePath={OldImagePath}, NewImagePath={NewImagePath}",
                    userId,
                    oldImagePath,
                    dbModel.ImagePath);
            }
        }

        logger.LogInformation(
            "Profile updated. UserId={UserId}, RemoveImage={RemoveImage}, NewImageUploaded={NewImageUploaded}, ImagePath={ImagePath}",
            userId,
            serviceModel.RemoveImage,
            serviceModel.Image is not null,
            dbModel.ImagePath);

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

        var user = await userManager.FindByIdAsync(dbModel.UserId);
        if (user is not null)
        {
            var identityResult = await userManager.DeleteAsync(user);
            if (!identityResult.Succeeded)
            {
                return string.Join(
                    "; ",
                    identityResult.Errors.Select(e => e.Description));
            }

            imageWriter.Delete(
                ProfilesImagePathPrefix,
                imagePathToDelete,
                DefaultImagePath);

            return true;
        }

        return false;
    }

    public async Task<Result> IncrementCreatedBooksCount(
        string userId,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = userService.GetId()!;
        var isNotCurrentUser = currentUserId != userId;

        if (isNotCurrentUser)
        {
            return LogAndReturnUnauthorizedMessage(
                currentUserId,
                userId);
        }

        var rowsAffected = await data
            .Profiles
            .Where(p => p.UserId == userId)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(
                    profile => profile.CreatedBooksCount,
                    profile => profile.CreatedBooksCount + 1),
                cancellationToken);

        return rowsAffected > 0;
    }

    public async Task<Result> IncrementCreatedAuthorsCount(
        string userId,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = userService.GetId()!;
        var isNotCurrentUser = currentUserId != userId;

        if (isNotCurrentUser)
        {
            return LogAndReturnUnauthorizedMessage(
                currentUserId,
                userId);
        }

        var rowsAffected = await data
            .Profiles
            .Where(p => p.UserId == userId)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(
                    profile => profile.CreatedAuthorsCount,
                    profile => profile.CreatedAuthorsCount + 1),
                cancellationToken);

        return rowsAffected > 0;
    }

    public async Task<Result> IncrementWrittenReviewsCount(
        string userId,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = userService.GetId()!;
        var isNotCurrentUser = currentUserId != userId;

        if (isNotCurrentUser)
        {
            return LogAndReturnUnauthorizedMessage(
                currentUserId,
                userId);
        }

        var rowsAffected = await data
            .Profiles
            .Where(p => p.UserId == userId)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(
                    profile => profile.ReviewsCount,
                    profile => profile.ReviewsCount + 1),
                cancellationToken);

        return rowsAffected > 0;
    }

    public async Task<Result> IncrementCurrentlyReadingBooksCount(
        string userId,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = userService.GetId()!;
        var isNotCurrentUser = currentUserId != userId;

        if (isNotCurrentUser)
        {
            return LogAndReturnUnauthorizedMessage(
                currentUserId,
                userId);
        }

        var rowsAffected = await data
            .Profiles
            .Where(p => p.UserId == userId)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(
                    profile => profile.CurrentlyReadingBooksCount,
                    profile => profile.CurrentlyReadingBooksCount + 1),
                cancellationToken);

        return rowsAffected > 0;
    }

    public async Task<Result> IncrementToReadBooksCount(
        string userId,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = userService.GetId()!;
        var isNotCurrentUser = currentUserId != userId;

        if (isNotCurrentUser)
        {
            return LogAndReturnUnauthorizedMessage(
                currentUserId,
                userId);
        }

        var rowsAffected = await data
            .Profiles
            .Where(p => p.UserId == userId)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(
                    profile => profile.ToReadBooksCount,
                    profile => profile.ToReadBooksCount + 1),
                cancellationToken);

        return rowsAffected > 0;
    }

    public async Task<Result> IncrementReadBooksCount(
        string userId,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = userService.GetId()!;
        var isNotCurrentUser = currentUserId != userId;

        if (isNotCurrentUser)
        {
            return LogAndReturnUnauthorizedMessage(
                currentUserId,
                userId);
        }

        var rowsAffected = await data
            .Profiles
            .Where(p => p.UserId == userId)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(
                    profile => profile.ReadBooksCount,
                    profile => profile.ReadBooksCount + 1),
                cancellationToken);

        return rowsAffected > 0;
    }

    public async Task<Result> DecrementCurrentlyReadingBooksCount(
    string userId,
    CancellationToken cancellationToken = default)
    {
        var currentUserId = userService.GetId()!;
        var isNotCurrentUser = currentUserId != userId;

        if (isNotCurrentUser)
        {
            return LogAndReturnUnauthorizedMessage(
                currentUserId,
                userId);
        }

        var rowsAffected = await data
            .Profiles
            .Where(p => p.UserId == userId)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(
                    profile => profile.CurrentlyReadingBooksCount,
                    profile => profile.CurrentlyReadingBooksCount - 1),
                cancellationToken);

        return rowsAffected > 0;
    }

    public async Task<Result> DecrementToReadBooksCount(
        string userId,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = userService.GetId()!;
        var isNotCurrentUser = currentUserId != userId;

        if (isNotCurrentUser)
        {
            return LogAndReturnUnauthorizedMessage(
                currentUserId,
                userId);
        }

        var rowsAffected = await data
            .Profiles
            .Where(p => p.UserId == userId)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(
                    profile => profile.ToReadBooksCount,
                    profile => profile.ToReadBooksCount - 1),
                cancellationToken);

        return rowsAffected > 0;
    }

    public async Task<Result> DecrementReadBooksCount(
        string userId,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = userService.GetId()!;
        var isNotCurrentUser = currentUserId != userId;

        if (isNotCurrentUser)
        {
            return LogAndReturnUnauthorizedMessage(
                currentUserId,
                userId);
        }

        var rowsAffected = await data
            .Profiles
            .Where(p => p.UserId == userId)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(
                    profile => profile.ReadBooksCount,
                    profile => profile.ReadBooksCount - 1),
                cancellationToken);

        return rowsAffected > 0;
    }

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
        string resourceUserId)
    {
        logger.LogWarning(
            UnauthorizedMessageTemplate,
            currentUserId,
            nameof(UserProfile),
            resourceUserId);

        return string.Format(
            UnauthorizedMessage,
            currentUserId,
            nameof(UserProfile),
            resourceUserId);
    }
}
