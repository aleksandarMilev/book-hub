namespace BookHub.Server.Data.Seed
{
    using Models;

    public static class ArticleSeeder
    {
        public static Article[] Seed()
            => new Article[]
            {
                new()
                {
                    Id = 1,
                    Title = "Exploring the Haunting Depths of *Pet Sematary*",
                    Introduction = "A tale of love, loss, and the chilling cost of defying death.",
                    Content = 
                        "Stephen King's *Pet Sematary* offers a chilling narrative about the emotional weight of grief and the lengths " +
                        "people go to in an effort to reverse the irreversible. When Louis Creed and his family move to rural Maine, " +
                        "they discover a mysterious burial ground behind their home that can bring the dead back to life. " +
                        "What begins as an innocent exploration of local lore quickly turns into a nightmare when Louis faces the ultimate temptation: " +
                        "using the cemetery’s power to alter fate itself. The novel dives deep into human frailty, moral dilemmas," +
                        "and the unintended consequences of playing God. King's skillful writing leaves readers pondering the fine line between love " +
                        "and selfishness, as well as the destructive force of denial.",
                    ImageUrl = "https://thedailytexan.com/wp-content/uploads/2023/09/pet_sematary_bloodlines_4press-1200x800.jpg",
                    Views = 0,
                    CreatedOn = DateTime.Now
                },
                new()
                {
                    Id = 2,
                    Title = "The Epic Conclusion: *Harry Potter and the Deathly Hallows*",
                    Introduction = "The battle against Voldemort reaches its thrilling and emotional finale.",
                    Content = 
                        "In *Harry Potter and the Deathly Hallows*, J.K. Rowling masterfully concludes the saga of the Boy Who Lived. Harry, Ron, " +
                        "and Hermione take center stage as they leave Hogwarts behind and embark on a perilous journey to locate and destroy Voldemort’s" +
                        "Horcruxes. Along the way, they uncover hidden truths about Dumbledore’s past, face betrayal, and reaffirm their unshakable " +
                        "friendship. The novel culminates in the iconic Battle of Hogwarts, where courage and sacrifice define the fates of beloved " +
                        "characters. Packed with heart-pounding action and poignant moments, Rowling’s finale is not just a fight between good and evil" +
                        "but a meditation on love, legacy, and the choices that shape our lives.",
                    ImageUrl = "https://choicefineart.com/cdn/shop/products/book-7-harry-potter-and-the-deathly-hallows-311816.jpg?v=1688079541",
                    Views = 0,
                    CreatedOn = DateTime.Now
                },
                new()
                {
                    Id = 3,
                    Title = "The Timeless Epic: *The Lord of the Rings*",
                    Introduction = "A journey of courage, friendship, and the fight against overwhelming darkness.",
                    Content = 
                        "J.R.R. Tolkien’s *The Lord of the Rings* stands as one of the greatest works of fantasy ever written. " +
                        "Spanning three volumes, the story follows Frodo Baggins and the Fellowship as they embark on a dangerous mission to destroy " +
                        "the One Ring, a powerful artifact that threatens to engulf Middle-earth in darkness. Tolkien’s world-building is unparalleled, " +
                        "bringing to life the rich landscapes of Middle-earth, from the idyllic Shire to the fiery Mount Doom. Along the way, characters " +
                        "like Aragorn, Legolas, and Samwise Gamgee demonstrate the values of loyalty, perseverance, and hope. " +
                        "The narrative explores themes of power, corruption, and the resilience of the human spirit, making it a tale that resonates " +
                        "across generations. Whether it’s Gandalf’s wisdom, Frodo’s burden, or the triumph of unity, Tolkien’s masterpiece continues " +
                        "to inspire and enchant readers worldwide.",
                    ImageUrl = "https://blogger.googleusercontent.com/img/b/R29vZ2xl/AVvXsEibI7_-Az0QVZhhwZO_PcgrNRK7RYnS7JPiddt_LvTC8NTgTzzYcaagGBLR6KtgY1J_VyZzS6HhL7MW9x1h-rioISPanc-daPbdgnZCQQb48PNELDt9gbQlohCJuXGHgritNS_3Ff08oUhs/w1200-h630-p-k-no-nu/acetolkien.jpg",
                    Views = 0,
                    CreatedOn = DateTime.Now
                }
            };
    }
}
