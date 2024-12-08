namespace BookHub.Server.Features.UserProfile.Data.Seed
{
    using Models;

    public static class ProfileSeeder
    {
        public static UserProfile[] Seed()
            => new UserProfile[]
            {
                new()
                {
                    UserId = "user1Id",
                    FirstName = "John",
                    LastName = "Doe",
                    ImageUrl = "https://www.shareicon.net/data/512x512/2016/05/24/770117_people_512x512.png",
                    PhoneNumber = "+1234567890",
                    DateOfBirth = new DateTime(1990, 05, 15),
                    SocialMediaUrl = "https://twitter.com/johndoe",
                    Biography = "John is a passionate reader and a book reviewer.",
                    CreatedBooksCount = 0,
                    CreatedAuthorsCount = 0,
                    ReviewsCount = 15,
                    ReadBooksCount = 3,
                    ToReadBooksCount = 2,
                    CurrentlyReadingBooksCount = 1,
                    IsPrivate = false,
                },
                new()
                {
                    UserId = "user2Id",
                    FirstName = "Alice",
                    LastName = "Smith",
                    ImageUrl = "https://static.vecteezy.com/system/resources/previews/002/002/257/non_2x/beautiful-woman-avatar-character-icon-free-vector.jpg",
                    PhoneNumber = "+1987654321",
                    DateOfBirth = new DateTime(1985, 07, 22),
                    SocialMediaUrl = "https://facebook.com/alicesmith",
                    Biography = "Alice enjoys exploring fantasy and sci-fi genres.",
                    CreatedBooksCount = 0,
                    CreatedAuthorsCount = 0,
                    ReviewsCount = 15,
                    ReadBooksCount = 3,
                    ToReadBooksCount = 1,
                    CurrentlyReadingBooksCount = 1,
                    IsPrivate = false,
                },
                new()
                {
                    UserId = "user3Id",
                    FirstName = "Bob",
                    LastName = "Johnson",
                    ImageUrl = "https://cdn1.iconfinder.com/data/icons/user-pictures/101/malecostume-512.png",
                    PhoneNumber = "+1122334455",
                    DateOfBirth = new DateTime(2000, 11, 01),
                    SocialMediaUrl = "https://instagram.com/bobjohnson",
                    Biography = "Bob is a new reader with a love for thrillers and mysteries.",
                    CreatedBooksCount = 0,
                    CreatedAuthorsCount = 0,
                    ReviewsCount = 14,
                    ReadBooksCount = 3,
                    ToReadBooksCount = 1,
                    CurrentlyReadingBooksCount = 2,
                    IsPrivate = false,
                }
            };
    }
}
