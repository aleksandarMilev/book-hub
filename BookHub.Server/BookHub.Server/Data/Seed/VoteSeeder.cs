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
            };
    }
}
