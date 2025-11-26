namespace BookHub.Features.Article.Service.Models;

using Infrastructure.Services.ImageWriter.Models.Image;


public class CreateArticleServiceModel : 
    ArticleServiceModel,
    IImageServiceModel
{
    public IFormFile? Image { get; set; }
}
