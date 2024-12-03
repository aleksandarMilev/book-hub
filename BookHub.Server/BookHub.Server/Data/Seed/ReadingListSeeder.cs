namespace BookHub.Server.Data.Seed
{
    using Models;
    using Models.Enums;

    public static class ReadingListSeeder
    {
        public static ReadingList[] Seed()
            => new ReadingList[]
            {
                //user1
                new()
                {
                    UserId = "user1Id",
                    BookId = 1,
                    Status = ReadingListStatus.Read
                },
                new()
                {
                    UserId = "user1Id",
                    BookId = 2,
                    Status = ReadingListStatus.Read
                },
                new()
                {
                    UserId = "user1Id",
                    BookId = 3,
                    Status = ReadingListStatus.Read
                },
                new()
                {
                    UserId = "user1Id",
                    BookId = 4,
                    Status = ReadingListStatus.ToRead
                },
                new()
                {
                    UserId = "user1Id",
                    BookId = 5,
                    Status = ReadingListStatus.ToRead
                },
                new()
                {
                    UserId = "user1Id",
                    BookId = 6,
                    Status = ReadingListStatus.CurrentlyReading
                },
                //user2
                new()
                {
                    UserId = "user2Id",
                    BookId = 1,
                    Status = ReadingListStatus.Read
                },
                new()
                {
                    UserId = "user2Id",
                    BookId = 5,
                    Status = ReadingListStatus.Read
                },
                new()
                {
                    UserId = "user2Id",
                    BookId = 6,
                    Status = ReadingListStatus.Read
                },
                new()
                {
                    UserId = "user2Id",
                    BookId = 10,
                    Status = ReadingListStatus.ToRead
                },
                new()
                {
                    UserId = "user2Id",
                    BookId = 12,
                    Status = ReadingListStatus.CurrentlyReading
                },
                //user3
                new()
                {
                    UserId = "user3Id",
                    BookId = 10,
                    Status = ReadingListStatus.Read
                },
                new()
                {
                    UserId = "user3Id",
                    BookId = 7,
                    Status = ReadingListStatus.Read
                },
                new()
                {
                    UserId = "user3Id",
                    BookId = 8,
                    Status = ReadingListStatus.Read
                },
                new()
                {
                    UserId = "user3Id",
                    BookId = 4,
                    Status = ReadingListStatus.ToRead
                },
                new()
                {
                    UserId = "user3Id",
                    BookId = 12,
                    Status = ReadingListStatus.CurrentlyReading
                },
                new()
                {
                    UserId = "user3Id",
                    BookId = 13,
                    Status = ReadingListStatus.CurrentlyReading
                },
            };
    }
}
