namespace BookHub.Server.Features.Article.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Common.Constants.Validation.Article;

    public class CreateArticleWebModel
    {
        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; init; } = null!;

        [Required]
        [StringLength(IntroductionMaxLength, MinimumLength = IntroductionMinLength)]
        public string Introduction { get; init; } = null!;

        [Required]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength)]
        public string Content { get; init; } = null!;

        [StringLength(UrlMaxLength, MinimumLength = UrlMinLength)]
        public string? ImageUrl { get; init; }
    }
}
