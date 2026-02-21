namespace BookHub.Features.UserProfile.Service;

using BookHub.Data;
using Data.Models;
using Identity.Data.Models;
using Infrastructure.Services.CurrentUser;
using Infrastructure.Services.ImageWriter;
using Infrastructure.Services.Result;
using Infrastructure.Services.StringSanitizer;
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
    IStringSanitizerService stringSanitizer,
    ILogger<ProfileService> logger) : IProfileService
{
    public async Task<IEnumerable<ProfileServiceModel>> TopThree(
        CancellationToken cancellationToken = default)
        => await data
            .Profiles
            .AsNoTracking()
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
            .AsNoTracking()
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
            .AsNoTracking()
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
                cancellationToken);

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

        var profile = await this.GetDbModel(
            userToDeleteId,
            cancellationToken);

        if (profile is null)
        {
            return this.LogAndReturnNotFoundMessage(userToDeleteId);
        }

        var isNotCurrentUserProfile = profile.UserId != currentUserId;
        var userIsNotAdmin = !userService.IsAdmin();

        if (isNotCurrentUserProfile && userIsNotAdmin)
        {
            return this.LogAndReturnUnauthorizedMessage(
                currentUserId,
                profile.UserId);
        }

        data.Profiles.Remove(profile);

        var user = await userManager.FindByIdAsync(profile.UserId);
        if (user is null)
        {
            return false;
        }

        var identityResult = await userManager.DeleteAsync(user);
        if (!identityResult.Succeeded)
        {
            return string.Join("; ", identityResult.Errors.Select(e => e.Description));
        }

        return true;
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
        var sanitizedId = stringSanitizer.SanitizeStringForLog(id);

        logger.LogWarning(
            DbEntityNotFoundTemplate,
            nameof(UserProfile),
            sanitizedId);

        return string.Format(
            DbEntityNotFound,
            nameof(UserProfile),
            sanitizedId);
    }

    private string LogAndReturnUnauthorizedMessage(
        string currentUserId,
        string resourceUserId)
    {
        var sanitizedCurrentUserId = stringSanitizer.SanitizeStringForLog(currentUserId);
        var sanitizedResourceUserId = stringSanitizer.SanitizeStringForLog(resourceUserId);

        logger.LogWarning(
            UnauthorizedMessageTemplate,
            sanitizedCurrentUserId,
            nameof(UserProfile),
            sanitizedResourceUserId);

        return string.Format(
            UnauthorizedMessage,
            sanitizedCurrentUserId,
            nameof(UserProfile),
            sanitizedResourceUserId);
    }
}
