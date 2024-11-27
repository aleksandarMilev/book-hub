namespace BookHub.Server.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Base;

    using static Common.Constants.Validation.Article;

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

        public int Views { get; init; }
    }
}
