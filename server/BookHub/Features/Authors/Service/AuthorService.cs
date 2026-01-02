namespace BookHub.Features.Authors.Service;

using Areas.Admin.Service;
using BookHub.Data;
using Data.Models;
using Features.UserProfile.Data.Models;
using Infrastructure.Extensions;
using Infrastructure.Services.CurrentUser;
using Infrastructure.Services.ImageWriter;
using Infrastructure.Services.Result;
using Microsoft.EntityFrameworkCore;
using Models;
using Notification.Service;
using UserProfile.Service;

using static Common.Constants.ErrorMessages;
using static Shared.AuthorMapping;
using static Shared.Constants.Paths;

public class AuthorService(
    BookHubDbContext data,
    ICurrentUserService userService,
    IAdminService adminService,
    INotificationService notificationService,
    IImageWriter imageWriter,
    ILogger<AuthorService> logger,
    IProfileService profileService) : IAuthorService
{
    public async Task<IEnumerable<AuthorNamesServiceModel>> Names(
        CancellationToken token = default)
      => await data
          .Authors
          .ToNamesServiceModels()
          .ToListAsync(token);

    public async Task<IEnumerable<AuthorServiceModel>> TopThree(
        CancellationToken token = default)
        => await data
            .Authors
            .ToServiceModels()
            .OrderByDescending(a => a.AverageRating)
            .Take(3)
            .ToListAsync(token);

    public async Task<AuthorDetailsServiceModel?> Details(
        Guid id,
        CancellationToken token = default)
        => await data
            .Authors
            .ToDetailsServiceModels()
            .FirstOrDefaultAsync(a => a.Id == id, token);

    public async Task<AuthorDetailsServiceModel?> AdminDetails(
        Guid id,
        CancellationToken token = default)
         => await data
             .Authors
             .IgnoreQueryFilters()
             .ApplyIsDeletedFilter()
             .ToDetailsServiceModels()
             .FirstOrDefaultAsync(a => a.Id == id, token);

    public async Task<AuthorDetailsServiceModel> Create(
        CreateAuthorServiceModel serviceModel,
        CancellationToken token = default)
    {
        var dbModel = serviceModel.ToDbModel();
        dbModel.CreatorId = userService.GetId();

        var isAdmin = userService.IsAdmin();
        if (isAdmin)
        {
            dbModel.IsApproved = true;
        }

        await imageWriter.Write(
            AuthorsImagePathPrefix,
            dbModel,
            serviceModel,
            DefaultImagePath,
            token);

        data.Add(dbModel);
        await data.SaveChangesAsync(token);

        logger.LogInformation(
            "New author with Id: {id} was created.",
            dbModel.Id);

        if (!isAdmin)
        {
            await notificationService.CreateOnEntityCreation(
                dbModel.Id,
                "Author",
                dbModel.Name,
                await adminService.GetId(),
                token);
        }

        return dbModel.ToDetailsServiceModel();
    }

    public async Task<Result> Edit(
        Guid id,
        CreateAuthorServiceModel serviceModel,
        CancellationToken token = default)
    {
        var dbModel = await this.GetDbModel(id, token);
        if (dbModel is null)
        {
            return this.LogAndReturnNotFoundMessage(id);
        }

        var userId = userService.GetId()!;
        if (dbModel.CreatorId != userId)
        {
            return LogAndReturnUnauthorizedMessage(id, userId);
        }

        var oldImagePath = dbModel.ImagePath;
        var isNewImageUploaded = serviceModel.Image is not null;

        serviceModel.UpdateDbModel(dbModel);

        await imageWriter.Write(
            AuthorsImagePathPrefix,
            dbModel,
            serviceModel,
            null,
            token);

        if (isNewImageUploaded)
        {
            imageWriter.Delete(
                nameof(AuthorDbModel),
                oldImagePath,
                DefaultImagePath);
        }

        await data.SaveChangesAsync(token);

        logger.LogInformation(
            "Author with Id: {id} was updated.",
            dbModel.Id);

        return true;
    }

    public async Task<Result> Delete(
        Guid authorId,
        CancellationToken token = default)
    {
        var dbModel = await this.GetDbModel(authorId, token);
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
        await data.SaveChangesAsync(token);

        logger.LogInformation(
            "Author with Id: {id} was deleted.",
            dbModel.Id);

        return true;
    }

    public async Task<Result> Approve(
        Guid id,
        CancellationToken token = default)
    {
        var dbModel = await data
             .Authors
             .IgnoreQueryFilters()
             .ApplyIsDeletedFilter()
             .FirstOrDefaultAsync(a => a.Id == id, token);

        if (dbModel is null)
        {
            return LogAndReturnNotFoundMessage(id);
        }

        if (!userService.IsAdmin())
        {
            return LogAndReturnUnauthorizedMessage(id, userService.GetId()!);
        }

        dbModel.IsApproved = true;
        await data.SaveChangesAsync(token);

        logger.LogInformation(
            "Author with Id: {id} was approved.",
            dbModel.Id);

        await notificationService.CreateOnEntityApprovalStatusChange(
            id,
            "Author",
            dbModel.Name,
            dbModel.CreatorId!,
            true);

        await profileService.IncrementCreatedAuthorsCount(
            dbModel.CreatorId!,
            token);

        return true;
    }

    public async Task<Result> Reject(
        Guid id,
        CancellationToken token = default)
    {
        var dbModel = await data
            .Authors
            .IgnoreQueryFilters()
            .ApplyIsDeletedFilter()
            .FirstOrDefaultAsync(a => a.Id == id, token);

        if (dbModel is null)
        {
            return LogAndReturnNotFoundMessage(id);
        }

        if (!userService.IsAdmin())
        {
            return LogAndReturnUnauthorizedMessage(id, userService.GetId()!);
        }

        logger.LogInformation(
            "Author with Id: {id} was rejected.",
            dbModel.Id);

        await notificationService.CreateOnEntityApprovalStatusChange(
            id,
            "Author",
            dbModel.Name,
            dbModel.CreatorId!,
            false,
            token);

        return true;
    }

    private async Task<AuthorDbModel?> GetDbModel(
        Guid id,
        CancellationToken token = default)
        => await data
            .Authors
            .FindAsync([id], token);

    private string LogAndReturnNotFoundMessage(Guid id)
    {
        logger.LogWarning(
            DbEntityNotFoundTemplate,
            nameof(AuthorDbModel),
            id);

        return string.Format(
            DbEntityNotFound,
            nameof(AuthorDbModel),
            id);
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
}
