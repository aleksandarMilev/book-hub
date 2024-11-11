namespace BookHub.Server.Features.Authors.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    using Common;

    using static Server.Common.Constants.Validation.Author;

    public class CreateAuthorWebModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; init; } = null!;

        [Required]
        [StringLength(ImageUrlMaxLength, MinimumLength = ImageUrlMinLength)]
        public string ImageUrl { get; init; } = null!;

        [Required]
        [StringLength(BiographyMaxLength, MinimumLength = BiographyMinLength)]
        public string Biography { get; init; } = null!;

        [StringLength(PenNameMaxLength, MinimumLength = PenNameMinLength)]
        public string? PenName { get; init; }

        public double Rating { get; init; }

        public Nationality Nationality { get; init; }

        public Gender Gender { get; init; }

        public DateTime? BornAt { get; init; }

        public DateTime? DiedAt { get; init; }
    }
}
