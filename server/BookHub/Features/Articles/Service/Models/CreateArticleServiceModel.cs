namespace BookHub.Features.Articles.Service.Models;

using Base;
using Infrastructure.Services.ImageWriter.Models.Image;

public class CreateArticleServiceModel : 
    ArticleBaseModel,
    IImageServiceModel
{
    public IFormFile? Image { get; init; }
}
