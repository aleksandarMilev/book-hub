namespace BookHub.Features.Statistics.Service.Models;

public class StatisticsServiceModel(
    int profiles,
    int books,
    int authors,
    int reviews,
    int genres,
    int articles)
{
    public int Profiles { get; init; } = profiles;

    public int Books { get; init; } = books;

    public int Authors { get; init; } = authors;

    public int Reviews { get; init; } = reviews;

    public int Genres { get; init; } = genres;

    public int Articles { get; init; } = articles;
}
