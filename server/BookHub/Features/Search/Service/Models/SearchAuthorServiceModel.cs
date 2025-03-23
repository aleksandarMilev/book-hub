﻿namespace BookHub.Features.Search.Service.Models
{
    public class SearchAuthorServiceModel
    {
        public int Id { get; init; }

        public string Name { get; init; } = null!;

        public string? PenName { get; init; }

        public string ImageUrl { get; init; } = null!;

        public double AverageRating { get; init; }
    }
}
