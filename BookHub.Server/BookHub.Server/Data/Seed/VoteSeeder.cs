namespace BookHub.Server.Data.Seed
{
    using Models;

    public static class VoteSeeder
    {
        public static Vote[] Seed()
            => new Vote[]
            {
                new()
                {
                    Id = 1,
                    IsUpvote = true,
                    ReviewId = 1,
                    CreatorId = "user2Id"
                },
                new()
                {
                    Id = 2,
                    IsUpvote = true,
                    ReviewId = 1,
                    CreatorId = "user3Id"
                },
                new()
                {
                    Id = 3,
                    IsUpvote = true,
                    ReviewId = 1,
                    CreatorId = "user4Id"
                },
                new()
                {
                    Id = 4,
                    IsUpvote = true,
                    ReviewId = 1,
                    CreatorId = "user5Id"
                },
                new()
                {
                    Id = 5,
                    IsUpvote = false,
                    ReviewId = 1,
                    CreatorId = "user6Id"
                },
                new()
                {
                    Id = 6,
                    IsUpvote = true,
                    ReviewId = 2,
                    CreatorId = "user2Id"
                },
                new()
                {
                    Id = 7,
                    IsUpvote = true,
                    ReviewId = 2,
                    CreatorId = "user3Id"
                },
                new()
                {
                    Id = 8,
                    IsUpvote = true,
                    ReviewId = 15,
                    CreatorId = "user2Id"
                },
                new()
                {
                    Id = 9,
                    IsUpvote = true,
                    ReviewId = 15,
                    CreatorId = "user3Id"
                },
                new()
                {
                    Id = 10,
                    IsUpvote = true,
                    ReviewId = 15,
                    CreatorId = "user4Id"
                },
                new()
                {
                    Id = 11,
                    IsUpvote = true,
                    ReviewId = 15,
                    CreatorId = "user3Id"
                },
                new()
                {
                    Id = 12,
                    IsUpvote = true,
                    ReviewId = 20,
                    CreatorId = "user3Id"
                },
                new()
                {
                    Id = 13,
                    IsUpvote = true,
                    ReviewId = 20,
                    CreatorId = "user3Id"
                },
                new()
                {
                    Id = 14,
                    IsUpvote = true,
                    ReviewId = 20,
                    CreatorId = "user3Id"
                },
                new()
                {
                    Id = 15,
                    IsUpvote = true,
                    ReviewId = 26,
                    CreatorId = "user2Id"
                },
                new()
                {
                    Id = 16,
                    IsUpvote = true,
                    ReviewId = 26,
                    CreatorId = "user3Id"
                },
                new()
                {
                    Id = 17,
                    IsUpvote = true,
                    ReviewId = 26,
                    CreatorId = "user4Id"
                },
                //dracula
                new()
                {
                    Id = 18,
                    IsUpvote = true,
                    ReviewId = 32,
                    CreatorId = "user1Id"
                },
                new()
                {
                    Id = 19,
                    IsUpvote = true,
                    ReviewId = 32,
                    CreatorId = "user2Id"
                },
                new()
                {
                    Id = 20,
                    IsUpvote = true,
                    ReviewId = 32,
                    CreatorId = "user3Id"
                },
                new()
                {
                    Id = 21,
                    IsUpvote = true,
                    ReviewId = 32,
                    CreatorId = "user4Id"
                },
                new()
                {
                    Id = 22,
                    IsUpvote = true,
                    ReviewId = 32,
                    CreatorId = "user5Id"
                },
                new()
                {
                    Id = 23,
                    IsUpvote = false,
                    ReviewId = 32,
                    CreatorId = "user6Id"
                },
                //it
                new()
                {
                    Id = 24,
                    IsUpvote = true,
                    ReviewId = 37,
                    CreatorId = "user1Id"
                },
                new()
                {
                    Id = 25,
                    IsUpvote = true,
                    ReviewId = 37,
                    CreatorId = "user2Id"
                },
                new()
                {
                    Id = 26,
                    IsUpvote = true,
                    ReviewId = 37,
                    CreatorId = "user3Id"
                },
                //animal farm
                new()
                {
                    Id = 27,
                    IsUpvote = true,
                    ReviewId = 41,
                    CreatorId = "user1Id"
                },
                new()
                {
                    Id = 28,
                    IsUpvote = true,
                    ReviewId = 41,
                    CreatorId = "user2Id"
                },
                new()
                {
                    Id = 29,
                    IsUpvote = true,
                    ReviewId = 41,
                    CreatorId = "user3Id"
                },
                //clockwork orange
                new()
                {
                    Id = 30,
                    IsUpvote = true,
                    ReviewId = 43,
                    CreatorId = "user1Id"
                },
                new()
                {
                    Id = 31,
                    IsUpvote = true,
                    ReviewId = 43,
                    CreatorId = "user2Id"
                },
                new()
                {
                    Id = 32,
                    IsUpvote = true,
                    ReviewId = 43,
                    CreatorId = "user3Id"
                },
                //orient express
                new()
                {
                    Id = 33,
                    IsUpvote = true,
                    ReviewId = 49,
                    CreatorId = "user2Id"
                },
                //roger acroyd
                new()
                {
                    Id = 34,
                    IsUpvote = true,
                    ReviewId = 53,
                    CreatorId = "user2Id"
                },
                //east of eden
                new()
                {
                    Id = 35,
                    IsUpvote = true,
                    ReviewId = 56,
                    CreatorId = "user2Id"
                },
                //dune
                new()
                {
                    Id = 36,
                    IsUpvote = true,
                    ReviewId = 60,
                    CreatorId = "user2Id"
                },
                //hitchhiker
                new()
                {
                    Id = 37,
                    IsUpvote = true,
                    ReviewId = 63,
                    CreatorId = "user2Id"
                },
            };
    }
}
