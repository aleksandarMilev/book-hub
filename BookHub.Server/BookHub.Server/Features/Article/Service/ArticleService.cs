namespace BookHub.Server.Features.Article.Service
{
    using AutoMapper;
    using Data;
    using Data.Models;
    using Infrastructure.Services;
    using Models;

    using static Common.Constants.DefaultValues;
    using static Common.Messages.Error.Article;

    public class ArticleService(
        BookHubDbContext data,
        ICurrentUserService userService,
        IMapper mapper) : IArticleService
    {
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
            model.ImageUrl ??= DefaultArticleImageUrl;

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
                return ArticleNotFound;
            }

            if (!this.userService.IsAdmin())
            {
                return UnauthorizedArticleEdit;
            }

            model.ImageUrl ??= DefaultArticleImageUrl;

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
                return ArticleNotFound;
            }

            if (!this.userService.IsAdmin())
            {
                return UnauthorizedArticleEdit;
            }

            this.data.Remove(article);
            await this.data.SaveChangesAsync();

            return true;
        }
    }
}
