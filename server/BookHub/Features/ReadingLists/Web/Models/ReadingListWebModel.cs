namespace BookHub.Features.ReadingLists.Web.Models;

using System.ComponentModel.DataAnnotations;
using Shared;

public class ReadingListWebModel : IValidatableObject
{
    public Guid BookId { get; init; }

    public ReadingListStatus Status { get; init; }

    public IEnumerable<ValidationResult> Validate(
        ValidationContext validationContext)
    {
        if (!Enum.IsDefined(this.Status))
        {
            yield return new ValidationResult(
                "Invalid Stattus value.",
                [nameof(this.Status)]);
        }
    }
}
