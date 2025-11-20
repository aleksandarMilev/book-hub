namespace BookHub.Features.Article.Service
{
    using BookHub.Data;
    using Data.Models;
    using Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Models;
    using Shared;

    using static Common.ErrorMessage;
    using static Shared.Constants.DefaultValues;

    public class ArticlesService(
        BookHubDbContext data,
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
                    .ExecuteUpdateAsync(s => s
                        .SetProperty(a => a.Views, a => a.Views + 1),
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

            if (serviceModel.Image is not null)
            {
                await SaveImageFile(serviceModel, dbModel, token);
            }
            else
            {
                dbModel.ImagePath = DefaultImagePath;
            }

            data.Add(dbModel);
            await data.SaveChangesAsync(token);

            logger.LogInformation(
                "New {article} with Id: {id} was created.", nameof(Article), dbModel.Id);

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

            serviceModel.UpdateDbModel(dbModel);

            if (serviceModel.Image is not null)
            {
                await SaveImageFile(serviceModel, dbModel, token);
            }

            await data.SaveChangesAsync(token);

            logger.LogInformation(
                "{article} with Id: {id} was updated.", nameof(Article), dbModel.Id);

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
                "{article} with Id: {id} was deleted.", nameof(Article), dbModel.Id);

            return true;
        }

        private async Task<Article?> GetDbModel(
            Guid id,
            CancellationToken token = default)
            => await data
                .Articles
                .FindAsync([id], token);

        private string LogAndReturnNotFoundMessage(Guid id)
        {
            logger.LogWarning(DbEntityNotFoundTemplate, nameof(Article), id);
            return string.Format(DbEntityNotFound, nameof(Article), id);
        }

        private static async Task SaveImageFile(
            CreateArticleServiceModel serviceModel,
            Article dbModel,
            CancellationToken token = default)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(serviceModel.Image!.FileName)}";
            var filePath = Path.Combine("wwwroot", "images", "articles", fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await serviceModel.Image.CopyToAsync(stream, token);

            dbModel.ImagePath = $"/images/articles/{fileName}";
        }
    }
}
