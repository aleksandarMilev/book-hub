﻿namespace BookHub.Features.Review.Service.Models
{
    public class ReviewServiceModel
    {
        public int Id { get; init; }

        public string Content { get; init; } = null!;

        public int Rating { get; init; }

        public int Upvotes { get; init; }

        public int Downvotes { get; init; }

        public string CreatorId { get; init; } = null!;

        public string CreatedBy { get; init; } = null!;

        public string CreatedOn { get; init; } = null!;

        public string? ModifiedOn { get; init; }

        public int BookId { get; init; }
    }
}
