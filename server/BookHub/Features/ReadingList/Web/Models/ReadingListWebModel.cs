namespace BookHub.Features.ReadingList.Web.Models;

using System.ComponentModel.DataAnnotations;
using Data.Models;

public class ReadingListWebModel : IValidatableObject
{
    public Guid BookId { get; init; }

    public ReadingListStatus Status { get; init; }

    public IEnumerable<ValidationResult> Validate(
        ValidationContext validationContext)
    {
        if (!Enum.IsDefined(typeof(ReadingListStatus), this.Status))
        {
            yield return new ValidationResult(
                "Invalid Stattus value.",
                [nameof(this.Status)]);
        }
    }
}
