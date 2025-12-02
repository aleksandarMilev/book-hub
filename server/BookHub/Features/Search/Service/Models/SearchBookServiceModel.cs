namespace BookHub.Features.Search.Service.Models
{
    using Genre.Service.Models;

    public class SearchBookServiceModel
    {
        public Guid Id { get; init; }

        public string Title { get; init; } = null!;

        public string? AuthorName { get; init; }

        public string ImagePath { get; init; } = null!;

        public string ShortDescription { get; init; } = null!;

        public double AverageRating { get; init; }

        public ICollection<GenreNameServiceModel> Genres { get; init; } = new HashSet<GenreNameServiceModel>();
    }
}
