namespace BookHub.Server.Features.Authors.Service.Models
{
    public class AuthorDetailsServiceModel
    {
        public int Id { get; init; }

        public string Name { get; init; } = null!;

        public string ImageUrl { get; init; } = null!;

        public string Biography { get; init; } = null!;

        public string? PenName { get; init; }

        public double Rating { get; init; }

        public string Nationality { get; init; } = null!;

        public string Gender { get; init; } = null!;

        public string? BornAt { get; init; }

        public string? DiedAt { get; init; }

        public string? CreatorId { get; set; }
    }
}
