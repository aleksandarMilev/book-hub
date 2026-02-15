namespace BookHub.Features.Challenges.Data.Models;

using BookHub.Data.Models.Base;
using Identity.Data.Models;
using Shared;

public class ReadingChallengeDbModel : DeletableEntity<int> 
{
    public string UserId { get; init; } = default!;

    public UserDbModel User { get; init; } = default!;

    public int Year { get; init; }

    public ReadingGoalType GoalType { get; set; }

    public int GoalValue { get; set; }
}
