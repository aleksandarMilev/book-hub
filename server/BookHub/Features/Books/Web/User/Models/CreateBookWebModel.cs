namespace BookHub.Features.Books.Web.Models;

using System.ComponentModel.DataAnnotations;

using static Shared.Constants.Validation;

public class CreateBookWebModel
{
    [Required]
    [StringLength(
        TitleMaxLength, 
        MinimumLength = TitleMinLength)]
    public string Title { get; init; } = null!;

    public Guid? AuthorId { get; init; }

    public IFormFile? Image { get; init; }

    [Required]
    [StringLength(
        ShortDescriptionMaxLength, 
        MinimumLength = ShortDescriptionMinLength)]
    public string ShortDescription { get; init; } = null!;

    [Required]
    [StringLength(
        LongDescriptionMaxLength, 
        MinimumLength = LongDescriptionMinLength)]
    public string LongDescription { get; init; } = null!;

    public string? PublishedDate { get; init; }

    public ICollection<Guid> Genres { get; init; } = new HashSet<Guid>();
}
