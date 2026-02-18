namespace BookHub.Features.Challenges.Service;

using BookHub.Data;
using Data.Models;
using Features.ReadingLists.Shared;
using Infrastructure.Services.CurrentUser;
using Infrastructure.Services.Result;
using Microsoft.EntityFrameworkCore;
using Models;
using Shared;

using static Shared.Constants.DefaultValues;
using static Shared.Constants;

public class ChallengeService(
    BookHubDbContext data,
    ICurrentUserService userService) : IReadingChallengeService
{
    public async Task<ReadingChallengeServiceModel?> Get(
        int year,
        CancellationToken cancellationToken = default)
    {
        if (!YearIsValid(year))
        {
            return null;
        }

        return await data
            .ReadingChallenges
            .AsNoTracking()
            .Where(c => 
                c.UserId == userService.GetId() && 
                c.Year == year)
            .ToServiceModels()
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Result> Upsert(
        UpsertReadingChallengeServiceModel model,
        CancellationToken cancellationToken = default)
    {
        if (!YearIsValid(model.Year))
        {
            return ErrorMessages.InvalidYear;
        }

        if (model.GoalValue < MinGoalValue)
        {
            return ErrorMessages.InvalidGoal;
        }

        if (!Enum.IsDefined(model.GoalType))
        {
            return ErrorMessages.InvalidGoalType;
        }

        var userId = userService.GetId();
        var dbModel = await data
            .ReadingChallenges
            .FirstOrDefaultAsync(
                c => c.UserId == userId && c.Year == model.Year,
                cancellationToken);

        if (dbModel is null)
        {
            var dbModel = new ReadingChallengeDbModel
            {
                UserId = userId!,
                Year = model.Year,
                GoalType = model.GoalType,
                GoalValue = model.GoalValue
            };

            data.ReadingChallenges.Add(dbModel);
        }
        else
        {
            dbModel.GoalType = model.GoalType;
            dbModel.GoalValue = model.GoalValue;
        }

        await data.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<ReadingChallengeProgressServiceModel?> Progress(
        int year,
        CancellationToken cancellationToken = default)
    {
        if (!YearIsValid(year))
        {
            return null;
        }

        var userId = userService.GetId();
        var dbModel = await data
            .ReadingChallenges
            .AsNoTracking()
            .FirstOrDefaultAsync(
                c => c.UserId == userId && c.Year == year,
                cancellationToken);

        if (dbModel is null)
        {
            return null;
        }

        var readingListsWithStatusRead = data
            .ReadingLists
            .AsNoTracking()
            .Where(
                rl => rl.UserId == userId &&
                rl.Status == ReadingListStatus.Read);

        var currentBooksInTheReadingList = await readingListsWithStatusRead
            .CountAsync(cancellationToken);

        var readingChallengeCurrentValue = currentBooksInTheReadingList;
        if (dbModel.GoalType == ReadingGoalType.Pages)
        {
            readingChallengeCurrentValue = await readingListsWithStatusRead
                .Select(rl => rl.Book.Pages)
                .SumAsync(p => p ?? 0, cancellationToken);
        }

        return dbModel.ToServiceModel(readingChallengeCurrentValue);
    }

    public async Task<Result> CheckInToday(
        CancellationToken cancellationToken = default)
    {
        var userId = userService.GetId()!;
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var dbModelExists = await data
            .ReadingCheckIns
            .AsNoTracking()
            .AnyAsync(
                c => c.UserId == userId && c.Date == today,
                cancellationToken);

        if (dbModelExists)
        {
            return ErrorMessages.CheckInAlreadyExists;
        }

        data.ReadingCheckIns.Add(new()
        {
            UserId = userId,
            Date = today
        });

        await data.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<ReadingStreakServiceModel> Streak(
        CancellationToken cancellationToken = default)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var dates = await data
            .ReadingCheckIns
            .AsNoTracking()
            .Where(c => c.UserId == userService.GetId())
            .OrderByDescending(c => c.Date)
            .Select(c => c.Date)
            .ToListAsync(cancellationToken);

        var checkedInToday = dates.Count > 0 && dates[0] == today;

        var currentStreak = ComputeCurrentStreak(dates, today);
        var longestStreak = ComputeLongestStreak(dates);

        return new()
        {
            CurrentStreak = currentStreak,
            LongestStreak = longestStreak,
            CheckedInToday = checkedInToday,
            Today = today
        };
    }

    private static bool YearIsValid(int year)
        => year >= MinYear && year <= MaxYear;

    private static int ComputeCurrentStreak(
        List<DateOnly> datesDescending,
        DateOnly today)
    {
        if (datesDescending.Count == 0)
        {
            return 0;
        }

        var datesAsSet = new HashSet<DateOnly>(datesDescending);
        var start = datesAsSet.Contains(today)
            ? today
            : today.AddDays(-1);

        var streak = 0;
        var cursor = start;

        while (datesAsSet.Contains(cursor))
        {
            streak++;
            cursor = cursor.AddDays(-1);
        }

        return streak;
    }

    private static int ComputeLongestStreak(List<DateOnly> datesDescending)
    {
        if (datesDescending.Count == 0)
        {
            return 0;
        }

        var unique = datesDescending
            .Distinct()
            .OrderBy(d => d)
            .ToList();

        var best = 1;
        var current = 1;

        for (var i = 1; i < unique.Count; i++)
        {
            var previous = unique[i - 1];
            var curr = unique[i];

            if (curr.DayNumber == previous.DayNumber + 1)
            {
                current++;
                best = Math.Max(best, current);
            }
            else
            {
                current = 1;
            }
        }

        return best;
    }
}
