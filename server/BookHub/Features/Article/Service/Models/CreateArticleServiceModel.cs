namespace BookHub.Features.Article.Service.Models
{
    using Common.Models.Image;

    public class CreateArticleServiceModel : IImageServiceModel
    {
        public string Title { get; init; } = null!;

        public string Introduction { get; init; } = null!;

        public string Content { get; init; } = null!;

        public IFormFile? Image { get; set; }
    }
}
