namespace BookHub.Features.Statistics.Data.Queries.AllStatistics;

using BookHub.Data;
using Microsoft.EntityFrameworkCore;
using Models;

public class StatisticsQuery(BookHubDbContext data) : IStatisticsQuery
{
    // One round trip: six scalar subqueries projected over a one-row anchor.
    // The filters are explicit (not the global query filters) because the result
    // is cached for every caller, and an admin's filters would include unapproved rows.
    public Task<StatisticsRow> All(
        CancellationToken cancellationToken)
        => data
            .Database
            .SqlQueryRaw<int>("SELECT 1 AS \"Value\"")
            .Select(_ => new StatisticsRow
            {
                Profiles = data.Profiles
                    .IgnoreQueryFilters()
                    .Count(p => !p.IsDeleted),

                Books = data.Books
                    .IgnoreQueryFilters()
                    .Count(b => !b.IsDeleted && b.IsApproved),

                Authors = data.Authors
                    .IgnoreQueryFilters()
                    .Count(a => !a.IsDeleted && a.IsApproved),

                Reviews = data.Reviews
                    .IgnoreQueryFilters()
                    .Count(r => !r.IsDeleted),

                Genres = data.Genres
                    .IgnoreQueryFilters()
                    .Count(g => !g.IsDeleted),

                Articles = data.Articles
                    .IgnoreQueryFilters()
                    .Count(a => !a.IsDeleted),
            })
            .SingleAsync(cancellationToken);
}
