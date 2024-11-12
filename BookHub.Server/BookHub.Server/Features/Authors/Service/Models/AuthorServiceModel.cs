namespace BookHub.Server.Features.Authors.Service.Models
{
    public class AuthorServiceModel
    {
        public int Id { get; init; }

        public string Name { get; init; } = null!;

        public string ImageUrl { get; init; } = null!;

        public string Biography { get; init; } = null!;

        public int BooksCount { get; init; }
    }
}
