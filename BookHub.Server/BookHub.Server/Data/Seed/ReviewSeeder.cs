namespace BookHub.Server.Data.Seed
{
    using Models;

    public static class ReviewSeeder
    {
        public static Review[] Seed()
            => new Review[]
            {
                new()
                {
                    Id = 1,
                    Content = "A truly chilling tale. King masterfully explores the dark side of human grief and love.",
                    Rating = 5,
                    CreatorId = "user1Id",
                    BookId = 1
                },
                new()
                {
                    Id = 2,
                    Content = "The book was gripping but felt a bit too disturbing at times for my taste.",
                    Rating = 3,
                    CreatorId = "user2Id",
                    BookId = 1
                },
                new()
                {
                    Id = 3,
                    Content = "An unforgettable story that haunts you long after you've finished it. Highly recommended!",
                    Rating = 5,
                    CreatorId = "user3Id",
                    BookId = 1
                },
                new()
                {
                    Id = 4,
                    Content = "The characters were well-developed, but the plot felt predictable toward the end.",
                    Rating = 4,
                    CreatorId = "user1Id",
                    BookId = 1
                },
                new()
                {
                    Id = 5,
                    Content = "An incredible conclusion to the series. Every twist and turn kept me on edge.",
                    Rating = 5,
                    CreatorId = "user2Id",
                    BookId = 2
                },
                new()
                {
                    Id = 6,
                    Content = "The Battle of Hogwarts was epic! A bittersweet yet satisfying ending.",
                    Rating = 5,
                    CreatorId = "user3Id",
                    BookId = 2
                },
                new()
                {
                    Id = 7,
                    Content = "I expected more from some of the character arcs, but still a solid read.",
                    Rating = 4,
                    CreatorId = "user1Id",
                    BookId = 2
                },
                new()
                {
                    Id = 8,
                    Content = "Rowling’s world-building continues to amaze, even in the final installment.",
                    Rating = 5,
                    CreatorId = "user2Id",
                    BookId = 2
                },
                new()
                {
                    Id = 9,
                    Content = "A timeless masterpiece. Tolkien’s world and characters are unmatched in depth and richness.",
                    Rating = 5,
                    CreatorId = "user3Id",
                    BookId = 3
                },
                new()
                {
                    Id = 10,
                    Content = "The pacing was slow at times, but the payoff in the end was well worth it.",
                    Rating = 4,
                    CreatorId = "user1Id",
                    BookId = 3
                },
                new()
                {
                    Id = 11,
                    Content = "The bond between Sam and Frodo is the heart of this epic journey. Beautifully written.",
                    Rating = 5,
                    CreatorId = "user2Id",
                    BookId = 3
                },
                new()
                {
                    Id = 12,
                    Content = "An epic tale that defines the fantasy genre. Loved every moment of it.",
                    Rating = 5,
                    CreatorId = "user3Id",
                    BookId = 3
                },
                new()
                {
                    Id = 13,
                    Content = "The attention to detail in Middle-earth is staggering. Tolkien is a true genius.",
                    Rating = 5,
                    CreatorId = "user1Id",
                    BookId = 3
                },
                new()
                {
                    Id = 14,
                    Content = "A bit long for my liking, but undeniably one of the greatest stories ever told.",
                    Rating = 4,
                    CreatorId = "user2Id",
                    BookId = 3
                }
            };
    }
}
