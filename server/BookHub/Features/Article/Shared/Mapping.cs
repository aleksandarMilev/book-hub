namespace BookHub.Features.Article.Shared
{
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
                ImageUrl = webModel.ImageUrl,
            };

        public static Article ToDbModel(
            this CreateArticleServiceModel serviceModel)
            => new()
            {
                Title = serviceModel.Title,
                Introduction = serviceModel.Introduction,
                Content = serviceModel.Content,
                ImageUrl = serviceModel.ImageUrl,
            };

        public static void UpdateDbModel(
            this CreateArticleServiceModel serviceModel,
            Article dbModel)
        {
            dbModel.Title = serviceModel.Title;
            dbModel.Introduction = serviceModel.Introduction;
            dbModel.Content = serviceModel.Content;
            dbModel.ImageUrl = serviceModel.ImageUrl;
        }

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
                ImageUrl = dbModel.ImageUrl,
            };
    }
}
