namespace BookHub.Server.Data.Seed
{
    using Models;
    using Models.Enums;

    public static class AuthorsSeeder
    {
        public static Author[] Seed() 
            => new Author[]
            {
                new()
                {
                    Id = 1,
                    Name = "Stephen King",
                    ImageUrl = "https://media.npr.org/assets/img/2020/04/07/stephen-king.by-shane-leonard_wide-c132de683d677c7a6a1e692f86a7e6670baf3338.jpg?s=1100&c=85&f=jpeg",
                    Biography = "Stephen Edwin King (born September 21, 1947) is an American author. " +
                    "Widely known for his horror novels, he has been crowned the \"King of Horror\". " +
                    "He has also explored other genres, among them suspense, crime, science-fiction, fantasy and mystery. " +
                    "Though known primarily for his novels, he has written approximately 200 short stories, " +
                    "most of which have been published in collections.",
                    PenName = "Richard Bachman",
                    AverageRating = 4.25,
                    NationalityId = 182,
                    Gender = Gender.Male,
                    BornAt = new DateTime(1947, 09, 21),
                    CreatorId = "user1Id",
                    IsApproved = true
                },

                new()
                {
                    Id = 2,
                    Name = "Joanne Rowling",
                    ImageUrl = "https://stories.jkrowling.com/wp-content/uploads/2021/09/Shot-B-105_V2_CROP-e1630873059779.jpg",
                    Biography = "Joanne Rowling known by her pen name J. K. Rowling, is a British author and philanthropist. " +
                    "She is the author of Harry Potter, a seven-volume fantasy novel series published from 1997 to 2007. " +
                    "The series has sold over 600 million copies, been translated into 84 languages, " +
                    "and spawned a global media franchise including films and video games. " +
                    "The Casual Vacancy (2012) was her first novel for adults. " +
                    "She writes Cormoran Strike, an ongoing crime fiction series, under the alias Robert Galbraith.",
                    PenName = "J. K. Rowling",
                    AverageRating = 4.75,
                    NationalityId = 181,
                    Gender = Gender.Female,
                    BornAt = new DateTime(1965, 07, 31),
                    CreatorId = "user2Id",
                    IsApproved = true
                },
                new()
                {
                    Id = 3,
                    Name = "John Ronald Reuel Tolkien ",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d4/J._R._R._Tolkien%2C_ca._1925.jpg/330px-J._R._R._Tolkien%2C_ca._1925.jpg",
                    Biography = "John Ronald Reuel Tolkien was an English writer and philologist. " +
                    "He was the author of the high fantasy works The Hobbit and The Lord of the Rings.",
                    PenName = "J.R.R Tolkien",
                    AverageRating = 4.67,
                    NationalityId = 181,
                    Gender = Gender.Male,
                    BornAt = new DateTime(1892, 01, 03),
                    DiedAt = new DateTime(1973, 09, 02),
                    CreatorId = "user3Id",
                    IsApproved = true
                }
            };
    }
}
