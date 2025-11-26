namespace BookHub.Features.Article.Data.Models;

using BookHub.Data.Models.Base;
using Infrastructure.Services.ImageWriter.Models.Image;
using System.ComponentModel.DataAnnotations;

using static Shared.Constants.ValidationConstants;

public class ArticleDbModel:
    DeletableEntity<Guid>,
    IImageDdModel
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

    public string ImagePath { get; set; } = null!;

    public int Views { get; set; }
}
