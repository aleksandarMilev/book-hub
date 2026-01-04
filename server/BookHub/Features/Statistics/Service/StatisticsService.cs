namespace BookHub.Features.Statistics.Service;

using Data;
using Microsoft.EntityFrameworkCore;
using Models;

public class StatisticsService(BookHubDbContext data) : IStatisticsService
{
    public async Task<StatisticsServiceModel> All(
        CancellationToken cancellationToken = default)
    {
        var users = await data
            .Users
            .CountAsync(cancellationToken);

        var books = await data
            .Books
            .CountAsync(cancellationToken);

        var authors = await data
            .Authors
            .CountAsync(cancellationToken);

        var reviews = await data
            .Reviews
            .CountAsync(cancellationToken);

        var genres = await data
            .Genres
            .CountAsync(cancellationToken);

        var articles = await data
            .Articles
            .CountAsync(cancellationToken);

        return new StatisticsServiceModel(
            users,
            books,
            authors,
            reviews,
            genres,
            articles);
    }
}
