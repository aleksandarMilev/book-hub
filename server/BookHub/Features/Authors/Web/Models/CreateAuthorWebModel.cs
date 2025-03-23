﻿namespace BookHub.Features.Authors.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Shared.ValidationConstants.Author;

    public class CreateAuthorWebModel
    {
        [Required]
        [StringLength(
            NameMaxLength,
            MinimumLength = NameMinLength)]
        public string Name { get; init; } = null!;

        [StringLength(
            ImageUrlMaxLength,
            MinimumLength = ImageUrlMinLength)]
        public string? ImageUrl { get; init; }

        [Required]
        [StringLength(
            BiographyMaxLength,
            MinimumLength = BiographyMinLength)]
        public string Biography { get; init; } = null!;

        [StringLength(
            PenNameMaxLength,
            MinimumLength = PenNameMinLength)]
        public string? PenName { get; init; }

        public int? NationalityId { get; init; }

        public string Gender { get; init; } = null!;

        public string? BornAt { get; init; }

        public string? DiedAt { get; init; }
    }
}
