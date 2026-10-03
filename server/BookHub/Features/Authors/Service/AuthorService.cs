namespace BookHub.Features.Authors.Service;

using Areas.Admin.Service;
using BookHub.Data;
using Data.Models;
using Infrastructure.Extensions;
using Infrastructure.Services.CurrentUser;
using Infrastructure.Services.ImageWriter;
using Infrastructure.Services.Result;
using Microsoft.EntityFrameworkCore;
using Models;
using Notifications.Service;
using Shared;
using UserProfile.Service;

using static Common.Constants.ErrorMessages;
using static Common.Utils;
using static Shared.AuthorMapping;
using static Shared.Constants.ErrorMessages;
using static Shared.Constants.Paths;

public class AuthorService(
    BookHubDbContext data,
    ICurrentUserService userService,
    IAdminService adminService,
    IProfileService profileService,
    INotificationService notificationService,
    IImageWriter imageWriter,
    ILogger<AuthorService> logger) : IAuthorService
{
    private const string ResourceName = "author";

    public async Task<IEnumerable<AuthorNamesServiceModel>> Names(
        CancellationToken cancellationToken = default)
        => await data
            .Authors
            .AsNoTracking()
            .ToNamesServiceModels()
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<AuthorServiceModel>> TopThree(
        CancellationToken cancellationToken = default)
        => await data
            .Authors
            .AsNoTracking()
            .ToServiceModels()
            .OrderByDescending(a => a.AverageRating)
            .Take(3)
            .ToListAsync(cancellationToken);

    public async Task<AuthorDetailsServiceModel?> Details(
        Guid authorId,
        CancellationToken cancellationToken = default)
    {
        var authorDetails = await data
            .Authors
            .AsNoTracking()
            .ToDetailsServiceModels()
            .FirstOrDefaultAsync(
                a => a.Id == authorId,
                cancellationToken);

        if (authorDetails is null)
        {
            return null;
        }

        var currentUserId = userService.GetId();
        var isCreator = string.Equals(
            authorDetails.CreatorId,
            currentUserId,
            StringComparison.OrdinalIgnoreCase);

        var isAdmin = userService.IsAdmin();
        if (!isCreator && !isAdmin)
        {
            return authorDetails;
        }

        var pending = await data
            .AuthorEdits
            .AsNoTracking()
            .FirstOrDefaultAsync(
                e => e.AuthorId == authorId,
                cancellationToken);

        if (pending is null)
        {
            return authorDetails;
        }

        return ApplyPendingEdit(authorDetails, pending);
    }

    public async Task<AuthorDetailsServiceModel?> AdminDetails(
        Guid authorId,
        CancellationToken cancellationToken = default)
    {
        var authorDetails = await data
            .Authors
            .AsNoTracking()
            .IgnoreQueryFilters()
            .ApplyIsDeletedFilter()
            .ToDetailsServiceModels()
            .FirstOrDefaultAsync(
                a => a.Id == authorId,
                cancellationToken);

        if (authorDetails is null)
        {
            return null;
        }

        var pending = await data.AuthorEdits
            .AsNoTracking()
            .IgnoreQueryFilters()
            .ApplyIsDeletedFilter()
            .FirstOrDefaultAsync(
                e => e.AuthorId == authorId,
                cancellationToken);

        return pending is null
            ? authorDetails
            : ApplyPendingEdit(authorDetails, pending);
    }

    public async Task<ResultWith<AuthorDetailsServiceModel>> Create(
        CreateAuthorServiceModel serviceModel,
        CancellationToken cancellationToken = default)
    {
        var genderIsInvalid = !GenderIsValidEnum(serviceModel.Gender);
        if (genderIsInvalid)
        {
            return InvalidGender;
        }

        var nationalityIsInvalid = !NationalityIsValidEnum(serviceModel.Nationality);
        if (nationalityIsInvalid)
        {
            return InvalidNationality;
        }

        var dbModel = serviceModel.ToDbModel();
        dbModel.CreatorId = userService.GetId();

        var isAdmin = userService.IsAdmin();
        if (isAdmin)
        {
            dbModel.IsApproved = true;
        }

        await imageWriter.Write(
            resourceName: ImagePathPrefix,
            dbModel,
            serviceModel,
            DefaultImagePath,
            cancellationToken);

        data.Add(dbModel);

        if (!isAdmin)
        {
            notificationService.AddOnAuthorCreation(
                dbModel.Id,
                dbModel.Name,
                await adminService.GetIds(cancellationToken));
        }

        // One SaveChanges: the author and the admin notifications commit together.
        await data.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "New author with Id: {id} was created.",
            dbModel.Id);

        var result = dbModel.ToDetailsServiceModel();
        return ResultWith<AuthorDetailsServiceModel>.Success(result);
    }

    public async Task<Result> Edit(
        Guid authorId,
        CreateAuthorServiceModel serviceModel,
        CancellationToken cancellationToken = default)
    {
        var genderIsInvalid = !GenderIsValidEnum(serviceModel.Gender);
        if (genderIsInvalid)
        {
            return InvalidGender;
        }

        var nationalityIsInvalid = !NationalityIsValidEnum(serviceModel.Nationality);
        if (nationalityIsInvalid)
        {
            return InvalidNationality;
        }

        var author = await this.GetDbModel(authorId, cancellationToken);
        if (author is null)
        {
            return this.LogAndReturnNotFound(authorId);
        }

        var userId = userService.GetId()!;
        var isNotCreator = author.CreatorId != userId;
        var isAdmin = userService.IsAdmin();

        if (isNotCreator && !isAdmin)
        {
            return LogAndReturnForbidden(authorId, userId);
        }

        if (isAdmin)
        {
            return await this.EditDirectly(
                author,
                serviceModel,
                cancellationToken);
        }

        var pending = await data
            .AuthorEdits
            .FirstOrDefaultAsync(
                a => a.AuthorId == authorId,
                cancellationToken);

        if (pending is null)
        {
            pending = new()
            {
                AuthorId = authorId,
                RequestedById = userId,
                ImagePath = author.ImagePath
            };

            data.AuthorEdits.Add(pending);
        }
        else
        {
            pending.RequestedById = userId;
        }

        var oldPendingImagePath = pending.ImagePath;
        var newImageUploaded = serviceModel.Image is not null;

        serviceModel.UpdatePendingDbModel(pending);

        if (newImageUploaded)
        {
            await imageWriter.Write(
                resourceName: PendingImagePathPrefix,
                dbModel: pending,
                serviceModel,
                defaultImagePath: null,
                cancellationToken);

            var shouldDeleteOldPendingImage =
                !string.Equals(
                    oldPendingImagePath,
                    pending.ImagePath,
                    StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(
                    oldPendingImagePath,
                    DefaultImagePath,
                    StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(
                    oldPendingImagePath,
                    author.ImagePath,
                    StringComparison.OrdinalIgnoreCase);

            if (shouldDeleteOldPendingImage)
            {
                imageWriter.Delete(
                    resourceName: PendingImagePathPrefix,
                    imagePath: oldPendingImagePath,
                    DefaultImagePath);
            }
        }
        else
        {
            pending.ImagePath = author.ImagePath;
        }

        notificationService.AddOnAuthorEdition(
            author.Id,
            author.Name,
            await adminService.GetIds(cancellationToken));

        // One SaveChanges: the pending edit and the admin notifications commit together.
        await data.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Author edit request created/updated for AuthorId: {id}. Pending edit id: {pendingId}",
            author.Id,
            pending.Id);

        return true;
    }

    public async Task<Result> Delete(
        Guid authorId,
        CancellationToken cancellationToken = default)
    {
        var dbModel = await this.GetDbModel(
            authorId,
            cancellationToken);

        if (dbModel is null)
        {
            return LogAndReturnNotFound(authorId);
        }

        var userId = userService.GetId()!;
        var isNotCreator = dbModel.CreatorId != userId;
        var isNotAdmin = !userService.IsAdmin();

        if (isNotCreator && isNotAdmin)
        {
            return LogAndReturnForbidden(authorId, userId);
        }

        data.Remove(dbModel);
        await data.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Author with Id: {id} was deleted.",
            dbModel.Id);

        return true;
    }

    public async Task<Result> Approve(
        Guid authorId,
        CancellationToken cancellationToken = default)
    {
        if (!userService.IsAdmin())
        {
            return LogAndReturnForbidden(
                authorId,
                userService.GetId()!);
        }

        var dbModel = await data
            .Authors
            .IgnoreQueryFilters()
            .ApplyIsDeletedFilter()
            .FirstOrDefaultAsync(a => a.Id == authorId, cancellationToken);

        if (dbModel is null)
        {
            return LogAndReturnNotFound(authorId);
        }

        var pendingDbModel = await data
            .AuthorEdits
            .IgnoreQueryFilters()
            .ApplyIsDeletedFilter()
            .FirstOrDefaultAsync(
                a => a.AuthorId == authorId,
                cancellationToken);

        if (pendingDbModel is not null)
        {
            var oldAuthorImagePath = dbModel.ImagePath;

            pendingDbModel.UpdatePendingDbModel(dbModel);

            data.AuthorEdits.Remove(pendingDbModel);

            dbModel.IsApproved = true;

            await data.SaveChangesAsync(cancellationToken);

            var shouldDeleteOldAuthorImage =
                !string.Equals(
                    oldAuthorImagePath,
                    dbModel.ImagePath,
                    StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(
                    oldAuthorImagePath,
                    DefaultImagePath,
                    StringComparison.OrdinalIgnoreCase);

            if (shouldDeleteOldAuthorImage)
            {
                imageWriter.Delete(
                    resourceName: ImagePathPrefix,
                    imagePath: oldAuthorImagePath,
                    defaultImagePath: DefaultImagePath);
            }

            logger.LogInformation(
                "Pending edit for AuthorId: {id} approved and applied.",
                dbModel.Id);

            await notificationService.CreateOnAuthorApproved(
                authorId,
                dbModel.Name,
                receiverId: dbModel.CreatorId!,
                cancellationToken);

            return true;
        }

        var wasPreviouslyUnapproved = !dbModel.IsApproved;
        dbModel.IsApproved = true;

        await data.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Author with Id: {id} was approved.",
            dbModel.Id);

        await notificationService.CreateOnAuthorApproved(
            authorId,
            dbModel.Name,
            receiverId: dbModel.CreatorId!,
            cancellationToken);

        if (wasPreviouslyUnapproved)
        {
            await profileService.IncrementCreatedAuthorsCount(
                userId: dbModel.CreatorId!,
                cancellationToken);
        }

        return true;
    }

    public async Task<Result> Reject(
        Guid authorId,
        CancellationToken cancellationToken = default)
    {
        if (!userService.IsAdmin())
        {
            return LogAndReturnForbidden(
                authorId,
                userService.GetId()!);
        }

        var dbModel = await data
            .Authors
            .IgnoreQueryFilters()
            .ApplyIsDeletedFilter()
            .FirstOrDefaultAsync(
                a => a.Id == authorId,
                cancellationToken);

        if (dbModel is null)
        {
            return LogAndReturnNotFound(authorId);
        }

        var pendingDbModel = await data
            .AuthorEdits
            .IgnoreQueryFilters()
            .ApplyIsDeletedFilter()
            .FirstOrDefaultAsync(
                a => a.AuthorId == authorId,
                cancellationToken);

        if (pendingDbModel is not null)
        {
            var pendingImagePath = pendingDbModel.ImagePath;

            data.AuthorEdits.Remove(pendingDbModel);
            await data.SaveChangesAsync(cancellationToken);

            var shouldDeletePendingImage =
                !string.Equals(
                    pendingImagePath,
                    dbModel.ImagePath,
                    StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(
                    pendingImagePath,
                    DefaultImagePath,
                    StringComparison.OrdinalIgnoreCase);

            if (shouldDeletePendingImage)
            {
                imageWriter.Delete(
                    resourceName: PendingImagePathPrefix,
                    imagePath: pendingImagePath,
                    DefaultImagePath);
            }

            logger.LogInformation(
                "Pending edit for AuthorId: {id} was rejected (author unchanged).",
                dbModel.Id);

            await notificationService.CreateOnAuthorRejected(
                authorId,
                dbModel.Name,
                receiverId: dbModel.CreatorId!,
                cancellationToken);

            return true;
        }

        data.Remove(dbModel);
        await data.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Author with Id: {id} was rejected and deleted.",
            dbModel.Id);

        await notificationService.CreateOnAuthorRejected(
            authorId,
            dbModel.Name,
            receiverId: dbModel.CreatorId!,
            cancellationToken);

        return true;
    }

    private static AuthorDetailsServiceModel ApplyPendingEdit(
        AuthorDetailsServiceModel serviceModel,
        AuthorEditDbModel pendingDbModel)
        => new ()
        {
            Id = serviceModel.Id,
            BooksCount = serviceModel.BooksCount,
            AverageRating = serviceModel.AverageRating,
            TopBooks = serviceModel.TopBooks,
            Name = pendingDbModel.Name,
            ImagePath = pendingDbModel.ImagePath,
            Biography = pendingDbModel.Biography,
            PenName = pendingDbModel.PenName,
            Nationality = pendingDbModel.Nationality,
            Gender = pendingDbModel.Gender,
            BornAt = pendingDbModel.BornAt.ToIso8601String(),
            DiedAt = pendingDbModel.DiedAt.ToIso8601String(),
            CreatorId = serviceModel.CreatorId,
            IsApproved = serviceModel.IsApproved
        };

    // Admins' edits apply directly to the author. Only non-admin edits go to the pending queue.
    private async Task<Result> EditDirectly(
        AuthorDbModel dbModel,
        CreateAuthorServiceModel serviceModel,
        CancellationToken cancellationToken = default)
    {
        var oldImagePath = dbModel.ImagePath;

        serviceModel.UpdateDbModel(dbModel);

        // A no-op when no image was uploaded (null default path keeps the current image).
        await imageWriter.Write(
            resourceName: ImagePathPrefix,
            dbModel,
            serviceModel,
            defaultImagePath: null,
            cancellationToken);

        await data.SaveChangesAsync(cancellationToken);

        var shouldDeleteOldImage =
            !string.Equals(
                oldImagePath,
                dbModel.ImagePath,
                StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(
                oldImagePath,
                DefaultImagePath,
                StringComparison.OrdinalIgnoreCase);

        if (shouldDeleteOldImage)
        {
            imageWriter.Delete(
                resourceName: ImagePathPrefix,
                imagePath: oldImagePath,
                defaultImagePath: DefaultImagePath);
        }

        logger.LogInformation(
            "Author with Id: {id} was edited directly by an admin.",
            dbModel.Id);

        return true;
    }

    private async Task<AuthorDbModel?> GetDbModel(
        Guid authorId,
        CancellationToken cancellationToken = default)
        => await data
            .Authors
            .FindAsync([authorId], cancellationToken);

    private Result LogAndReturnNotFound(Guid authorId)
    {
        logger.LogWarning(
            DbEntityNotFoundTemplate,
            nameof(AuthorDbModel),
            authorId);

        return Result.NotFound(string.Format(
            ResourceNotFound,
            ResourceName));
    }

    private Result LogAndReturnForbidden(
        Guid authorId,
        string userId)
    {
        logger.LogWarning(
            UnauthorizedMessageTemplate,
            userId,
            nameof(AuthorDbModel),
            authorId);

        return Result.Forbidden(string.Format(
            ResourceForbidden,
            ResourceName));
    }

    private static bool GenderIsValidEnum(
        Gender gender)
        => Enum.IsDefined(gender);

    private static bool NationalityIsValidEnum(
        Nationality nationality)
        => Enum.IsDefined(nationality);
}
