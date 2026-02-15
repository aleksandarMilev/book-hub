namespace BookHub.Features.Challenges.Web.Models;

using System.ComponentModel.DataAnnotations;
using Shared;

public class UpsertReadingChallengeWebModel : IValidatableObject
{
    [Range(2_000, 2_100)]
    public int Year { get; init; }

    public ReadingGoalType GoalType { get; init; }

    [Range(1, int.MaxValue)]
    public int GoalValue { get; init; }

    public IEnumerable<ValidationResult> Validate(
        ValidationContext validationContext)
    {
        if (!Enum.IsDefined(this.GoalType))
        {
            yield return new ValidationResult(
                "Invalid GoalType value.",
                [nameof(this.GoalType)]);
        }
    }
}
