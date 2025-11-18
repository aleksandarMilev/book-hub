namespace BookHub.Features.Article.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using BookHub.Data.Models.Base;

    using static Shared.Constants.ValidationConstants;

    public class Article : DeletableEntity<string>
    {
        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(IntroductionMaxLength)]
        public string Introduction { get; set; } = null!;

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; } = null!;

        [MaxLength(UrlMaxLength)]
        public string? ImageUrl { get; set; }

        public int Views { get; set; }
    }
}
