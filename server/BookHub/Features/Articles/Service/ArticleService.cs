namespace BookHub.Features.Articles.Service;

using BookHub.Data;
using Data.Models;
using Infrastructure.Services.ImageWriter;
using Infrastructure.Services.Result;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;
using Shared;

using static Common.Constants.ErrorMessages;
using static Shared.Constants.Paths;

public class ArticleService(
    BookHubDbContext data,
    IImageWriter imageWriter,
    ILogger<ArticleService> logger) : IArticleService
{
    public async Task<ArticleDetailsServiceModel?> Details(
        Guid articleId,
        bool isEditMode = false,
        CancellationToken cancellationToken = default)
    {
        if (!isEditMode)
        {
            var rowsAffected = await data
                .Articles
                .Where(a => a.Id == articleId)
                .ExecuteUpdateAsync(
                    setters => setters.SetProperty
                        (a => a.Views, a => a.Views + 1),
                    cancellationToken);

            if (rowsAffected == 0)
            {
                return null;
            }
        }

        return await data
            .Articles
            .AsNoTracking()
            .ToServiceDetailsModels()
            .FirstOrDefaultAsync(
                a => a.Id == articleId,
                cancellationToken);
    }

    public async Task<ArticleDetailsServiceModel> Create(
        CreateArticleServiceModel serviceModel,
        CancellationToken cancellationToken = default)
    {
        var dbModel = serviceModel.ToDbModel();

        await imageWriter.Write(
           ImagePathPrefix,
           dbModel,
           serviceModel,
           DefaultImagePath,
           cancellationToken);

        data.Add(dbModel);
        await data.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "New article with Id: {id} was created.",
            dbModel.Id);

        return dbModel.ToDetailsServiceModel();
    }

    public async Task<Result> Edit(
        Guid articleId,
        CreateArticleServiceModel serviceModel,
        CancellationToken cancellationToken = default)
    {
        var dbModel = await this.GetDbModel(
            articleId,
            cancellationToken);

        if (dbModel is null)
        {
            return LogAndReturnNotFoundMessage(articleId);
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

        await data.SaveChangesAsync(cancellationToken);

        var shouldDeleteOldImage =
            isNewImageUploaded &&
            !string.Equals(
                oldImagePath,
                dbModel.ImagePath,
                StringComparison.OrdinalIgnoreCase);

        if (shouldDeleteOldImage)
        {
            imageWriter.Delete(
                ImagePathPrefix,
                oldImagePath,
                DefaultImagePath);
        }

        logger.LogInformation(
            "Article with Id: {id} was updated.",
            dbModel.Id);

        return true;
    }

    public async Task<Result> Delete(
        Guid articleId,
        CancellationToken cancellationToken = default)
    {
        var dbModel = await this.GetDbModel(
            articleId,
            cancellationToken);

        if (dbModel is null)
        {
            return LogAndReturnNotFoundMessage(articleId);
        }

        data.Remove(dbModel);
        await data.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Article with Id: {id} was deleted.",
            dbModel.Id);

        return true;
    }

    private async Task<ArticleDbModel?> GetDbModel(
        Guid articleId,
        CancellationToken cancellationToken = default)
        => await data
            .Articles
            .FindAsync([articleId], cancellationToken);

    private string LogAndReturnNotFoundMessage(Guid articleId)
    {
        logger.LogWarning(
            DbEntityNotFoundTemplate,
            nameof(ArticleDbModel),
            articleId);

        return string.Format(
            DbEntityNotFound,
            nameof(ArticleDbModel),
            articleId);
    }
}
