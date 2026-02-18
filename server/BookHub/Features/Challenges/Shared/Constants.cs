namespace BookHub.Features.Challenges.Shared;

public static class Constants
{
    public static class ErrorMessages
    {
        public const string InvalidYear = "Invalid year.";

        public const string InvalidGoal = "Invalid goal value.";

        public const string InvalidGoalType = "Invalid goal type.";

        public const string ChallengeAlreadyExists = "Challenge for this year already exists.";

        public const string CheckInAlreadyExists = "You already checked in for this date.";
    }

    public static class DefaultValues
    {
        public const int MinYear = 2_000;

        public const int MaxYear = 2_100;

        public const int MinGoalValue = 1;
    }
}
