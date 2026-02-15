namespace BookHub.Features.Challenges.Service.Models;

using Shared;

public class UpsertReadingChallengeServiceModel
{
    public int Year { get; init; }

    public ReadingGoalType GoalType { get; init; }

    public int GoalValue { get; init; }
}
