namespace BookHub.Server.Features.Statistics.Service.Models
{
    public class StatisticsServiceModel(
        int users,
        int books,
        int authors,
        int reviews,
        int genres,
        int articles)
    {
        public int Users { get; init; } = users;

        public int Books { get; init; } = books;

        public int Authors { get; init; } = authors;

        public int Reviews { get; init; } = reviews;

        public int Genres { get; init; } = genres;

        public int Articles { get; init; } = articles;
    }
}
