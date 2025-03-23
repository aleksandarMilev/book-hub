﻿namespace BookHub.Features.Review.Service.Models
{
    public class CreateReviewServiceModel
    {
        public string Content { get; set; } = null!;

        public int Rating { get; init; }

        public int BookId { get; init; }
    }
}
