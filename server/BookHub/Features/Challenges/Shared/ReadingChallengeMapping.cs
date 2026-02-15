namespace BookHub.Features.Challenges.Shared;

using BookHub.Features.Challenges.Web.Models;
using Data.Models;
using Service.Models;

public static class ReadingChallengeMapping
{
    public static IQueryable<ReadingChallengeServiceModel> ToServiceModels(
        this IQueryable<ReadingChallengeDbModel> challenges)
        => challenges.Select(c => new ReadingChallengeServiceModel
        {
            Year = c.Year,
            GoalType = c.GoalType,
            GoalValue = c.GoalValue
        });

    public static IQueryable<ReadingCheckInServiceModel> ToServiceModels(
        this IQueryable<ReadingCheckInDbModel> checkIns)
        => checkIns.Select(c => new ReadingCheckInServiceModel
        {
            Date = c.Date
        });

    public static UpsertReadingChallengeServiceModel ToServiceModel(
        this UpsertReadingChallengeWebModel webModel)
        => new()
        {
            Year = webModel.Year,
            GoalType = webModel.GoalType,
            GoalValue = webModel.GoalValue
        };
}
