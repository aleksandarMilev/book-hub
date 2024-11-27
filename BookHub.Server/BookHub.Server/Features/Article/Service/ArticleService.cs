namespace BookHub.Server.Features.Article.Service
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Models;

    using static Common.Constants.DefaultValues;

    public class ArticleService(
        BookHubDbContext data,
        IMapper mapper) : IArticleService
    {
        private readonly BookHubDbContext data = data;
        private readonly IMapper mapper = mapper;

        public async Task<ArticleDetailsServiceModel?> DetailsAsync(int id)
            => await this.data
                .Articles
                .ProjectTo<ArticleDetailsServiceModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

        public async Task<int> CreateAsync(CreateArticleServiceModel model)
        {
            model.ImageUrl ??= DefaultArticleImageUrl;

            var article = this.mapper.Map<Article>(model);

            this.data.Add(article);
            await this.data.SaveChangesAsync();

            return article.Id;
        }
    }
}
