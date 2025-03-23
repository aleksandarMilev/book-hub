namespace BookHub.Features.Authors.Service.Models
{
    public class CreateAuthorServiceModel
    {
        public string Name { get; init; } = null!;

        public string? ImageUrl { get; set; }

        public string Biography { get; init; } = null!;

        public string? PenName { get; init; }

        public int? NationalityId { get; init; }

        public string Gender { get; init; } = null!;

        public string? BornAt { get; init; }

        public string? DiedAt { get; init; }
    }
}
