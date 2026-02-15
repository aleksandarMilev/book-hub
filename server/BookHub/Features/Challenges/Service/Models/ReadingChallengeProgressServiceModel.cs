namespace BookHub.Features.Challenges.Service.Models;

using Shared;

public class ReadingChallengeProgressServiceModel
{
    public int Year { get; init; }

    public ReadingGoalType GoalType { get; init; }

    public int GoalValue { get; init; }

    public int CurrentValue { get; init; }

    public double ProgressPercent
        => this.GoalValue <= 0
            ? 0
            : Math.Min(100.0, (double)this.CurrentValue / this.GoalValue * 100.0);
}
