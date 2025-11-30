namespace BookHub.Features.Article.Shared;

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

    public static ArticleDbModel ToDbModel(
        this CreateArticleServiceModel serviceModel)
        => new()
        {
            Title = serviceModel.Title,
            Introduction = serviceModel.Introduction,
            Content = serviceModel.Content,
        };

    public static void UpdateDbModel(
        this CreateArticleServiceModel serviceModel,
        ArticleDbModel dbModel)
    {
        dbModel.Title = serviceModel.Title;
        dbModel.Introduction = serviceModel.Introduction;
        dbModel.Content = serviceModel.Content;
    }

    public static IQueryable<ArticleDetailsServiceModel> ToServiceDetailsModels(
        this IQueryable<ArticleDbModel> articles)
        => articles.Select(a => new ArticleDetailsServiceModel
        {
            Id = a.Id,
            Views = a.Views,
            Title = a.Title,
            Introduction = a.Introduction,
            Content = a.Content,
            ImagePath = a.ImagePath,
            CreatedOn = a.CreatedOn,
            ModifiedOn = a.ModifiedOn
        });

    public static ArticleDetailsServiceModel ToDetailsServiceModel(
        this ArticleDbModel dbModel)
        => new()
        {
            Id = dbModel.Id,
            Views = dbModel.Views,
            Title = dbModel.Title,
            Introduction = dbModel.Introduction,
            Content = dbModel.Content,
            ImagePath = dbModel.ImagePath,
            CreatedOn = dbModel.CreatedOn,
            ModifiedOn = dbModel.ModifiedOn,
        };
}
