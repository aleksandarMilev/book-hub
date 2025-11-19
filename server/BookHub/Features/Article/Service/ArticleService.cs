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

    public class ArticleService(
        BookHubDbContext data,
        ILogger<ArticleService> logger) : IArticleService
    {
        public async Task<ArticleDetailsServiceModel?> Details(
            Guid id,
            CancellationToken token = default)
        {
            var rowsAffected = await data.Articles
                .Where(a => a.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(a => a.Views, a => a.Views + 1),
                    token);

            if (rowsAffected == 0)
            {
                return null;
            }

            var dbModel = await this.GetByIdAsNoTracking(id, token);
            if (dbModel is null)
            {
                return null;
            }

            return dbModel.ToDetailsServiceModel();
        }

        public async Task<ArticleDetailsServiceModel> Create(
            CreateArticleServiceModel serviceModel,
            CancellationToken token = default)
        {
            var dbModel = serviceModel.ToDbModel();
            SetDefaultImageIfNull(dbModel);

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
            var dbModel = await this.GetById(id, token);
            if (dbModel is null)
            {
                return LogAndReturnNotFoundMessage(id);
            }

            serviceModel.UpdateDbModel(dbModel);
            SetDefaultImageIfNull(dbModel);

            await data.SaveChangesAsync(token);

            logger.LogInformation(
                "{article} with Id: {id} was updated.", nameof(Article), dbModel.Id);

            return true;
        }

        public async Task<Result> Delete(
            Guid id,
            CancellationToken token = default)
        {
            var dbModel = await this.GetById(id, token);
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

        private async Task<Article?> GetById(
            Guid id,
            CancellationToken token = default)
            => await data
                .Articles
                .FindAsync([id], token);

        private async Task<Article?> GetByIdAsNoTracking(
            Guid id,
            CancellationToken token = default)
            => await data
                .Articles
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id, token);

        private string LogAndReturnNotFoundMessage(Guid id)
        {
            logger.LogWarning(DbEntityNotFoundTemplate, nameof(Article), id);
            return string.Format(DbEntityNotFound, nameof(Article), id);
        }

        private static void SetDefaultImageIfNull(Article article)
            => article.ImageUrl ??= DefaultImageUrl;
    }
}
