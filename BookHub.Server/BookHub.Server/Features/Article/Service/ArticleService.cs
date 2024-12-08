namespace BookHub.Server.Features.Article.Service
{
    using AutoMapper;
    using Data.Models;
    using Infrastructure.Services;
    using Models;
    using Server.Data;

    using static Common.ErrorMessage;

    public class ArticleService(
        BookHubDbContext data,
        ICurrentUserService userService,
        IMapper mapper) : IArticleService
    {
        private const string DefaultImageUrl = "https://img.freepik.com/free-photo/bookmark-books-arrangement-top-view_23-2149894335.jpg";

        private readonly BookHubDbContext data = data;
        private readonly ICurrentUserService userService = userService;
        private readonly IMapper mapper = mapper;

        public async Task<ArticleDetailsServiceModel?> DetailsAsync(int id)
        {
            var article = await this.data
                .Articles
                .FindAsync(id);

            if (article is null) 
            {
                return null;
            }

            article.Views++;
            await this.data.SaveChangesAsync();

            return this.mapper.Map<ArticleDetailsServiceModel>(article);
        }

        public async Task<int> CreateAsync(CreateArticleServiceModel model)
        {
            model.ImageUrl ??= DefaultImageUrl;

            var article = this.mapper.Map<Article>(model);

            this.data.Add(article);
            await this.data.SaveChangesAsync();

            return article.Id;
        }

        public async Task<Result> EditAsync(int id, CreateArticleServiceModel model)
        {
            var article = await this.data
                 .Articles
                 .FindAsync(id);

            if (article is null)
            {
                return string.Format(DbEntityNotFound, nameof(Article), id);
            }

            if (!this.userService.IsAdmin())
            {
                return string.Format(
                    UnauthorizedDbEntityAction,
                    this.userService.GetUsername(),
                    nameof(Article),
                    id);
            }

            model.ImageUrl ??= DefaultImageUrl;

            this.mapper.Map(model, article);

            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var article = await this.data
                  .Articles
                  .FindAsync(id);

            if (article is null)
            {
                return string.Format(DbEntityNotFound, nameof(Article), id);
            }

            if (!this.userService.IsAdmin())
            {
                return string.Format(
                    UnauthorizedDbEntityAction,
                    this.userService.GetUsername(),
                    nameof(Article),
                    id);
            }

            this.data.Remove(article);
            await this.data.SaveChangesAsync();

            return true;
        }
    }
}
