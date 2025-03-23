namespace BookHub.Features.Statistics.Service
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class StatisticsService(BookHubDbContext data) : IStatisticsService
    {
        private readonly BookHubDbContext data = data;

        public async Task<StatisticsServiceModel> All()
        {
            var users = await this.data
                .Users
                .CountAsync();

            var books = await this.data
                .Books
                .CountAsync();

            var authors = await this.data
                .Authors
                .CountAsync();

            var reviews = await this.data
                .Reviews
                .CountAsync();

            var genres = await this.data
                .Genres
                .CountAsync();

            var articles = await this.data
                .Articles
                .CountAsync();

            return new StatisticsServiceModel(
                users,
                books,
                authors,
                reviews,
                genres,
                articles);
        }
    }
}
