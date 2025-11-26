namespace BookHub.Features.Article.Web.Models;

using System.ComponentModel.DataAnnotations;

using static Shared.Constants.ValidationConstants;

public class CreateArticleWebModel
{
    [Required]
    [StringLength(
        TitleMaxLength,
        MinimumLength = TitleMinLength)]
    public string Title { get; init; } = null!;

    [Required]
    [StringLength(
        IntroductionMaxLength,
        MinimumLength = IntroductionMinLength)]
    public string Introduction { get; init; } = null!;

    [Required]
    [StringLength(
        ContentMaxLength,
        MinimumLength = ContentMinLength)]
    public string Content { get; init; } = null!;

    public IFormFile? Image { get; init; }
}
