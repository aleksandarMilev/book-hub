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
using static Shared.AuthorMapping;
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
        => await data
            .Authors
            .AsNoTracking()
            .ToDetailsServiceModels()
            .FirstOrDefaultAsync(
                a => a.Id == authorId,
                cancellationToken);

    public async Task<AuthorDetailsServiceModel?> AdminDetails(
        Guid authorId,
        CancellationToken cancellationToken = default)
         => await data
             .Authors
             .AsNoTracking()
             .IgnoreQueryFilters()
             .ApplyIsDeletedFilter()
             .ToDetailsServiceModels()
             .FirstOrDefaultAsync(
                a => a.Id == authorId,
                cancellationToken);

    public async Task<ResultWith<AuthorDetailsServiceModel>> Create(
        CreateAuthorServiceModel serviceModel,
        CancellationToken cancellationToken = default)
    {
        var genderIsInvalid = !GenderIsValidEnum(serviceModel.Gender);
        if (genderIsInvalid)
        {
            return $"{serviceModel.Gender} is not valid Gender enumeartion!";
        }

        var nationalityIsInvalid = !NationalityIsValidEnum(serviceModel.Nationality);
        if (nationalityIsInvalid)
        {
            return $"{serviceModel.Nationality} is not valid Nationality enumeartion!";
        }

        var dbModel = serviceModel.ToDbModel();
        dbModel.CreatorId = userService.GetId();

        var isAdmin = userService.IsAdmin();
        if (isAdmin)
        {
            dbModel.IsApproved = true;
        }

        await imageWriter.Write(
            ImagePathPrefix,
            dbModel,
            serviceModel,
            DefaultImagePath,
            cancellationToken);

        data.Add(dbModel);
        await data.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "New author with Id: {id} was created.",
            dbModel.Id);

        if (!isAdmin)
        {
            await notificationService.CreateOnAuthorCreation(
                dbModel.Id,
                dbModel.Name,
                await adminService.GetId(),
                cancellationToken);
        }

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
            return $"{serviceModel.Gender} is not valid Gender enumeartion!";
        }

        var nationalityIsInvalid = !NationalityIsValidEnum(serviceModel.Nationality);
        if (nationalityIsInvalid)
        {
            return $"{serviceModel.Nationality} is not valid Nationality enumeartion!";
        }

        var dbModel = await this.GetDbModel(
            authorId,
            cancellationToken);

        if (dbModel is null)
        {
            return this.LogAndReturnNotFoundMessage(authorId);
        }

        var userId = userService.GetId()!;

        var isNotCreator = dbModel.CreatorId != userId;
        var isNotAdmin = !userService.IsAdmin();

        if (isNotCreator && isNotAdmin)
        {
            return LogAndReturnUnauthorizedMessage(
                authorId,
                userId);
        }

        var oldImagePath = dbModel.ImagePath;
        var isNewImageUploaded = serviceModel.Image is not null;

        serviceModel.UpdateDbModel(dbModel);

        await imageWriter.Write(
            ImagePathPrefix,
            dbModel,
            serviceModel,
            null,
            cancellationToken);

        var shouldDeleteOldImage =
            isNewImageUploaded &&
            !string.Equals(
                oldImagePath,
                dbModel.ImagePath,
                StringComparison.OrdinalIgnoreCase);

        if (shouldDeleteOldImage)
        {
            imageWriter.Delete(
                nameof(AuthorDbModel),
                oldImagePath,
                DefaultImagePath);
        }

        dbModel.IsApproved = false;

        await data.SaveChangesAsync(cancellationToken);

        if (!userService.IsAdmin())
        {
            await notificationService.CreateOnAuthorEdition(
               dbModel.Id,
               dbModel.Name,
               await adminService.GetId(),
               cancellationToken);
        }

        logger.LogInformation(
            "Author with Id: {id} was updated.",
            dbModel.Id);

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
            return LogAndReturnNotFoundMessage(authorId);
        }

        var userId = userService.GetId()!;
        var isNotCreator = dbModel.CreatorId != userId;
        var isNotAdmin = !userService.IsAdmin();

        if (isNotCreator && isNotAdmin)
        {
            return LogAndReturnUnauthorizedMessage(authorId, userId);
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
            return LogAndReturnUnauthorizedMessage(
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
            return LogAndReturnNotFoundMessage(authorId);
        }

        dbModel.IsApproved = true;
        await data.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Author with Id: {id} was approved.",
            dbModel.Id);

        await notificationService.CreateOnAuthorApproved(
            authorId,
            dbModel.Name,
            dbModel.CreatorId!,
            cancellationToken);

        await profileService.IncrementCreatedAuthorsCount(
            dbModel.CreatorId!,
            cancellationToken);

        return true;
    }

    public async Task<Result> Reject(
        Guid authorId,
        CancellationToken cancellationToken = default)
    {
        if (!userService.IsAdmin())
        {
            return LogAndReturnUnauthorizedMessage(
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
            return LogAndReturnNotFoundMessage(authorId);
        }

        data.Remove(dbModel);
        await data.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Author with Id: {id} was rejected and deleted.",
            dbModel.Id);

        await notificationService.CreateOnAuthorRejected(
            authorId,
            dbModel.Name,
            dbModel.CreatorId!,
            cancellationToken);

        return true;
    }

    private async Task<AuthorDbModel?> GetDbModel(
        Guid authorId,
        CancellationToken cancellationToken = default)
        => await data
            .Authors
            .FindAsync([authorId], cancellationToken);

    private string LogAndReturnNotFoundMessage(Guid authorId)
    {
        logger.LogWarning(
            DbEntityNotFoundTemplate,
            nameof(AuthorDbModel),
            authorId);

        return string.Format(
            DbEntityNotFound,
            nameof(AuthorDbModel),
            authorId);
    }

    private string LogAndReturnUnauthorizedMessage(
        Guid authorId,
        string userId)
    {
        logger.LogWarning(
            UnauthorizedMessageTemplate,
            userId,
            nameof(AuthorDbModel),
            authorId);

        return string.Format(
            UnauthorizedMessage,
            userId,
            nameof(AuthorDbModel),
            authorId);
    }

    private static bool GenderIsValidEnum(
        Gender gender)
        => Enum.IsDefined(gender);

    private static bool NationalityIsValidEnum(
        Nationality nationality)
        => Enum.IsDefined(nationality);
}
