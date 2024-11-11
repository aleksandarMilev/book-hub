namespace BookHub.Server.Features.Authors.Service.Models
{
    using Common;

    public class AuthorDetailsServiceModel
    {
        public int Id { get; init; }

        public string Name { get; init; } = null!;

        public string ImageUrl { get; init; } = null!;

        public string Biography { get; init; } = null!;

        public string? PenName { get; init; }

        public double Rating { get; init; }

        public Nationality Nationality { get; init; }

        public Gender Gender { get; init; }

        public DateTime? BornAt { get; init; }

        public DateTime? DiedAt { get; init; }

        public string? CreatorId { get; set; }
    }
}
