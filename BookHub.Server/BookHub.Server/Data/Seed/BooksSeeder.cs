namespace BookHub.Server.Data.Seed
{
    using Models;

    public static class BooksSeeder
    {
        public static Book[] Seed()
            => new Book[]
            {
                new()
                {
                    Id = 1,
                    Title = "Pet Sematary",
                    ShortDescription = "Sometimes dead is better.",
                    LongDescription = "Pet Sematary is a 1983 horror novel by American writer Stephen King. " +
                    "The novel was nominated for a World Fantasy Award for Best Novel in 1984," +
                    "and adapted into two films: one in 1989 and another in 2019." +
                    " In November 2013, PS Publishing released Pet Sematary in a limited 30th-anniversary edition.",
                    AverageRating = 4.25,
                    RatingsCount = 4,
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/24/Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg/330px-Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg",
                    AuthorId = 1,
                    CreatorId = "user1Id",
                    PublishedDate = new DateTime(1983, 11, 4),
                    IsApproved = true
                },
                new()
                {
                    Id = 2,
                    Title = "Harry Potter and the Deathly Hallows",
                    ShortDescription = "The last book from the Harry Potter series.",
                    LongDescription = "Harry Potter and the Deathly Hallows is a fantasy novel written by the British author J. K. Rowling. " +
                    "It is the seventh and final novel in the Harry Potter series. " +
                    "It was released on 21 July 2007 in the United Kingdom by Bloomsbury Publishing, " +
                    "in the United States by Scholastic, and in Canada by Raincoast Books. " +
                    "The novel chronicles the events directly following Harry Potter and the Half-Blood Prince (2005) " +
                    "and the final confrontation between the wizards Harry Potter and Lord Voldemort.",
                    AverageRating = 4.75,
                    RatingsCount = 4,
                    ImageUrl = "https://librerialaberintopr.com/cdn/shop/products/hallows_459x.jpg?v=1616596206",
                    AuthorId = 2,
                    CreatorId = "user2Id",
                    PublishedDate = new DateTime(2007, 07, 21),
                    IsApproved = true
                },
                new()
                {
                    Id = 3,
                    Title = "Lord of the Rings",
                    ShortDescription = "The Lord of the Rings is an epic fantasy novel by the English author J. R. R. Tolkien.",
                    LongDescription = "Set in Middle-earth, the story began as a sequel to Tolkien's 1937 children's book The Hobbit, " +
                    "but eventually developed into a much larger work. Written in stages between 1937 and 1949, " +
                    "The Lord of the Rings is one of the best-selling books ever written, with over 150 million copies sold.",
                    AverageRating = 4.67,
                    RatingsCount = 6,
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/en/e/e9/First_Single_Volume_Edition_of_The_Lord_of_the_Rings.gif",
                    AuthorId = 3,
                    CreatorId = "user3Id",
                    PublishedDate = new DateTime(1954, 07, 29),
                    IsApproved = true
                }
            };
    }
}
