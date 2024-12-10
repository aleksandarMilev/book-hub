namespace BookHub.Server.Tests.Services
{
    using AutoMapper;
    using BookHub.Server.Data.Models.Shared.ChatUser;
    using Data;
    using Features.Article.Data.Models;
    using Features.Authors.Data.Models;
    using Features.Book.Data.Models;
    using Features.Chat.Data.Models;
    using Features.Search.Mapper;
    using Features.Search.Service;
    using Features.Search.Service.Models;
    using Features.UserProfile.Data.Models;
    using Features.UserProfile.Service;
    using FluentAssertions;
    using Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class SearchServiceTests
    {
        private readonly BookHubDbContext data;

        private readonly Mock<ICurrentUserService> mockUserService;
        private readonly IMapper mapper;

        private readonly SearchService searchService;

        public SearchServiceTests()
        {
            var options = new DbContextOptionsBuilder<BookHubDbContext>()
                .UseInMemoryDatabase(databaseName: "SearchServiceInMemoryDatabase")
                .Options;

            this.mockUserService = new Mock<ICurrentUserService>();

            this.mockUserService
                .Setup(x => x.GetUsername())
                .Returns("current-user-username");

            this.mockUserService
                .Setup(x => x.GetId())
                .Returns("current-user-id");

            this.mockUserService
                .Setup(x => x.IsAdmin())
                .Returns(false);

            this.data = new BookHubDbContext(options, this.mockUserService.Object);

            this.mapper = new MapperConfiguration(cfg => cfg.AddProfile(new SearchMapper())).CreateMapper();

            this.searchService = new SearchService(
                this.data,
                this.mockUserService.Object,
                this.mapper
            );

            Task
                .Run(this.PrepareDbAsync)
                .GetAwaiter()
                .GetResult();
        }

        [Fact]
        public async Task BooksAsync_ShouldReturnAllBooks_WhenSearchTermIsNull()
        {
            var page = 1;
            var pageSize = 10;

            var result = await this.searchService.BooksAsync(null, page, pageSize);

            result.Items.Should().NotBeNullOrEmpty();
            result.Items.Should().AllBeOfType(typeof(SearchBookServiceModel));
            result.TotalItems.Should().Be(5); 
        }

        [Fact]
        public async Task BooksAsync_ShouldReturnPaginatedResult()
        {
            var page = 1;
            var pageSize = 2;

            var result = await this.searchService.BooksAsync(null, page, pageSize);

            result.Items.Should().HaveCount(pageSize);
            result.TotalItems.Should().Be(5);
            result.PageIndex.Should().Be(page);
            result.PageSize.Should().Be(pageSize);
        }

        [Fact]
        public async Task BooksAsync_ShouldFilterByTitle()
        {
            var searchTerm = "Pet";
            var page = 1;
            var pageSize = 10;

            var result = await this.searchService.BooksAsync(searchTerm, page, pageSize);

            result.Items.Should().HaveCount(1); 
            result.Items.First().Title.Should().Contain(searchTerm);
        }

        [Fact]
        public async Task BooksAsync_ShouldFilterByIntroduction()
        {
            var searchTerm = "Sometimes dead is";
            var page = 1;
            var pageSize = 10;

            var result = await this.searchService.BooksAsync(searchTerm, page, pageSize);

            result.Items.Should().HaveCount(1);
            result.Items.First().Title.Should().Be("Pet Sematary");
        }

        [Fact]
        public async Task BooksAsync_ShouldFilterByAuthorName()
        {
            var searchTerm = "Stephen K";
            var page = 1;
            var pageSize = 10;

            var result = await this.searchService.BooksAsync(searchTerm, page, pageSize);

            result.Items.Should().HaveCount(1);
            result.Items.First().Title.Should().Be("Pet Sematary");
        }

        [Fact]
        public async Task BooksAsync_ShouldBeCaseInsensitive()
        {
            var searchTerm = "PeT SEmAtAry";
            var page = 1;
            var pageSize = 10;

            var result = await this.searchService.BooksAsync(searchTerm, page, pageSize);

            result.Items.Should().HaveCount(1);
            result.Items.First().Title.Should().Be("Pet Sematary");
        }

        [Fact]
        public async Task BooksAsync_ShouldReturnEmptyCollection_WhenSearchTermDoesNotMatch()
        {
            var searchTerm = "invalid search term";
            var page = 1;
            var pageSize = 10;

            var result = await this.searchService.BooksAsync(searchTerm, page, pageSize);

            result.Items.Should().BeEmpty();
            result.TotalItems.Should().Be(0);
        }

        [Fact]
        public async Task ArticlesAsync_ShouldReturnAllArticles_WhenSearchTermIsNull()
        {
            var page = 1;
            var pageSize = 10;

            var result = await this.searchService.ArticlesAsync(null, page, pageSize);

            result.Items.Should().NotBeNullOrEmpty();
            result.Items.Should().AllBeOfType(typeof(SearchArticleServiceModel));
            result.TotalItems.Should().Be(4);
        }

        [Fact]
        public async Task ArticlesAsync_ShouldReturnPaginatedResult()
        {
            var page = 1;
            var pageSize = 2;

            var result = await this.searchService.ArticlesAsync(null, page, pageSize);

            result.Items.Should().HaveCount(pageSize);
            result.TotalItems.Should().Be(4);
            result.PageIndex.Should().Be(page);
            result.PageSize.Should().Be(pageSize);
        }

        [Fact]
        public async Task ArticlesAsync_ShouldFilterByTitle()
        {
            var searchTerm = "Exploring the Haunting";
            var page = 1;
            var pageSize = 10;

            var result = await this.searchService.ArticlesAsync(searchTerm, page, pageSize);

            result.Items.Should().HaveCount(1);
            result.Items.First().Title.Should().Contain(searchTerm);
        }

        [Fact]
        public async Task ArticlesAsync_ShouldFilterByIntroduction()
        {
            var searchTerm = "loss, and the chilling";
            var page = 1;
            var pageSize = 10;

            var result = await this.searchService.ArticlesAsync(searchTerm, page, pageSize);

            result.Items.Should().HaveCount(1);
            result.Items.First().Introduction.Should().Contain(searchTerm);
        }

        [Fact]
        public async Task ArticlesAsync_ShouldBeCaseInsensitive()
        {
            var searchTerm = "EXPloRinG tHE HauNTinG";
            var page = 1;
            var pageSize = 10;

            var result = await this.searchService.ArticlesAsync(searchTerm, page, pageSize);

            result.Items.Should().HaveCount(1);
            result.Items.First().Title.Should().Be("Exploring the Haunting Depths of Pet Sematary");
        }

        [Fact]
        public async Task ArticlesAsync_ShouldReturnEmptyCollection_WhenSearchTermDoesNotMatch()
        {
            var searchTerm = "invalid search term";
            var page = 1;
            var pageSize = 10;

            var result = await this.searchService.ArticlesAsync(searchTerm, page, pageSize);

            result.Items.Should().BeEmpty();
            result.TotalItems.Should().Be(0);
        }

        [Fact]
        public async Task AuthorsAsync_ShouldReturnAllAuthors_WhenSearchTermIsNull()
        {
            var page = 1;
            var pageSize = 10;

            var result = await this.searchService.AuthorsAsync(null, page, pageSize);

            result.Items.Should().NotBeNullOrEmpty();
            result.Items.Should().AllBeOfType(typeof(SearchAuthorServiceModel));
            result.TotalItems.Should().Be(5);
        }

        [Fact]
        public async Task AuthorsAsync_ShouldReturnPaginatedResult()
        {
            var page = 1;
            var pageSize = 2;

            var result = await this.searchService.AuthorsAsync(null, page, pageSize);

            result.Items.Should().HaveCount(pageSize);
            result.TotalItems.Should().Be(5);
            result.PageIndex.Should().Be(page);
            result.PageSize.Should().Be(pageSize);
        }

        [Fact]
        public async Task AuthorsAsync_ShouldFilterByName()
        {
            var searchTerm = "Steph";
            var page = 1;
            var pageSize = 10;

            var result = await this.searchService.AuthorsAsync(searchTerm, page, pageSize);

            result.Items.Should().HaveCount(1);
            result.Items.First().Name.Should().Contain(searchTerm);
        }

        [Fact]
        public async Task AuthorsAsync_ShouldFilterByPenName()
        {
            var searchTerm = "Bach";
            var page = 1;
            var pageSize = 10;

            var result = await this.searchService.AuthorsAsync(searchTerm, page, pageSize);

            result.Items.Should().HaveCount(1);
            result.Items.First().Name.Should().Be("Stephen King");
        }

        [Fact]
        public async Task AuthorsAsync_ShouldBeCaseInsensitive()
        {
            var searchTerm = "STEphEn KiNg";
            var page = 1;
            var pageSize = 10;

            var result = await this.searchService.AuthorsAsync(searchTerm, page, pageSize);

            result.Items.Should().HaveCount(1);
            result.Items.First().Name.Should().Be("Stephen King");
        }

        [Fact]
        public async Task AuthorsAsync_ShouldReturnEmptyCollection_WhenSearchTermDoesNotMatch()
        {
            var searchTerm = "invalid search term";
            var page = 1;
            var pageSize = 10;

            var result = await this.searchService.AuthorsAsync(searchTerm, page, pageSize);

            result.Items.Should().BeEmpty();
            result.TotalItems.Should().Be(0);
        }

        [Fact]
        public async Task ProfilesAsync_ShouldReturnAllProfiles_WhenSearchTermIsNull()
        {
            var page = 1;
            var pageSize = 10;

            var result = await this.searchService.ProfilesAsync(null, page, pageSize);

            result.Items.Should().NotBeNullOrEmpty();
            result.Items.Should().AllBeOfType(typeof(SearchProfileServiceModel));
            result.TotalItems.Should().Be(3);
        }

        [Fact]
        public async Task ProfilesAsync_ShouldReturnPaginatedResult()
        {
            var page = 1;
            var pageSize = 2;

            var result = await this.searchService.ProfilesAsync(null, page, pageSize);

            result.Items.Should().HaveCount(pageSize);
            result.TotalItems.Should().Be(3);
            result.PageIndex.Should().Be(page);
            result.PageSize.Should().Be(pageSize);
        }

        [Fact]
        public async Task ProfilesAsync_ShouldFilterByFristName()
        {
            var searchTerm = "John";
            var page = 1;
            var pageSize = 10;

            var result = await this.searchService.ProfilesAsync(searchTerm, page, pageSize);

            result.Items.Should().HaveCount(2);
            result.Items.Any(p => p.FirstName == "John").Should().BeTrue();
            result.Items.Any(p => p.LastName == "Johnson").Should().BeTrue();
        }

        [Fact]
        public async Task ProfilesAsync_ShouldFilterByLastName()
        {
            var searchTerm = "Doe";
            var page = 1;
            var pageSize = 10;

            var result = await this.searchService.ProfilesAsync(searchTerm, page, pageSize);

            result.Items.Should().HaveCount(1);
            result.Items.First().LastName.Should().Be(searchTerm);
        }

        [Fact]
        public async Task ProfilesAsync_ShouldFilter_ConcatenatedFirstAndLastName()
        {
            var searchTerm = "John D";
            var page = 1;
            var pageSize = 10;

            var result = await this.searchService.ProfilesAsync(searchTerm, page, pageSize);

            result.Items.Should().HaveCount(1);
            result.Items.First().FirstName.Should().Be("John");
            result.Items.First().LastName.Should().Be("Doe");
        }

        [Fact]
        public async Task ProfilesAsync_ShouldBeCaseInsensitive()
        {
            var searchTerm = "jOhN DoE";
            var page = 1;
            var pageSize = 10;

            var result = await this.searchService.ProfilesAsync(searchTerm, page, pageSize);

            result.Items.Should().HaveCount(1);
            result.Items.First().FirstName.Should().Be("John");
            result.Items.First().LastName.Should().Be("Doe");
        }

        [Fact]
        public async Task ProfilesAsync_ShouldReturnEmptyCollection_WhenSearchTermDoesNotMatch()
        {
            var searchTerm = "invalid search term";
            var page = 1;
            var pageSize = 10;

            var result = await this.searchService.ProfilesAsync(searchTerm, page, pageSize);

            result.Items.Should().BeEmpty();
            result.TotalItems.Should().Be(0);
        }


























        [Fact]
        public async Task ChatsAsync_ShouldReturnAllChats_WhereCurrentUserIsParticipant_AndAlso_HasAcceptedToJoin_WhenSearchTermIsNull()
        {
            this.SetCurrentUserId("user1Id");

            var page = 1;
            var pageSize = 10;

            var result = await this.searchService.ChatsAsync(null, page, pageSize);

            result.Items.Should().NotBeNullOrEmpty();
            result.Items.Should().AllBeOfType(typeof(SearchChatServiceModel));
            result.TotalItems.Should().Be(2);
        }

        [Fact]
        public async Task ChatsAsync_ShouldReturnPaginatedResult()
        {
            this.SetCurrentUserId("user1Id");

            var page = 1;
            var pageSize = 2;

            var result = await this.searchService.ChatsAsync(null, page, pageSize);

            result.Items.Should().HaveCount(pageSize);
            result.TotalItems.Should().Be(2);
            result.PageIndex.Should().Be(page);
            result.PageSize.Should().Be(pageSize);
        }

        [Fact]
        public async Task ChatsAsync_ShouldFilterByName()
        {
            this.SetCurrentUserId("user1Id");

            var searchTerm = "Chat 1";
            var page = 1;
            var pageSize = 10;

            var result = await this.searchService.ChatsAsync(searchTerm, page, pageSize);

            result.Items.Should().HaveCount(1);
            result.Items.First().Name.Should().Be(searchTerm);
        }

        [Fact]
        public async Task ChatsAsync_ShouldBeCaseInsensitive()
        {
            this.SetCurrentUserId("user1Id");

            var searchTerm = "ChAt 1";
            var page = 1;
            var pageSize = 10;

            var result = await this.searchService.ChatsAsync(searchTerm, page, pageSize);

            result.Items.Should().HaveCount(1);
            result.Items.First().Name.Should().Be("Chat 1");
        }

        [Fact]
        public async Task ChatsAsync_ShouldReturnEmptyCollection_WhenSearchTermDoesNotMatch()
        {
            var searchTerm = "invalid search term";
            var page = 1;
            var pageSize = 10;

            var result = await this.searchService.ChatsAsync(searchTerm, page, pageSize);

            result.Items.Should().BeEmpty();
            result.TotalItems.Should().Be(0);
        }

        private void SetCurrentUserId(string userId)
            => this.mockUserService
                .Setup(x => x.GetId())
                .Returns(userId);

        private async Task PrepareDbAsync()
        {
            if (!await this.data.Books.AnyAsync())
            {
                var books = new Book[]
                {
                    new()
                    {
                        Id = 1,
                        Title = "Pet Sematary",
                        ShortDescription = "Sometimes dead is better.",
                        LongDescription =
                            "Pet Sematary is a 1983 horror novel by American writer Stephen King. " +
                            "The story revolves around the Creed family who move into a rural home near a pet cemetery that has " +
                            "the power to resurrect the dead. However, the resurrected creatures return with sinister changes. " +
                            "The novel explores themes of grief, mortality, and the consequences of tampering with nature. " +
                            "It was nominated for a World Fantasy Award for Best Novel in 1984. " +
                            "The book was adapted into two films: one in 1989 directed by Mary Lambert and another in 2019 directed by Kevin Kölsch and " +
                            "Dennis Widmyer. In November 2013, PS Publishing released Pet Sematary in a limited 30th-anniversary edition, " +
                            "further solidifying its status as a classic in horror literature.",
                        AverageRating = 4.25,
                        RatingsCount = 4,
                        ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/24/Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg/330px-Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg",
                        AuthorId = 1,
                        PublishedDate = new DateTime(1983, 11, 4),
                        IsApproved = true
                    },
                    new()
                    {
                        Id = 2,
                        Title = "Harry Potter and the Deathly Hallows",
                        ShortDescription = "The last book from the Harry Potter series.",
                        LongDescription =
                            "Harry Potter and the Deathly Hallows is a fantasy novel written by British author J.K. Rowling. " +
                            "It is the seventh and final book in the Harry Potter series and concludes the epic tale of Harry's battle against Lord Voldemort. " +
                            "The story begins with Harry, Hermione, and Ron embarking on a dangerous quest to locate and destroy Voldemort's Horcruxes, " +
                            "which are key to his immortality. Along the way, they uncover secrets about the Deathly Hallows—three powerful magical objects " +
                            "that could aid them in their fight. The book builds to an intense and emotional climax at the Battle of Hogwarts, " +
                            "where Harry confronts Voldemort for the last time. Released on 21 July 2007, the book became a cultural phenomenon, " +
                            "breaking sales records and receiving critical acclaim for its complex characters, intricate plotting, and resonant themes " +
                            "of sacrifice, friendship, and love.",
                        AverageRating = 4.75,
                        RatingsCount = 4,
                        ImageUrl = "https://librerialaberintopr.com/cdn/shop/products/hallows_459x.jpg?v=1616596206",
                        AuthorId = 2,
                        PublishedDate = new DateTime(2007, 07, 21),
                        IsApproved = true
                    },
                    new()
                    {
                        Id = 3,
                        Title = "Lord of the Rings",
                        ShortDescription = "The Lord of the Rings is an epic fantasy novel by the English author J. R. R. Tolkien.",
                        LongDescription =
                            "The Lord of the Rings is a high-fantasy novel written by J.R.R. Tolkien and set in the fictional world of Middle-earth. " +
                            "The story follows the journey of Frodo Baggins, a humble hobbit who inherits the One Ring—a powerful artifact created by the Dark Lord Sauron " +
                            "to control Middle-earth. Along with a fellowship of companions, Frodo sets out on a perilous mission to destroy the ring in the fires of Mount Doom, " +
                            "the only place where it can be unmade. The narrative interweaves themes of friendship, courage, sacrifice, and the corrupting influence of power. " +
                            "Written in stages between 1937 and 1949, the novel is widely regarded as one of the greatest works of fantasy literature, " +
                            "influencing countless authors and spawning adaptations, including Peter Jackson's acclaimed film trilogy. " +
                            "With over 150 million copies sold, it remains one of the best-selling books of all time, praised for its richly detailed world-building, " +
                            "complex characters, and timeless appeal.",
                        AverageRating = 4.67,
                        RatingsCount = 6,
                        ImageUrl = "https://upload.wikimedia.org/wikipedia/en/e/e9/First_Single_Volume_Edition_of_The_Lord_of_the_Rings.gif",
                        AuthorId = 3,
                        PublishedDate = new DateTime(1954, 07, 29),
                        IsApproved = true
                    },
                    new()
                    {
                        Id = 4,
                        Title = "1984",
                        ShortDescription = "A dystopian novel exploring the dangers of totalitarianism.",
                        LongDescription =
                            "1984, written by George Orwell, is a dystopian novel set in a totalitarian society under the omnipresent surveillance " +
                            "of Big Brother. Published in 1949, the story follows Winston Smith, a low-ranking member of the Party, as he secretly rebels against " +
                            "the oppressive regime. Through his illicit love affair with Julia and his pursuit of forbidden knowledge, Winston challenges the Party's " +
                            "control over truth, history, and individuality. The novel introduces concepts such as 'doublethink,' 'Newspeak,' and 'thoughtcrime,' " +
                            "which have since become part of modern political discourse. Widely regarded as a classic of English literature, 1984 is a chilling exploration " +
                            "of propaganda, censorship, and the erosion of personal freedoms, serving as a cautionary tale for future generations.",
                        AverageRating = 4.8,
                        RatingsCount = 5,
                        ImageUrl = "https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1327144697i/3744438.jpg",
                        AuthorId = 5,
                        PublishedDate = new DateTime(1949, 6, 8),
                        IsApproved = true
                    },
                    new()
                    {
                        Id = 5,
                        Title = "One Flew Over the Cuckoo's Nest",
                        ShortDescription = "A story about individuality and institutional control.",
                        LongDescription =
                            "One Flew Over the Cuckoo's Nest, a novel by Ken Kesey, takes place in a mental institution " +
                            "and explores themes of individuality, freedom, and rebellion against oppressive systems. The protagonist, Randle P. McMurphy, " +
                            "a charismatic convict, fakes insanity to serve his sentence in a psychiatric hospital instead of prison. " +
                            "He clashes with Nurse Ratched, the authoritarian head nurse, and inspires the other patients to assert their independence. " +
                            "The story, narrated by Chief Bromden, a silent observer and fellow patient, examines the dynamics of power and the human spirit's resilience. " +
                            "Published in 1962, the book was adapted into a 1975 film that won five Academy Awards. It remains a poignant critique of institutional " +
                            "control and a celebration of nonconformity.",
                        AverageRating = 4.83,
                        RatingsCount = 6,
                        ImageUrl = "https://m.media-amazon.com/images/I/61Lpsc7B3jL.jpg",
                        AuthorId = 4,
                        PublishedDate = new DateTime(1962, 2, 1),
                        IsApproved = true
                    }
                };

                this.data.AddRange(books);
                await this.data.SaveChangesAsync();
            }

            if (!await this.data.Articles.AnyAsync())
            {
                var articles = new Article[]
                {
                    new()
                    {
                        Id = 1,
                        Title = "Exploring the Haunting Depths of Pet Sematary",
                        Introduction = "A tale of love, loss, and the chilling cost of defying death.",
                        Content =
                            "Stephen King's Pet Sematary is not just a horror story; it is a poignant exploration of grief, the inevitability of death, and the dark corners of the human psyche. " +
                            "At its heart, the novel is about Louis Creed, a young father who moves with his wife, Rachel, and their two children to a rural house in Maine. Soon after arriving, they discover a " +
                            "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                            "for a nightmare that challenges Louis' understanding of life, death, and morality."
                        ,
                        ImageUrl = "https://thedailytexan.com/wp-content/uploads/2023/09/pet_sematary_bloodlines_4press-1200x800.jpg",
                        Views = 0,
                        CreatedOn = DateTime.Now
                    },
                    new()
                    {
                        Id = 2,
                        Title = "The Epic Conclusion: Harry Potter and the Deathly Hallows",
                        Introduction = "The battle against Voldemort reaches its thrilling and emotional finale.",
                        Content =
                            "Harry Potter and the Deathly Hallows is not just the final book in J. K. Rowling’s iconic series, but the culmination of a journey that began with a young, orphaned wizard discovering " +
                            "his destiny in a world full of magic, mystery, and danger. The seventh and final installment brings to a close the epic battle between Harry Potter and the dark wizard Lord Voldemort, " +
                            "and it is a story that revolves around themes of love, sacrifice, loyalty, and the ultimate quest for truth. As the story reaches its climax, readers are taken on a journey not only through " +
                            "the magical world but also through the very heart of what it means to face evil, to stand up for what is right, and to embrace the unknown."
                        ,
                        ImageUrl = "https://choicefineart.com/cdn/shop/products/book-7-harry-potter-and-the-deathly-hallows-311816.jpg?v=1688079541",
                        Views = 0,
                        CreatedOn = DateTime.Now
                    },
                    new()
                    {
                        Id = 3,
                        Title = "The Timeless Epic: The Lord of the Rings",
                        Introduction = "A journey of courage, friendship, and the fight against overwhelming darkness.",
                        Content =
                            "The Lord of the Rings is an epic fantasy novel by J. R. R. Tolkien, widely regarded as one of the greatest works " +
                            "of literature in the 20th century. The novel follows the journey of a young hobbit, Frodo Baggins, as he is tasked " +
                            "with destroying a powerful and corrupt artifact known as the One Ring, which was created by the Dark Lord Sauron to " +
                            "rule over all of Middle-earth. As the story unfolds, Frodo and his companions embark on an arduous and perilous quest " +
                            "to reach the fires of Mount Doom, where the Ring must be destroyed in order to defeat Sauron and save their world. " +
                            "However, the journey is far from simple—trapped by the corrupting influence of the Ring, the fellowship of travelers " +
                            "must navigate treachery, loss, and immense personal challenges, all while facing the growing darkness of Sauron's forces."
                        ,
                        ImageUrl = "https://blogger.googleusercontent.com/img/b/R29vZ2xl/AVvXsEibI7_-Az0QVZhhwZO_PcgrNRK7RYnS7JPiddt_LvTC8NTgTzzYcaagGBLR6KtgY1J_VyZzS6HhL7MW9x1h-rioISPanc-daPbdgnZCQQb48PNELDt9gbQlohCJuXGHgritNS_3Ff08oUhs/w1200-h630-p-k-no-nu/acetolkien.jpg",
                        Views = 0,
                        CreatedOn = DateTime.Now
                    },
                    new()
                    {
                    Id = 4,
                    Title = "The Haunting Legacy of The Shining",
                    Introduction = "A chilling exploration of isolation, madness, and the supernatural, The Shining takes readers deep into the heart of terror.",
                    Content =
                        "The Shining is a psychological horror novel by Stephen King, first published in 1977. The story follows Jack Torrance, " +
                        "an aspiring writer and recovering alcoholic, who takes a job as the winter caretaker of the Overlook Hotel, a remote " +
                        "resort nestled in the mountains of Colorado. Jack moves to the hotel with his wife Wendy and his young son Danny, " +
                        "who possesses a psychic ability called 'the shining.' As the winter weather isolates the family from the outside world, " +
                        "the Overlook's dark, haunted history begins to unravel, and the hotel's malevolent supernatural forces begin to take " +
                        "control of Jack, pushing him toward violence and madness. What begins as a simple family retreat spirals into a nightmare " +
                        "of terror and survival, as Danny and Wendy fight to escape the hotel's deadly grip. "
                    ,
                    ImageUrl = "https://kcopera.org/wp-content/uploads/2024/03/the-shining-recording-page-banner.jpg",
                    Views = 0,
                    CreatedOn = DateTime.Now
                    },
                };

                this.data.AddRange(articles);
                await this.data.SaveChangesAsync();
            }

            if (!await this.data.Authors.AnyAsync())
            {
                var authors = new Author[]
                {
                    new()
                    {
                        Id = 1,
                        Name = "Stephen King",
                        ImageUrl = "https://media.npr.org/assets/img/2020/04/07/stephen-king.by-shane-leonard_wide-c132de683d677c7a6a1e692f86a7e6670baf3338.jpg?s=1100&c=85&f=jpeg",
                        Biography =
                            "Stephen Edwin King (born September 21, 1947) is an American author of horror, supernatural fiction, suspense, crime, " +
                            "and fantasy novels. He is often referred to as the 'King of Horror,' but his work spans many genres, earning him a reputation as " +
                            "one of the most prolific and successful authors of our time. King's novels have sold more than 350 million copies, many of which " +
                            "have been adapted into feature films,  works include Carrie, The Shining,"
                        ,
                        PenName = "Richard Bachman",
                        RatingsCount = 14,
                        AverageRating = 4.7,
                        NationalityId = 182,
                        Gender = Gender.Male,
                        BornAt = new DateTime(1947, 09, 21),
                        IsApproved = true
                    },
                    new()
                    {
                        Id = 2,
                        Name = "Joanne Rowling",
                        ImageUrl = "https://stories.jkrowling.com/wp-content/uploads/2021/09/Shot-B-105_V2_CROP-e1630873059779.jpg",
                        Biography =
                            "Stephen Edwin King (born September 21, 1947) is an American author of horror, supernatural fiction, suspense, crime, " +
                            "and fantasy novels. He is often referred to as the 'King of Horror,' but his work spans many genres, earning him a reputation as " +
                            "one of the most prolific and successful authors of our time. King's novels have sold more than 350 million copies, many of which " +
                            "have been adapted into feature films,  works include Carrie, The Shining,"
                        ,
                        PenName = "J. K. Rowling",
                        RatingsCount = 4,
                        AverageRating = 4.75,
                        NationalityId = 181,
                        Gender = Gender.Female,
                        BornAt = new DateTime(1965, 07, 31),
                        IsApproved = true
                    },
                    new()
                    {
                        Id = 3,
                        Name = "John Ronald Reuel Tolkien ",
                        ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d4/J._R._R._Tolkien%2C_ca._1925.jpg/330px-J._R._R._Tolkien%2C_ca._1925.jpg",
                        Biography =
                            "Stephen Edwin King (born September 21, 1947) is an American author of horror, supernatural fiction, suspense, crime, " +
                            "and fantasy novels. He is often referred to as the 'King of Horror,' but his work spans many genres, earning him a reputation as " +
                            "one of the most prolific and successful authors of our time. King's novels have sold more than 350 million copies, many of which " +
                            "have been adapted into feature films,  works include Carrie, The Shining,"
                        ,
                        PenName = "J.R.R Tolkien",
                        RatingsCount = 6,
                        AverageRating = 4.67,
                        NationalityId = 181,
                        Gender = Gender.Male,
                        BornAt = new DateTime(1892, 01, 03),
                        DiedAt = new DateTime(1973, 09, 02),
                        IsApproved = true
                    },
                    new()
                    {
                        Id = 4,
                        Name = "Ken Kesey",
                        ImageUrl = "https://upload.wikimedia.org/wikipedia/en/9/9b/Ken_Kesey%2C_American_author%2C_1935-2001.jpg",
                        Biography =
                            "Stephen Edwin King (born September 21, 1947) is an American author of horror, supernatural fiction, suspense, crime, " +
                            "and fantasy novels. He is often referred to as the 'King of Horror,' but his work spans many genres, earning him a reputation as " +
                            "one of the most prolific and successful authors of our time. King's novels have sold more than 350 million copies, many of which " +
                            "have been adapted into feature films,  works include Carrie, The Shining,"
                        ,
                        PenName = null,
                        RatingsCount = 6,
                        AverageRating = 4.6,
                        NationalityId = 182,
                        Gender = Gender.Male,
                        BornAt = new DateTime(1935, 09, 17),
                        DiedAt = new DateTime(2001, 11, 10),
                        IsApproved = true
                    },
                    new()
                    {
                        Id = 5,
                        Name = "George Orwell",
                        ImageUrl = "https://hips.hearstapps.com/hmg-prod/images/george-orwell.jpg",
                        Biography =
                              "Stephen Edwin King (born September 21, 1947) is an American author of horror, supernatural fiction, suspense, crime, " +
                            "and fantasy novels. He is often referred to as the 'King of Horror,' but his work spans many genres, earning him a reputation as " +
                            "one of the most prolific and successful authors of our time. King's novels have sold more than 350 million copies, many of which " +
                            "have been adapted into feature films,  works include Carrie, The Shining,"
                        ,
                        PenName = "George Orwell",
                        RatingsCount = 7,
                        AverageRating = 4.9,
                        NationalityId = 181,
                        Gender = Gender.Male,
                        BornAt = new DateTime(1903, 06, 25),
                        DiedAt = new DateTime(1950, 01, 21),
                        IsApproved = true
                    }
                };

                this.data.AddRange(authors);
                await this.data.SaveChangesAsync();
            }

            if (!await this.data.Profiles.AnyAsync())
            {
                var profiles = new UserProfile[]
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

                this.data.AddRange(profiles);
                await this.data.SaveChangesAsync();
            }

            if (!await this.data.Chats.AnyAsync())
            {
                var chats = new Chat[]
                {
                    new()
                    { 
                        Id = 1,
                        Name = "Chat 1",
                        ImageUrl = "https://mock.image.com",
                        CreatorId = "user1Id",
                    },
                    new()
                    {
                        Id = 2,
                        Name = "Chat 2",
                        ImageUrl = "https://mock.image.com",
                        CreatorId = "user2Id",
                    },
                    new()
                    {
                        Id = 3,
                        Name = "Chat 3",
                        ImageUrl = "https://mock.image.com",
                        CreatorId = "user3Id",
                    },
                };

                var chatsUsers = new ChatUser[]
                {
                    new()
                    { 
                        UserId = "user1Id",
                        ChatId = 1,
                        HasAccepted = true
                    },
                    new()
                    {
                        UserId = "user2Id",
                        ChatId = 1,
                        HasAccepted = true
                    },
                    new()
                    {
                        UserId = "user3Id",
                        ChatId = 1,
                        HasAccepted = false
                    },

                    new()
                    {
                        UserId = "user2Id",
                        ChatId = 2,
                        HasAccepted = true
                    },
                    new()
                    {
                        UserId = "user3Id",
                        ChatId = 2,
                        HasAccepted = true
                    },
                    new()
                    {
                        UserId = "user1Id",
                        ChatId = 2,
                        HasAccepted = false
                    },

                    new()
                    {
                        UserId = "user3Id",
                        ChatId = 3,
                        HasAccepted = true
                    },
                    new()
                    {
                        UserId = "user1Id",
                        ChatId = 3,
                        HasAccepted = true
                    },
                    new()
                    {
                        UserId = "user2Id",
                        ChatId = 3,
                        HasAccepted = false
                    },
                };

                this.data.AddRange(chats);
                this.data.AddRange(chatsUsers);
                await this.data.SaveChangesAsync();
            }
        }
    }
}
