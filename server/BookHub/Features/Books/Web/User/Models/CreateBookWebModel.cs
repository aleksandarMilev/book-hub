namespace BookHub.Features.Books.Web.User.Models;

using System.ComponentModel.DataAnnotations;
using Infrastructure.Validation;

using static Shared.Constants.Validation;

public class CreateBookWebModel
{
    [Required]
    [StringLength(
        TitleMaxLength, 
        MinimumLength = TitleMinLength)]
    public string Title { get; init; } = default!;

    public Guid? AuthorId { get; init; }

    [ImageUpload]
    public IFormFile? Image { get; init; }

    [Required]
    [StringLength(
        ShortDescriptionMaxLength, 
        MinimumLength = ShortDescriptionMinLength)]
    public string ShortDescription { get; init; } = default!;

    [Required]
    [StringLength(
        LongDescriptionMaxLength, 
        MinimumLength = LongDescriptionMinLength)]
    public string LongDescription { get; init; } = default!;

    public DateTime? PublishedDate { get; init; }

    public ICollection<Guid> Genres { get; init; } = new HashSet<Guid>();
}
