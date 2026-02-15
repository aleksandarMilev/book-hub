namespace BookHub.Features.Challenges.Service.Models;

public class ReadingStreakServiceModel
{
    public int CurrentStreak { get; init; }

    public int LongestStreak { get; init; }

    public bool CheckedInToday { get; init; }

    public DateOnly Today { get; init; }
}
