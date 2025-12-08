namespace BookHub.Features.Statistics.Service;

using Data;
using Microsoft.EntityFrameworkCore;
using Models;

public class StatisticsService(BookHubDbContext data) : IStatisticsService
{
    public async Task<StatisticsServiceModel> All(CancellationToken token = default)
    {
        var users = await data
            .Users
            .CountAsync(token);

        var books = await data
            .Books
            .CountAsync(token);

        var authors = await data
            .Authors
            .CountAsync(token);

        var reviews = await data
            .Reviews
            .CountAsync(token);

        var genres = await data
            .Genres
            .CountAsync(token);

        var articles = await data
            .Articles
            .CountAsync(token);

        return new StatisticsServiceModel(
            users,
            books,
            authors,
            reviews,
            genres,
            articles);
    }
}
