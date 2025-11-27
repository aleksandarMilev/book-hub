namespace BookHub.Features.Article.Service.Models;

using BookHub.Features.Articles.Service.Models.Base;
using Infrastructure.Services.ImageWriter.Models.Image;

public class CreateArticleServiceModel : 
    ArticleBaseModel,
    IImageServiceModel
{
    public IFormFile? Image { get; init; }
}
