#nullable disable
namespace BookHub.Server.Data.Migrations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.EntityFrameworkCore.Migrations;

    [DbContext(typeof(BookHubDbContext))]
    [Migration("20241130175501_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BookHub.Server.Data.Models.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(5000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("Introduction")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Views")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Articles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Content = "Stephen King's *Pet Sematary* offers a chilling narrative about the emotional weight of grief and the lengths people go to in an effort to reverse the irreversible. When Louis Creed and his family move to rural Maine, they discover a mysterious burial ground behind their home that can bring the dead back to life. What begins as an innocent exploration of local lore quickly turns into a nightmare when Louis faces the ultimate temptation: using the cemetery’s power to alter fate itself. The novel dives deep into human frailty, moral dilemmas,and the unintended consequences of playing God. King's skillful writing leaves readers pondering the fine line between love and selfishness, as well as the destructive force of denial.",
                            CreatedOn = new DateTime(2024, 11, 30, 19, 54, 59, 977, DateTimeKind.Local).AddTicks(8503),
                            ImageUrl = "https://thedailytexan.com/wp-content/uploads/2023/09/pet_sematary_bloodlines_4press-1200x800.jpg",
                            Introduction = "A tale of love, loss, and the chilling cost of defying death.",
                            IsDeleted = false,
                            Title = "Exploring the Haunting Depths of *Pet Sematary*",
                            Views = 0
                        },
                        new
                        {
                            Id = 2,
                            Content = "In *Harry Potter and the Deathly Hallows*, J.K. Rowling masterfully concludes the saga of the Boy Who Lived. Harry, Ron, and Hermione take center stage as they leave Hogwarts behind and embark on a perilous journey to locate and destroy Voldemort’sHorcruxes. Along the way, they uncover hidden truths about Dumbledore’s past, face betrayal, and reaffirm their unshakable friendship. The novel culminates in the iconic Battle of Hogwarts, where courage and sacrifice define the fates of beloved characters. Packed with heart-pounding action and poignant moments, Rowling’s finale is not just a fight between good and evilbut a meditation on love, legacy, and the choices that shape our lives.",
                            CreatedOn = new DateTime(2024, 11, 30, 19, 54, 59, 977, DateTimeKind.Local).AddTicks(8632),
                            ImageUrl = "https://choicefineart.com/cdn/shop/products/book-7-harry-potter-and-the-deathly-hallows-311816.jpg?v=1688079541",
                            Introduction = "The battle against Voldemort reaches its thrilling and emotional finale.",
                            IsDeleted = false,
                            Title = "The Epic Conclusion: *Harry Potter and the Deathly Hallows*",
                            Views = 0
                        },
                        new
                        {
                            Id = 3,
                            Content = "J.R.R. Tolkien’s *The Lord of the Rings* stands as one of the greatest works of fantasy ever written. Spanning three volumes, the story follows Frodo Baggins and the Fellowship as they embark on a dangerous mission to destroy the One Ring, a powerful artifact that threatens to engulf Middle-earth in darkness. Tolkien’s world-building is unparalleled, bringing to life the rich landscapes of Middle-earth, from the idyllic Shire to the fiery Mount Doom. Along the way, characters like Aragorn, Legolas, and Samwise Gamgee demonstrate the values of loyalty, perseverance, and hope. The narrative explores themes of power, corruption, and the resilience of the human spirit, making it a tale that resonates across generations. Whether it’s Gandalf’s wisdom, Frodo’s burden, or the triumph of unity, Tolkien’s masterpiece continues to inspire and enchant readers worldwide.",
                            CreatedOn = new DateTime(2024, 11, 30, 19, 54, 59, 977, DateTimeKind.Local).AddTicks(8643),
                            ImageUrl = "https://blogger.googleusercontent.com/img/b/R29vZ2xl/AVvXsEibI7_-Az0QVZhhwZO_PcgrNRK7RYnS7JPiddt_LvTC8NTgTzzYcaagGBLR6KtgY1J_VyZzS6HhL7MW9x1h-rioISPanc-daPbdgnZCQQb48PNELDt9gbQlohCJuXGHgritNS_3Ff08oUhs/w1200-h630-p-k-no-nu/acetolkien.jpg",
                            Introduction = "A journey of courage, friendship, and the fight against overwhelming darkness.",
                            IsDeleted = false,
                            Title = "The Timeless Epic: *The Lord of the Rings*",
                            Views = 0
                        });
                });

            modelBuilder.Entity("BookHub.Server.Data.Models.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("AverageRating")
                        .HasColumnType("float");

                    b.Property<string>("Biography")
                        .IsRequired()
                        .HasMaxLength(10000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("BornAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DiedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("NationalityId")
                        .HasColumnType("int");

                    b.Property<string>("PenName")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("RatingsCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("NationalityId");

                    b.ToTable("Authors");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AverageRating = 4.25,
                            Biography = "Stephen Edwin King (born September 21, 1947) is an American author. Widely known for his horror novels, he has been crowned the \"King of Horror\". He has also explored other genres, among them suspense, crime, science-fiction, fantasy and mystery. Though known primarily for his novels, he has written approximately 200 short stories, most of which have been published in collections.",
                            BornAt = new DateTime(1947, 9, 21, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user1Id",
                            Gender = 0,
                            ImageUrl = "https://media.npr.org/assets/img/2020/04/07/stephen-king.by-shane-leonard_wide-c132de683d677c7a6a1e692f86a7e6670baf3338.jpg?s=1100&c=85&f=jpeg",
                            IsApproved = true,
                            IsDeleted = false,
                            Name = "Stephen King",
                            NationalityId = 182,
                            PenName = "Richard Bachman",
                            RatingsCount = 0
                        },
                        new
                        {
                            Id = 2,
                            AverageRating = 4.75,
                            Biography = "Joanne Rowling known by her pen name J. K. Rowling, is a British author and philanthropist. She is the author of Harry Potter, a seven-volume fantasy novel series published from 1997 to 2007. The series has sold over 600 million copies, been translated into 84 languages, and spawned a global media franchise including films and video games. The Casual Vacancy (2012) was her first novel for adults. She writes Cormoran Strike, an ongoing crime fiction series, under the alias Robert Galbraith.",
                            BornAt = new DateTime(1965, 7, 31, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user2Id",
                            Gender = 1,
                            ImageUrl = "https://stories.jkrowling.com/wp-content/uploads/2021/09/Shot-B-105_V2_CROP-e1630873059779.jpg",
                            IsApproved = true,
                            IsDeleted = false,
                            Name = "Joanne Rowling",
                            NationalityId = 181,
                            PenName = "J. K. Rowling",
                            RatingsCount = 0
                        },
                        new
                        {
                            Id = 3,
                            AverageRating = 4.6699999999999999,
                            Biography = "John Ronald Reuel Tolkien was an English writer and philologist. He was the author of the high fantasy works The Hobbit and The Lord of the Rings.",
                            BornAt = new DateTime(1892, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user3Id",
                            DiedAt = new DateTime(1973, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Gender = 0,
                            ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d4/J._R._R._Tolkien%2C_ca._1925.jpg/330px-J._R._R._Tolkien%2C_ca._1925.jpg",
                            IsApproved = true,
                            IsDeleted = false,
                            Name = "John Ronald Reuel Tolkien ",
                            NationalityId = 181,
                            PenName = "J.R.R Tolkien",
                            RatingsCount = 0
                        });
                });

            modelBuilder.Entity("BookHub.Server.Data.Models.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AuthorId")
                        .HasColumnType("int");

                    b.Property<double>("AverageRating")
                        .HasColumnType("float");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LongDescription")
                        .IsRequired()
                        .HasMaxLength(5000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("PublishedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("RatingsCount")
                        .HasColumnType("int");

                    b.Property<string>("ShortDescription")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("CreatorId");

                    b.ToTable("Books");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AuthorId = 1,
                            AverageRating = 4.25,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user1Id",
                            ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/24/Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg/330px-Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg",
                            IsApproved = true,
                            IsDeleted = false,
                            LongDescription = "Pet Sematary is a 1983 horror novel by American writer Stephen King. The novel was nominated for a World Fantasy Award for Best Novel in 1984,and adapted into two films: one in 1989 and another in 2019. In November 2013, PS Publishing released Pet Sematary in a limited 30th-anniversary edition.",
                            PublishedDate = new DateTime(1983, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            RatingsCount = 4,
                            ShortDescription = "Sometimes dead is better.",
                            Title = "Pet Sematary"
                        },
                        new
                        {
                            Id = 2,
                            AuthorId = 2,
                            AverageRating = 4.75,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user2Id",
                            ImageUrl = "https://librerialaberintopr.com/cdn/shop/products/hallows_459x.jpg?v=1616596206",
                            IsApproved = true,
                            IsDeleted = false,
                            LongDescription = "Harry Potter and the Deathly Hallows is a fantasy novel written by the British author J. K. Rowling. It is the seventh and final novel in the Harry Potter series. It was released on 21 July 2007 in the United Kingdom by Bloomsbury Publishing, in the United States by Scholastic, and in Canada by Raincoast Books. The novel chronicles the events directly following Harry Potter and the Half-Blood Prince (2005) and the final confrontation between the wizards Harry Potter and Lord Voldemort.",
                            PublishedDate = new DateTime(2007, 7, 21, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            RatingsCount = 4,
                            ShortDescription = "The last book from the Harry Potter series.",
                            Title = "Harry Potter and the Deathly Hallows"
                        },
                        new
                        {
                            Id = 3,
                            AuthorId = 3,
                            AverageRating = 4.6699999999999999,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user3Id",
                            ImageUrl = "https://upload.wikimedia.org/wikipedia/en/e/e9/First_Single_Volume_Edition_of_The_Lord_of_the_Rings.gif",
                            IsApproved = true,
                            IsDeleted = false,
                            LongDescription = "Set in Middle-earth, the story began as a sequel to Tolkien's 1937 children's book The Hobbit, but eventually developed into a much larger work. Written in stages between 1937 and 1949, The Lord of the Rings is one of the best-selling books ever written, with over 150 million copies sold.",
                            PublishedDate = new DateTime(1954, 7, 29, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            RatingsCount = 6,
                            ShortDescription = "The Lord of the Rings is an epic fantasy novel by the English author J. R. R. Tolkien.",
                            Title = "Lord of the Rings"
                        });
                });

            modelBuilder.Entity("BookHub.Server.Data.Models.BookGenre", b =>
                {
                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<int>("GenreId")
                        .HasColumnType("int");

                    b.HasKey("BookId", "GenreId");

                    b.HasIndex("GenreId");

                    b.ToTable("BooksGenres");

                    b.HasData(
                        new
                        {
                            BookId = 1,
                            GenreId = 1
                        },
                        new
                        {
                            BookId = 1,
                            GenreId = 6
                        },
                        new
                        {
                            BookId = 2,
                            GenreId = 3
                        },
                        new
                        {
                            BookId = 2,
                            GenreId = 14
                        },
                        new
                        {
                            BookId = 2,
                            GenreId = 15
                        },
                        new
                        {
                            BookId = 3,
                            GenreId = 3
                        });
                });

            modelBuilder.Entity("BookHub.Server.Data.Models.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(3000)
                        .HasColumnType("nvarchar(3000)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Genres");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Horror fiction is designed to scare, unsettle, or horrify readers. It explores themes of fear and the unknown, often incorporating supernatural elements like ghosts, monsters, or curses. The genre can also delve into the darker aspects of human psychology, portraying paranoia, obsession, and moral corruption. Subgenres include Gothic horror, psychological horror, and splatterpunk, each offering unique ways to evoke dread. Settings often amplify the tension, ranging from haunted houses to desolate landscapes, while the stories frequently address societal fears and existential questions.",
                            ImageUrl = "https://org-dcmp-staticassets.s3.us-east-1.amazonaws.com/posterimages/13453_1.jpg",
                            IsDeleted = false,
                            Name = "Horror"
                        },
                        new
                        {
                            Id = 2,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Science fiction explores futuristic, scientific, and technological themes, challenging readers to consider the possibilities and consequences of innovation. These stories often involve space exploration, artificial intelligence, time travel, or parallel universes. Beyond the speculative elements, science fiction frequently tackles ethical dilemmas, societal transformations, and the human condition. Subgenres include cyberpunk, space opera, and hard science fiction, each offering distinct visions of the future. The genre invites readers to imagine the impact of progress and to ponder humanity’s place in the cosmos.",
                            ImageUrl = "https://www.editoreric.com/greatlit/litgraphics/book-spiral-galaxy.jpg",
                            IsDeleted = false,
                            Name = "Science Fiction"
                        },
                        new
                        {
                            Id = 3,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Fantasy stories transport readers to magical realms filled with mythical creatures, enchanted objects, and epic quests. These tales often feature battles between good and evil, drawing upon folklore, mythology, and the human imagination. Characters may wield powerful magic or undertake journeys of self-discovery in richly crafted worlds. Subgenres like high fantasy, urban fantasy, and dark fantasy provide diverse settings and tones, appealing to a wide range of readers. Themes of heroism, destiny, and transformation are central to the genre, offering both escape and inspiration.",
                            ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT5EcrB6fhai5L3-7Ted6fZgxUjCti0W4avrA&s",
                            IsDeleted = false,
                            Name = "Fantasy"
                        },
                        new
                        {
                            Id = 4,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Mystery fiction is a puzzle-driven genre that engages readers with suspense and intrigue. The narrative typically revolves around solving a crime, uncovering hidden truths, or exposing a web of deceit. Protagonists range from amateur sleuths to seasoned detectives, each navigating clues, red herrings, and unexpected twists. Subgenres such as noir, cozy mysteries, and legal thrillers cater to varied tastes. Mystery stories often delve into human motives and societal dynamics, providing a satisfying journey toward uncovering the truth.",
                            ImageUrl = "https://celadonbooks.com/wp-content/uploads/2020/03/what-is-a-mystery.jpg",
                            IsDeleted = false,
                            Name = "Mystery"
                        },
                        new
                        {
                            Id = 5,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Romance novels celebrate the complexities of love and relationships, weaving stories of passion, connection, and emotional growth. They can be set in diverse contexts, from historical periods to fantastical worlds, and often feature characters overcoming personal or external obstacles to find happiness. Subgenres like contemporary romance, historical romance, and paranormal romance offer unique flavors and settings. The genre emphasizes emotional resonance, with narratives that inspire hope and affirm the power of love.",
                            ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/3/36/Hammond-SS10.jpg",
                            IsDeleted = false,
                            Name = "Romance"
                        },
                        new
                        {
                            Id = 6,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Thrillers are characterized by their fast-paced, high-stakes plots designed to keep readers on edge. They often involve life-and-death scenarios, sinister conspiracies, or relentless antagonists. The genre thrives on tension and unexpected twists, with protagonists racing against time to prevent disaster. Subgenres like psychological thrillers, spy thrillers, and action thrillers cater to diverse interests. The stories explore themes of survival, justice, and moral ambiguity, delivering an adrenaline-fueled reading experience.",
                            ImageUrl = "https://celadonbooks.com/wp-content/uploads/2019/10/what-is-a-thriller-1024x768.jpg",
                            IsDeleted = false,
                            Name = "Thriller"
                        },
                        new
                        {
                            Id = 7,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Adventure stories are dynamic tales of action, exploration, and survival. Protagonists often face daunting challenges, traversing uncharted territories or overcoming perilous odds. The genre celebrates courage, resilience, and the human spirit, taking readers on exhilarating journeys. From treasure hunts to epic battles, adventure fiction encompasses diverse settings and narratives. It appeals to those who crave excitement and the thrill of discovery.",
                            ImageUrl = "https://thumbs.dreamstime.com/b/open-book-ship-sailing-waves-concept-reading-adventure-literature-generative-ai-270347849.jpg",
                            IsDeleted = false,
                            Name = "Adventure"
                        },
                        new
                        {
                            Id = 8,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Historical fiction immerses readers in the past, blending factual events with fictional narratives to create vivid portrayals of bygone eras. These stories illuminate the lives, struggles, and triumphs of people from different times, providing insight into cultural, social, and political contexts. Subgenres include historical romance, historical mysteries, and alternate histories, each offering unique perspectives. The genre enriches our understanding of history while engaging us with compelling characters and plots.",
                            ImageUrl = "https://celadonbooks.com/wp-content/uploads/2020/03/Historical-Fiction-scaled.jpg",
                            IsDeleted = false,
                            Name = "Historical"
                        },
                        new
                        {
                            Id = 9,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Biographies chronicle the lives of real individuals, offering intimate portraits of their experiences, achievements, and legacies. These works range from comprehensive life stories to focused accounts of specific events or periods. Biographies can inspire, inform, and provide deep insight into historical or contemporary figures. Autobiographies and memoirs, subgenres of biography, allow subjects to share their own narratives, adding personal depth to the genre.",
                            ImageUrl = "https://i0.wp.com/uspeakgreek.com/wp-content/uploads/2024/01/biography.webp?fit=780%2C780&ssl=1",
                            IsDeleted = false,
                            Name = "Biography"
                        },
                        new
                        {
                            Id = 10,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Self-help books are guides to personal growth, offering practical advice for improving one’s life. Topics range from mental health and relationships to productivity and spiritual fulfillment. The genre emphasizes empowerment, providing readers with strategies and tools for achieving goals and overcoming challenges. Subgenres include motivational literature, mindfulness guides, and career development books, catering to diverse needs and aspirations.",
                            ImageUrl = "https://www.wellnessroadpsychology.com/wp-content/uploads/2024/05/Self-Help.jpg",
                            IsDeleted = false,
                            Name = "Self-help"
                        },
                        new
                        {
                            Id = 11,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Non-fiction encompasses works rooted in factual information, offering insights into real-world topics. It spans memoirs, investigative journalism, essays, and academic studies, covering subjects like history, science, culture, and politics. The genre educates and engages readers, often challenging perceptions and broadening understanding. Non-fiction can be narrative-driven or expository, appealing to those seeking knowledge or a deeper connection to reality.",
                            ImageUrl = "https://pickbestbook.com/wp-content/uploads/2023/06/Nonfiction-Literature-1.png",
                            IsDeleted = false,
                            Name = "Non-fiction"
                        },
                        new
                        {
                            Id = 12,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Poetry is a literary form that condenses emotions, thoughts, and imagery into carefully chosen words, often structured with rhythm and meter. It explores universal themes such as love, nature, grief, and introspection, offering readers profound and evocative experiences. From traditional sonnets and haikus to free verse and spoken word, poetry captivates through its ability to articulate the inexpressible, creating deep emotional resonance and intellectual reflection.",
                            ImageUrl = "https://assets.ltkcontent.com/images/9037/examples-of-poetry-genres_7abbbb2796.jpg",
                            IsDeleted = false,
                            Name = "Poetry"
                        },
                        new
                        {
                            Id = 13,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Drama fiction delves into emotional and relational conflicts, portraying the complexities of human interactions and emotions. It emphasizes character development and nuanced storytelling, often exploring themes of love, betrayal, identity, and societal struggles. Drama offers readers a lens into the intricacies of the human experience, whether through tragic, romantic, or morally ambiguous narratives. Its focus on realism and emotional depth creates stories that resonate deeply with audiences.",
                            ImageUrl = "https://basudewacademichub.in/wp-content/uploads/2024/02/drama-literature-solution.png",
                            IsDeleted = false,
                            Name = "Drama"
                        },
                        new
                        {
                            Id = 14,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Children's literature is crafted to captivate and inspire young readers with imaginative worlds, moral lessons, and relatable characters. These stories often emphasize themes of curiosity, friendship, and bravery, delivering messages of kindness, resilience, and growth. From whimsical picture books to adventurous chapter books, children's fiction nurtures creativity and fosters a lifelong love of reading, helping young minds explore both real and fantastical realms.",
                            ImageUrl = "https://media.vanityfair.com/photos/598888671dc63c45b7b1db6e/master/w_2560%2Cc_limit/MAG-0817-Wild-Things-a.jpg",
                            IsDeleted = false,
                            Name = "Children's"
                        },
                        new
                        {
                            Id = 15,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Young Adult (YA) fiction speaks to the unique experiences and challenges of adolescence, addressing themes such as identity, first love, friendship, and coming of age. These stories often feature relatable protagonists navigating personal growth, societal expectations, and emotional upheaval. Subgenres such as fantasy, dystopian, and contemporary YA provide diverse backdrops for these journeys, resonating with readers through authentic and engaging storytelling that reflects their own struggles and triumphs.",
                            ImageUrl = "https://m.media-amazon.com/images/I/81xRLF1KCAL._AC_UF1000,1000_QL80_.jpg",
                            IsDeleted = false,
                            Name = "Young Adult"
                        },
                        new
                        {
                            Id = 16,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Comedy fiction aims to entertain and delight readers through humor, satire, and absurdity. It uses wit and clever storytelling to highlight human follies, societal quirks, or surreal situations. From lighthearted escapades to biting social commentary, comedy encompasses a range of tones and styles. The genre often brings laughter and joy, offering an escape from the mundane while sometimes delivering thought-provoking messages in the guise of humor.",
                            ImageUrl = "https://mandyevebarnett.com/wp-content/uploads/2017/12/humor.jpg?w=640",
                            IsDeleted = false,
                            Name = "Comedy"
                        },
                        new
                        {
                            Id = 17,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Graphic novels seamlessly blend visual art and narrative storytelling, using a combination of text and illustrations to convey complex plots and emotions. This versatile format spans a wide array of genres, including superhero tales, memoirs, historical epics, and science fiction. Graphic novels offer an immersive reading experience, appealing to diverse audiences through their ability to convey vivid imagery and intricate storylines that are as impactful as traditional prose.",
                            ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSb0THovTlPB_nRl3RY6TsbWD4R2qEC-TQSAg&s",
                            IsDeleted = false,
                            Name = "Graphic Novel"
                        },
                        new
                        {
                            Id = 18,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "The 'Other' genre serves as a home for unconventional, experimental, or cross-genre works that defy traditional categorization. This category embraces innovation and diversity, welcoming stories that push the boundaries of storytelling, structure, and style. From hybrid narratives to avant-garde experiments, 'Other' offers a platform for unique voices and creative expressions that don’t fit neatly into predefined genres.",
                            ImageUrl = "https://www.98thpercentile.com/hubfs/388x203%20(4).png",
                            IsDeleted = false,
                            Name = "Other"
                        },
                        new
                        {
                            Id = 19,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Dystopian fiction paints a grim portrait of societies marred by oppression, inequality, or disaster, often set in a future shaped by catastrophic events or authoritarian regimes. These cautionary tales explore themes like survival, rebellion, and the loss of humanity, serving as critiques of political, social, and environmental trends. Subgenres such as post-apocalyptic and cyber-dystopia examine the fragility of civilization and the consequences of unchecked power or technological overreach.",
                            ImageUrl = "https://www.ideology-theory-practice.org/uploads/1/3/5/5/135563566/050_orig.jpg",
                            IsDeleted = false,
                            Name = "Dystopian"
                        },
                        new
                        {
                            Id = 20,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Spirituality books delve into the deeper questions of existence, faith, and the human soul, offering insights and practices to nurture inner peace and personal growth. They often explore themes of mindfulness, self-awareness, and connection to a higher power or universal energy. From philosophical reflections to practical guides, these works resonate with readers seekinginspiration, understanding, and spiritual fulfillment across diverse traditions and belief systems.",
                            ImageUrl = "https://m.media-amazon.com/images/I/61jxcM3UskL._AC_UF1000,1000_QL80_.jpg",
                            IsDeleted = false,
                            Name = "Spirituality"
                        });
                });

            modelBuilder.Entity("BookHub.Server.Data.Models.Nationality", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Nationalities");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Afghanistan"
                        },
                        new
                        {
                            Id = 2,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Albania"
                        },
                        new
                        {
                            Id = 3,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Algeria"
                        },
                        new
                        {
                            Id = 4,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Andorra"
                        },
                        new
                        {
                            Id = 5,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Angola"
                        },
                        new
                        {
                            Id = 6,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Antigua and Barbuda"
                        },
                        new
                        {
                            Id = 7,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Argentina"
                        },
                        new
                        {
                            Id = 8,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Armenia"
                        },
                        new
                        {
                            Id = 9,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Australia"
                        },
                        new
                        {
                            Id = 10,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Austria"
                        },
                        new
                        {
                            Id = 11,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Azerbaijan"
                        },
                        new
                        {
                            Id = 12,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Bahamas"
                        },
                        new
                        {
                            Id = 13,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Bahrain"
                        },
                        new
                        {
                            Id = 14,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Bangladesh"
                        },
                        new
                        {
                            Id = 15,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Barbados"
                        },
                        new
                        {
                            Id = 16,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Belarus"
                        },
                        new
                        {
                            Id = 17,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Belgium"
                        },
                        new
                        {
                            Id = 18,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Belize"
                        },
                        new
                        {
                            Id = 19,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Benin"
                        },
                        new
                        {
                            Id = 20,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Bhutan"
                        },
                        new
                        {
                            Id = 21,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Bolivia"
                        },
                        new
                        {
                            Id = 22,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Bosnia and Herzegovina"
                        },
                        new
                        {
                            Id = 23,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Botswana"
                        },
                        new
                        {
                            Id = 24,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Brazil"
                        },
                        new
                        {
                            Id = 25,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Brunei"
                        },
                        new
                        {
                            Id = 26,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Bulgaria"
                        },
                        new
                        {
                            Id = 27,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Burkina Faso"
                        },
                        new
                        {
                            Id = 28,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Burundi"
                        },
                        new
                        {
                            Id = 29,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Cabo Verde"
                        },
                        new
                        {
                            Id = 30,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Cambodia"
                        },
                        new
                        {
                            Id = 31,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Cameroon"
                        },
                        new
                        {
                            Id = 32,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Canada"
                        },
                        new
                        {
                            Id = 33,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Central African Republic"
                        },
                        new
                        {
                            Id = 34,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Chad"
                        },
                        new
                        {
                            Id = 35,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Chile"
                        },
                        new
                        {
                            Id = 36,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "China"
                        },
                        new
                        {
                            Id = 37,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Colombia"
                        },
                        new
                        {
                            Id = 38,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Comoros"
                        },
                        new
                        {
                            Id = 39,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Costa Rica"
                        },
                        new
                        {
                            Id = 40,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Croatia"
                        },
                        new
                        {
                            Id = 41,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Cuba"
                        },
                        new
                        {
                            Id = 42,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Cyprus"
                        },
                        new
                        {
                            Id = 43,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Czech Republic"
                        },
                        new
                        {
                            Id = 44,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Denmark"
                        },
                        new
                        {
                            Id = 45,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Djibouti"
                        },
                        new
                        {
                            Id = 46,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Dominica"
                        },
                        new
                        {
                            Id = 47,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Dominican Republic"
                        },
                        new
                        {
                            Id = 48,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Ecuador"
                        },
                        new
                        {
                            Id = 49,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Egypt"
                        },
                        new
                        {
                            Id = 50,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "El Salvador"
                        },
                        new
                        {
                            Id = 51,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Equatorial Guinea"
                        },
                        new
                        {
                            Id = 52,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Eritrea"
                        },
                        new
                        {
                            Id = 53,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Estonia"
                        },
                        new
                        {
                            Id = 54,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Eswatini"
                        },
                        new
                        {
                            Id = 55,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Ethiopia"
                        },
                        new
                        {
                            Id = 56,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Fiji"
                        },
                        new
                        {
                            Id = 57,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Finland"
                        },
                        new
                        {
                            Id = 58,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "France"
                        },
                        new
                        {
                            Id = 59,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Gabon"
                        },
                        new
                        {
                            Id = 60,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Gambia"
                        },
                        new
                        {
                            Id = 61,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Georgia"
                        },
                        new
                        {
                            Id = 62,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Germany"
                        },
                        new
                        {
                            Id = 63,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Ghana"
                        },
                        new
                        {
                            Id = 64,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Greece"
                        },
                        new
                        {
                            Id = 65,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Grenada"
                        },
                        new
                        {
                            Id = 66,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Guatemala"
                        },
                        new
                        {
                            Id = 67,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Guinea"
                        },
                        new
                        {
                            Id = 68,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Guinea-Bissau"
                        },
                        new
                        {
                            Id = 69,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Guyana"
                        },
                        new
                        {
                            Id = 70,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Haiti"
                        },
                        new
                        {
                            Id = 71,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Honduras"
                        },
                        new
                        {
                            Id = 72,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Hungary"
                        },
                        new
                        {
                            Id = 73,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Iceland"
                        },
                        new
                        {
                            Id = 74,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "India"
                        },
                        new
                        {
                            Id = 75,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Indonesia"
                        },
                        new
                        {
                            Id = 76,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Iran"
                        },
                        new
                        {
                            Id = 77,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Iraq"
                        },
                        new
                        {
                            Id = 78,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Ireland"
                        },
                        new
                        {
                            Id = 79,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Israel"
                        },
                        new
                        {
                            Id = 80,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Italy"
                        },
                        new
                        {
                            Id = 81,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Jamaica"
                        },
                        new
                        {
                            Id = 82,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Japan"
                        },
                        new
                        {
                            Id = 83,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Jordan"
                        },
                        new
                        {
                            Id = 84,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Kazakhstan"
                        },
                        new
                        {
                            Id = 85,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Kenya"
                        },
                        new
                        {
                            Id = 86,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Kiribati"
                        },
                        new
                        {
                            Id = 87,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "North Korea"
                        },
                        new
                        {
                            Id = 88,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "South Korea"
                        },
                        new
                        {
                            Id = 89,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Kuwait"
                        },
                        new
                        {
                            Id = 90,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Kyrgyzstan"
                        },
                        new
                        {
                            Id = 91,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Laos"
                        },
                        new
                        {
                            Id = 92,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Latvia"
                        },
                        new
                        {
                            Id = 93,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Lebanon"
                        },
                        new
                        {
                            Id = 94,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Lesotho"
                        },
                        new
                        {
                            Id = 95,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Liberia"
                        },
                        new
                        {
                            Id = 96,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Libya"
                        },
                        new
                        {
                            Id = 97,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Liechtenstein"
                        },
                        new
                        {
                            Id = 98,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Lithuania"
                        },
                        new
                        {
                            Id = 99,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Luxembourg"
                        },
                        new
                        {
                            Id = 100,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Madagascar"
                        },
                        new
                        {
                            Id = 101,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Malawi"
                        },
                        new
                        {
                            Id = 102,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Malaysia"
                        },
                        new
                        {
                            Id = 103,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Maldives"
                        },
                        new
                        {
                            Id = 104,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Mali"
                        },
                        new
                        {
                            Id = 105,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Malta"
                        },
                        new
                        {
                            Id = 106,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Marshall Islands"
                        },
                        new
                        {
                            Id = 107,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Mauritania"
                        },
                        new
                        {
                            Id = 108,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Mauritius"
                        },
                        new
                        {
                            Id = 109,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Mexico"
                        },
                        new
                        {
                            Id = 110,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Micronesia"
                        },
                        new
                        {
                            Id = 111,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Moldova"
                        },
                        new
                        {
                            Id = 112,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Monaco"
                        },
                        new
                        {
                            Id = 113,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Mongolia"
                        },
                        new
                        {
                            Id = 114,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Montenegro"
                        },
                        new
                        {
                            Id = 115,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Morocco"
                        },
                        new
                        {
                            Id = 116,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Mozambique"
                        },
                        new
                        {
                            Id = 117,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Myanmar"
                        },
                        new
                        {
                            Id = 118,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Namibia"
                        },
                        new
                        {
                            Id = 119,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Nauru"
                        },
                        new
                        {
                            Id = 120,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Nepal"
                        },
                        new
                        {
                            Id = 121,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Netherlands"
                        },
                        new
                        {
                            Id = 122,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "New Zealand"
                        },
                        new
                        {
                            Id = 123,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Nicaragua"
                        },
                        new
                        {
                            Id = 124,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Niger"
                        },
                        new
                        {
                            Id = 125,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Nigeria"
                        },
                        new
                        {
                            Id = 126,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "North Macedonia"
                        },
                        new
                        {
                            Id = 127,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Norway"
                        },
                        new
                        {
                            Id = 128,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Oman"
                        },
                        new
                        {
                            Id = 129,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Pakistan"
                        },
                        new
                        {
                            Id = 130,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Palau"
                        },
                        new
                        {
                            Id = 131,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Panama"
                        },
                        new
                        {
                            Id = 132,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Papua New Guinea"
                        },
                        new
                        {
                            Id = 133,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Paraguay"
                        },
                        new
                        {
                            Id = 134,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Peru"
                        },
                        new
                        {
                            Id = 135,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Philippines"
                        },
                        new
                        {
                            Id = 136,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Poland"
                        },
                        new
                        {
                            Id = 137,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Portugal"
                        },
                        new
                        {
                            Id = 138,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Qatar"
                        },
                        new
                        {
                            Id = 139,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Romania"
                        },
                        new
                        {
                            Id = 140,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Russia"
                        },
                        new
                        {
                            Id = 141,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Rwanda"
                        },
                        new
                        {
                            Id = 142,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Saint Kitts and Nevis"
                        },
                        new
                        {
                            Id = 143,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Saint Lucia"
                        },
                        new
                        {
                            Id = 144,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Saint Vincent and the Grenadines"
                        },
                        new
                        {
                            Id = 145,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Samoa"
                        },
                        new
                        {
                            Id = 146,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "San Marino"
                        },
                        new
                        {
                            Id = 147,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "São Tomé and Príncipe"
                        },
                        new
                        {
                            Id = 148,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Saudi Arabia"
                        },
                        new
                        {
                            Id = 149,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Senegal"
                        },
                        new
                        {
                            Id = 150,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Serbia"
                        },
                        new
                        {
                            Id = 151,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Seychelles"
                        },
                        new
                        {
                            Id = 152,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Sierra Leone"
                        },
                        new
                        {
                            Id = 153,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Singapore"
                        },
                        new
                        {
                            Id = 154,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Slovakia"
                        },
                        new
                        {
                            Id = 155,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Slovenia"
                        },
                        new
                        {
                            Id = 156,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Solomon Islands"
                        },
                        new
                        {
                            Id = 157,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Somalia"
                        },
                        new
                        {
                            Id = 158,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "South Africa"
                        },
                        new
                        {
                            Id = 159,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "South Sudan"
                        },
                        new
                        {
                            Id = 160,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Spain"
                        },
                        new
                        {
                            Id = 161,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Sri Lanka"
                        },
                        new
                        {
                            Id = 162,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Sudan"
                        },
                        new
                        {
                            Id = 163,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Suriname"
                        },
                        new
                        {
                            Id = 164,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Sweden"
                        },
                        new
                        {
                            Id = 165,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Switzerland"
                        },
                        new
                        {
                            Id = 166,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Syria"
                        },
                        new
                        {
                            Id = 167,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Taiwan"
                        },
                        new
                        {
                            Id = 168,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Tajikistan"
                        },
                        new
                        {
                            Id = 169,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Tanzania"
                        },
                        new
                        {
                            Id = 170,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Thailand"
                        },
                        new
                        {
                            Id = 171,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Togo"
                        },
                        new
                        {
                            Id = 172,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Tonga"
                        },
                        new
                        {
                            Id = 173,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Trinidad and Tobago"
                        },
                        new
                        {
                            Id = 174,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Tunisia"
                        },
                        new
                        {
                            Id = 175,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Turkey"
                        },
                        new
                        {
                            Id = 176,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Turkmenistan"
                        },
                        new
                        {
                            Id = 177,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Tuvalu"
                        },
                        new
                        {
                            Id = 178,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Uganda"
                        },
                        new
                        {
                            Id = 179,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Ukraine"
                        },
                        new
                        {
                            Id = 180,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "United Arab Emirates"
                        },
                        new
                        {
                            Id = 181,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "United Kingdom"
                        },
                        new
                        {
                            Id = 182,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "United States"
                        },
                        new
                        {
                            Id = 183,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Unknown"
                        },
                        new
                        {
                            Id = 184,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Uruguay"
                        },
                        new
                        {
                            Id = 185,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Uzbekistan"
                        },
                        new
                        {
                            Id = 186,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Vanuatu"
                        },
                        new
                        {
                            Id = 187,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Vatican City"
                        },
                        new
                        {
                            Id = 188,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Venezuela"
                        },
                        new
                        {
                            Id = 189,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Vietnam"
                        },
                        new
                        {
                            Id = 190,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Yemen"
                        },
                        new
                        {
                            Id = 191,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Zambia"
                        },
                        new
                        {
                            Id = 192,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Zimbabwe"
                        });
                });

            modelBuilder.Entity("BookHub.Server.Data.Models.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("ReceiverId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ResourceId")
                        .HasColumnType("int");

                    b.Property<string>("ResourceType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("BookHub.Server.Data.Models.Reply", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Context")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("Dislikes")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("Likes")
                        .HasColumnType("int");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("ReviewId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("ReviewId");

                    b.ToTable("Replies");
                });

            modelBuilder.Entity("BookHub.Server.Data.Models.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.HasIndex("CreatorId");

                    b.ToTable("Reviews");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BookId = 1,
                            Content = "A truly chilling tale. King masterfully explores the dark side of human grief and love.",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user1Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 2,
                            BookId = 1,
                            Content = "The book was gripping but felt a bit too disturbing at times for my taste.",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user2Id",
                            IsDeleted = false,
                            Rating = 3
                        },
                        new
                        {
                            Id = 3,
                            BookId = 1,
                            Content = "An unforgettable story that haunts you long after you've finished it. Highly recommended!",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user3Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 4,
                            BookId = 1,
                            Content = "The characters were well-developed, but the plot felt predictable toward the end.",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user4Id",
                            IsDeleted = false,
                            Rating = 4
                        },
                        new
                        {
                            Id = 5,
                            BookId = 2,
                            Content = "An incredible conclusion to the series. Every twist and turn kept me on edge.",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user1Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 6,
                            BookId = 2,
                            Content = "The Battle of Hogwarts was epic! A bittersweet yet satisfying ending.",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user2Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 7,
                            BookId = 2,
                            Content = "I expected more from some of the character arcs, but still a solid read.",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user3Id",
                            IsDeleted = false,
                            Rating = 4
                        },
                        new
                        {
                            Id = 8,
                            BookId = 2,
                            Content = "Rowling’s world-building continues to amaze, even in the final installment.",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user4Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 9,
                            BookId = 3,
                            Content = "A timeless masterpiece. Tolkien’s world and characters are unmatched in depth and richness.",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user1Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 10,
                            BookId = 3,
                            Content = "The pacing was slow at times, but the payoff in the end was well worth it.",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user2Id",
                            IsDeleted = false,
                            Rating = 4
                        },
                        new
                        {
                            Id = 11,
                            BookId = 3,
                            Content = "The bond between Sam and Frodo is the heart of this epic journey. Beautifully written.",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user3Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 12,
                            BookId = 3,
                            Content = "An epic tale that defines the fantasy genre. Loved every moment of it.",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user4Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 13,
                            BookId = 3,
                            Content = "The attention to detail in Middle-earth is staggering. Tolkien is a true genius.",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user5Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 14,
                            BookId = 3,
                            Content = "A bit long for my liking, but undeniably one of the greatest stories ever told.",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user6Id",
                            IsDeleted = false,
                            Rating = 4
                        });
                });

            modelBuilder.Entity("BookHub.Server.Data.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "user1Id",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "6e71d9fd-cb0c-4c18-b4f8-8a09b98e5d2a",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "user1@mail.com",
                            EmailConfirmed = false,
                            IsDeleted = false,
                            LockoutEnabled = false,
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "e68f7799-169a-4e99-9261-f0eb6a3b640b",
                            TwoFactorEnabled = false,
                            UserName = "user1name"
                        },
                        new
                        {
                            Id = "user2Id",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "22d14517-4c8c-46d5-8f9e-b4499c809dbf",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "user2@mail.com",
                            EmailConfirmed = false,
                            IsDeleted = false,
                            LockoutEnabled = false,
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "a5fd96f5-4c8e-46a5-bfb8-83e15b4f681e",
                            TwoFactorEnabled = false,
                            UserName = "user2name"
                        },
                        new
                        {
                            Id = "user3Id",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "436849b5-3eb5-40da-a2e7-26c7d5a7ffb2",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "user3@mail.com",
                            EmailConfirmed = false,
                            IsDeleted = false,
                            LockoutEnabled = false,
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "d281b5d6-004c-4444-8673-7558cc795994",
                            TwoFactorEnabled = false,
                            UserName = "user3name"
                        },
                        new
                        {
                            Id = "user4Id",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "73a7b8b9-b407-4fdf-a560-bdd3aead7e0a",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "user4@mail.com",
                            EmailConfirmed = false,
                            IsDeleted = false,
                            LockoutEnabled = false,
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "f046f46c-94b3-420c-88e2-b23161a427fd",
                            TwoFactorEnabled = false,
                            UserName = "user4name"
                        },
                        new
                        {
                            Id = "user5Id",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "8c9f3281-db0f-4510-96d6-10d57968d8ea",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "user5@mail.com",
                            EmailConfirmed = false,
                            IsDeleted = false,
                            LockoutEnabled = false,
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "14c64b16-3440-4e95-be5a-a3ce681b7046",
                            TwoFactorEnabled = false,
                            UserName = "user5name"
                        },
                        new
                        {
                            Id = "user6Id",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "4618ca90-889c-46b9-b67e-70d98aed533c",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "user6@mail.com",
                            EmailConfirmed = false,
                            IsDeleted = false,
                            LockoutEnabled = false,
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "79cac276-bf73-4a7e-ac71-cbee59109869",
                            TwoFactorEnabled = false,
                            UserName = "user6name"
                        });
                });

            modelBuilder.Entity("BookHub.Server.Data.Models.UserProfile", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Biography")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<bool>("IsPrivate")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("SocialMediaUrl")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.HasKey("UserId");

                    b.ToTable("Profiles");
                });

            modelBuilder.Entity("BookHub.Server.Data.Models.Vote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsUpvote")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("ReviewId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("ReviewId");

                    b.ToTable("Votes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user2Id",
                            IsUpvote = true,
                            ReviewId = 1
                        },
                        new
                        {
                            Id = 2,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user3Id",
                            IsUpvote = true,
                            ReviewId = 1
                        },
                        new
                        {
                            Id = 3,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user4Id",
                            IsUpvote = true,
                            ReviewId = 1
                        },
                        new
                        {
                            Id = 4,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user5Id",
                            IsUpvote = true,
                            ReviewId = 1
                        },
                        new
                        {
                            Id = 5,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user6Id",
                            IsUpvote = false,
                            ReviewId = 1
                        },
                        new
                        {
                            Id = 6,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user2Id",
                            IsUpvote = true,
                            ReviewId = 2
                        },
                        new
                        {
                            Id = 7,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user3Id",
                            IsUpvote = true,
                            ReviewId = 2
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("BookHub.Server.Data.Models.Author", b =>
                {
                    b.HasOne("BookHub.Server.Data.Models.User", "Creator")
                        .WithMany("Authors")
                        .HasForeignKey("CreatorId");

                    b.HasOne("BookHub.Server.Data.Models.Nationality", "Nationality")
                        .WithMany("Authors")
                        .HasForeignKey("NationalityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");

                    b.Navigation("Nationality");
                });

            modelBuilder.Entity("BookHub.Server.Data.Models.Book", b =>
                {
                    b.HasOne("BookHub.Server.Data.Models.Author", "Author")
                        .WithMany("Books")
                        .HasForeignKey("AuthorId");

                    b.HasOne("BookHub.Server.Data.Models.User", "Creator")
                        .WithMany("Books")
                        .HasForeignKey("CreatorId");

                    b.Navigation("Author");

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("BookHub.Server.Data.Models.BookGenre", b =>
                {
                    b.HasOne("BookHub.Server.Data.Models.Book", "Book")
                        .WithMany("BooksGenres")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookHub.Server.Data.Models.Genre", "Genre")
                        .WithMany("BooksGenres")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("Genre");
                });

            modelBuilder.Entity("BookHub.Server.Data.Models.Notification", b =>
                {
                    b.HasOne("BookHub.Server.Data.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BookHub.Server.Data.Models.Reply", b =>
                {
                    b.HasOne("BookHub.Server.Data.Models.User", "Creator")
                        .WithMany("Replies")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookHub.Server.Data.Models.Review", "Review")
                        .WithMany("Replies")
                        .HasForeignKey("ReviewId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Creator");

                    b.Navigation("Review");
                });

            modelBuilder.Entity("BookHub.Server.Data.Models.Review", b =>
                {
                    b.HasOne("BookHub.Server.Data.Models.Book", "Book")
                        .WithMany("Reviews")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookHub.Server.Data.Models.User", "Creator")
                        .WithMany("Reviews")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("BookHub.Server.Data.Models.UserProfile", b =>
                {
                    b.HasOne("BookHub.Server.Data.Models.User", "User")
                        .WithOne("Profile")
                        .HasForeignKey("BookHub.Server.Data.Models.UserProfile", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("BookHub.Server.Data.Models.Vote", b =>
                {
                    b.HasOne("BookHub.Server.Data.Models.User", "Creator")
                        .WithMany("Votes")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookHub.Server.Data.Models.Review", "Review")
                        .WithMany("Votes")
                        .HasForeignKey("ReviewId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Creator");

                    b.Navigation("Review");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("BookHub.Server.Data.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("BookHub.Server.Data.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookHub.Server.Data.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("BookHub.Server.Data.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BookHub.Server.Data.Models.Author", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("BookHub.Server.Data.Models.Book", b =>
                {
                    b.Navigation("BooksGenres");

                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("BookHub.Server.Data.Models.Genre", b =>
                {
                    b.Navigation("BooksGenres");
                });

            modelBuilder.Entity("BookHub.Server.Data.Models.Nationality", b =>
                {
                    b.Navigation("Authors");
                });

            modelBuilder.Entity("BookHub.Server.Data.Models.Review", b =>
                {
                    b.Navigation("Replies");

                    b.Navigation("Votes");
                });

            modelBuilder.Entity("BookHub.Server.Data.Models.User", b =>
                {
                    b.Navigation("Authors");

                    b.Navigation("Books");

                    b.Navigation("Profile");

                    b.Navigation("Replies");

                    b.Navigation("Reviews");

                    b.Navigation("Votes");
                });
#pragma warning restore 612, 618
        }
    }
}
