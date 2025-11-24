namespace BookHub.Features.Article.Service.Models
{
    using Common.Models.Image;

    public class CreateArticleServiceModel : 
        ArticleServiceModel,
        IImageServiceModel
    {
        public IFormFile? Image { get; set; }
    }
}
