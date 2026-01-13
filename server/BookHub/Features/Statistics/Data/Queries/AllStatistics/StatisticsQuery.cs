namespace BookHub.Features.Statistics.Data.Queries.AllStatistics;

using BookHub.Data;
using Microsoft.EntityFrameworkCore;
using Models;

public class StatisticsQuery(BookHubDbContext data) : IStatisticsQuery
{
    public Task<StatisticsRow> All(
        CancellationToken cancellationToken)
    {
        const string Sql = """
        SELECT
          (SELECT COUNT(*) FROM Profiles) AS Profiles,
          (SELECT COUNT(*) FROM Books)    AS Books,
          (SELECT COUNT(*) FROM Authors)  AS Authors,
          (SELECT COUNT(*) FROM Reviews)  AS Reviews,
          (SELECT COUNT(*) FROM Genres)   AS Genres,
          (SELECT COUNT(*) FROM Articles) AS Articles
        """;

        return data
            .Set<StatisticsRow>()
            .FromSqlRaw(Sql)
            .AsNoTracking()
            .SingleAsync(cancellationToken);
    }
}
