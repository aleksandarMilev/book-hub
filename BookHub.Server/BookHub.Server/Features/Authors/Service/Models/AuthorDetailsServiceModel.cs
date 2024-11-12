namespace BookHub.Server.Features.Authors.Service.Models
{
    public class AuthorDetailsServiceModel : AuthorServiceModel
    {
        public string? PenName { get; init; }

        public double Rating { get; init; }

        public string Nationality { get; init; } = null!;

        public string Gender { get; init; } = null!;

        public string? BornAt { get; init; }

        public string? DiedAt { get; init; }

        public string? CreatorId { get; set; }
    }
}
