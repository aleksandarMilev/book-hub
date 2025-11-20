namespace BookHub.Features.Article.Shared
{
    using System.Linq.Expressions;
    using Data.Models;
    using Service.Models;
    using Web.Models;

    public static class Mapping
    {
        public static CreateArticleServiceModel ToServiceModel(
            this CreateArticleWebModel webModel)
            => new()
            { 
                Title = webModel.Title,
                Introduction = webModel.Introduction,
                Content = webModel.Content,
                Image = webModel.Image,
            };

        public static Article ToDbModel(
            this CreateArticleServiceModel serviceModel)
            => new()
            {
                Title = serviceModel.Title,
                Introduction = serviceModel.Introduction,
                Content = serviceModel.Content,
            };

        public static void UpdateDbModel(
            this CreateArticleServiceModel serviceModel,
            Article dbModel)
        {
            dbModel.Title = serviceModel.Title;
            dbModel.Introduction = serviceModel.Introduction;
            dbModel.Content = serviceModel.Content;
        }

        public static Expression<Func<Article, ArticleDetailsServiceModel>> ToDetailsServiceModelExpression =>
            dbModel => new ArticleDetailsServiceModel
            {
                Id = dbModel.Id,
                CreatedOn = dbModel.CreatedOn,
                Views = dbModel.Views,
                Title = dbModel.Title,
                Introduction = dbModel.Introduction,
                Content = dbModel.Content,
                ImagePath = dbModel.ImagePath,
            };

        public static ArticleDetailsServiceModel ToDetailsServiceModel(
            this Article dbModel)
            => new()
            {
                Id = dbModel.Id,
                CreatedOn = dbModel.CreatedOn,
                Views = dbModel.Views,
                Title = dbModel.Title,
                Introduction = dbModel.Introduction,
                Content = dbModel.Content,
                ImagePath = dbModel.ImagePath,
            };
    }
}
