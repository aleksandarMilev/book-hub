namespace BookHub.Server.Features.Article.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Server.Data.Models.Base;

    using static Shared.ValidationConstants;

    public class Article : DeletableEntity<int>
    {
        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; init; } = null!;

        [Required]
        [MaxLength(IntroductionMaxLength)]
        public string Introduction { get; init; } = null!;

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; init; } = null!;

        [MaxLength(UrlMaxLength)]
        public string? ImageUrl { get; set; }

        public int Views { get; set; }
    }
}
