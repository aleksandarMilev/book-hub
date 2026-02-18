namespace BookHub.Features.Challenges.Web.Models;

using System.ComponentModel.DataAnnotations;
using Shared;

using static Shared.Constants;

public class UpsertReadingChallengeWebModel : IValidatableObject
{
    [Range(
        DefaultValues.MinYear,
        DefaultValues.MaxYear)]
    public int Year { get; init; }

    public ReadingGoalType GoalType { get; init; }

    [Range(
        DefaultValues.MinGoalValue,
        DefaultValues.MaxGoalValue)]
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
