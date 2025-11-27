namespace BookHub.Features.Article.Service;

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

public class ArticlesService(
    BookHubDbContext data,
    IImageWriter imageWriter,
    ILogger<ArticlesService> logger) : IArticlesService
{
    public async Task<ArticleDetailsServiceModel?> Details(
        Guid id,
        bool isEditMode = false,
        CancellationToken token = default)
    {
        if (!isEditMode)
        {
            var rowsAffected = await data
                .Articles
                .Where(a => a.Id == id)
                .ExecuteUpdateAsync(
                    s => s.SetProperty
                        (a => a.Views, a => a.Views + 1),
                    token);

            if (rowsAffected == 0)
            {
                return null;
            }
        }

        return await data
            .Articles
            .Select(Mapping.ToDetailsServiceModelExpression)
            .FirstOrDefaultAsync(a => a.Id == id, token);
    }

    public async Task<ArticleDetailsServiceModel> Create(
        CreateArticleServiceModel serviceModel,
        CancellationToken token = default)
    {
        var dbModel = serviceModel.ToDbModel();

        await imageWriter.Write(
            dbModel.Id,
            ArticlesImagePathPrefix,
            dbModel,
            serviceModel,
            DefaultImagePath,
            token);

        data.Add(dbModel);
        await data.SaveChangesAsync(token);

        logger.LogInformation(
            "New article with Id: {id} was created.",
            dbModel.Id);

        return dbModel.ToDetailsServiceModel();
    }

    public async Task<Result> Edit(
        Guid id,
        CreateArticleServiceModel serviceModel,
        CancellationToken token = default)
    {
        var dbModel = await this.GetDbModel(id, token);
        if (dbModel is null)
        {
            return LogAndReturnNotFoundMessage(id);
        }

        var oldImagePath = dbModel.ImagePath;
        var isNewImageUploaded = serviceModel.Image is not null;

        serviceModel.UpdateDbModel(dbModel);

        await imageWriter.Write(
            dbModel.Id,
            ArticlesImagePathPrefix,
            dbModel,
            serviceModel,
            null,
            token);

        if (isNewImageUploaded)
        {
            imageWriter.Delete(
                id,
                nameof(ArticleDbModel),
                oldImagePath,
                DefaultImagePath);
        }

        await data.SaveChangesAsync(token);

        logger.LogInformation(
            "Article with Id: {id} was updated.",
            dbModel.Id);

        return true;
    }


    public async Task<Result> Delete(
        Guid id,
        CancellationToken token = default)
    {
        var dbModel = await this.GetDbModel(id, token);
        if (dbModel is null)
        {
            return LogAndReturnNotFoundMessage(id);
        }

        data.Remove(dbModel);
        await data.SaveChangesAsync(token);

        logger.LogInformation(
            "Article with Id: {id} was deleted.",
            dbModel.Id);

        return true;
    }

    private async Task<ArticleDbModel?> GetDbModel(
        Guid id,
        CancellationToken token = default)
        => await data
            .Articles
            .FindAsync([id], token);

    private string LogAndReturnNotFoundMessage(Guid id)
    {
        logger.LogWarning(
            DbEntityNotFoundTemplate,
            nameof(ArticleDbModel),
            id);

        return string.Format(
            DbEntityNotFound,
            nameof(ArticleDbModel),
            id);
    }
}
