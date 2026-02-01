namespace BookHub.Features.Articles.Web.Admin.Models;

using System.ComponentModel.DataAnnotations;
using Infrastructure.Validation;

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

    [ImageUpload]
    public IFormFile? Image { get; init; }
}
