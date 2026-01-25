namespace BookHub.Features.Article.Web.Models;

using System.ComponentModel.DataAnnotations;

using static Shared.Constants.Validation;

public class CreateArticleWebModel
{
    [Required]
    [StringLength(
        TitleMaxLength,
        MinimumLength = TitleMinLength)]
    public string Title { get; init; } = default!;

    [Required]
    [StringLength(
        IntroductionMaxLength,
        MinimumLength = IntroductionMinLength)]
    public string Introduction { get; init; } = default!;

    [Required]
    [StringLength(
        ContentMaxLength,
        MinimumLength = ContentMinLength)]
    public string Content { get; init; } = default!;

    public IFormFile? Image { get; init; }
}
