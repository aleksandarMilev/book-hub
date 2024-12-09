namespace BookHub.Server.Tests.Services
{
    using AutoMapper;
    using Data;
    using Features.Book.Data.Models;
    using Features.ReadingList.Data.Models;
    using Features.ReadingList.Mapper;
    using Features.ReadingList.Service;
    using Features.UserProfile.Data.Models;
    using Features.UserProfile.Service;
    using FluentAssertions;
    using Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class ReadingListServiceTests
    {
        private readonly BookHubDbContext data;

        private readonly Mock<ICurrentUserService> mockUserService;
        private readonly Mock<IProfileService> mockProfileService;
        private readonly IMapper mapper;

        private readonly ReadingListService readingListService;

        public ReadingListServiceTests()
        {
            var options = new DbContextOptionsBuilder<BookHubDbContext>()
                .UseInMemoryDatabase(databaseName: "ReadingListServiceInMemoryDatabase")
                .Options;

            this.mockUserService = new Mock<ICurrentUserService>();
            this.mockProfileService = new Mock<IProfileService>();

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

            this.mapper = new MapperConfiguration(cfg => cfg.AddProfile(new ReadingListMapper())).CreateMapper();

            this.readingListService = new ReadingListService(
                this.data,
                this.mockUserService.Object,
                this.mockProfileService.Object,
                this.mapper
            );

            Task
                .Run(this.PrepareDbAsync)
                .GetAwaiter()
                .GetResult();
        }

        [Fact]
        public async Task AddAsync_ShouldReturnErrorMessage_IfUserHasMoreThanFiveBooksInThisList()
        {
            this.mockProfileService
                .Setup(x => x.MoreThanFiveCurrentlyReadingAsync("current-user-id"))
                .ReturnsAsync(true);

            var result = await this.readingListService.AddAsync(1, "CurrentlyReading");
            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Be("User can not add more than 5 books in the currently reading list!");
        }

        [Fact]
        public async Task AddAsync_ShouldReturnErrorMessage_IfMapEntityAlreadyExist()
        {
            foreach (var rl in this.data.ReadingLists)
            {
                this.data.Remove(rl);
            }

            await this.data.SaveChangesAsync();

            var newRl = new ReadingList()
            {
                UserId = "current-user-id",
                BookId = 1,
                Status = ReadingListStatus.Read,
            };

            this.data.Add(newRl);
            await this.data.SaveChangesAsync();

            this.mockProfileService
                .Setup(x => x.MoreThanFiveCurrentlyReadingAsync("current-user-id"))
                .ReturnsAsync(false);

            var result = await this.readingListService.AddAsync(1, "Read");
            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Be("This book is already added in the user list!");
        }

        [Fact]
        public async Task AddAsync_ShouldReturnTrue_IfMapEntityCreated()
        {
            foreach (var rl in this.data.ReadingLists)
            {
                this.data.Remove(rl);
            }

            await this.data.SaveChangesAsync();

            this.mockProfileService
                .Setup(x => x.MoreThanFiveCurrentlyReadingAsync("current-user-id"))
                .ReturnsAsync(false);

            var result = await this.readingListService.AddAsync(1, "Read");
            result.Succeeded.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnErrorMessage_IfMapEntityNotFound()
        {
            foreach (var rl in this.data.ReadingLists)
            {
                this.data.Remove(rl);
            }

            await this.data.SaveChangesAsync();

            var newRl = new ReadingList()
            {
                UserId = "current-user-id",
                BookId = 1,
                Status = ReadingListStatus.Read,
            };

            this.data.Add(newRl);
            await this.data.SaveChangesAsync();

            this.mockProfileService
                .Setup(x => x.MoreThanFiveCurrentlyReadingAsync("current-user-id"))
                .ReturnsAsync(false);

            var result = await this.readingListService.DeleteAsync(2, "Read");
            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Be("The book has not been found in the user list!");
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_IfMapEntityDeleted()
        {
            foreach (var rl in this.data.ReadingLists)
            {
                this.data.Remove(rl);
            }

            await this.data.SaveChangesAsync();

            var newRl = new ReadingList()
            {
                UserId = "current-user-id",
                BookId = 1,
                Status = ReadingListStatus.Read,
            };

            this.data.Add(newRl);
            await this.data.SaveChangesAsync();

            this.mockProfileService
                .Setup(x => x.MoreThanFiveCurrentlyReadingAsync("current-user-id"))
                .ReturnsAsync(false);

            var result = await this.readingListService.DeleteAsync(1, "Read");
            result.Succeeded.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();
        }

        [Fact]
        public async Task AllAsync_ShouldReturnPaginatedBooks_IfBooksExist()
        {
            foreach (var rl in this.data.ReadingLists)
            {
                this.data.Remove(rl);
            }

            await this.data.SaveChangesAsync();

            var status = "Read";
            var userId = "current-user-id";

            var readingLists = new List<ReadingList>()
            {
                new() { UserId = userId, BookId = 1, Status = ReadingListStatus.Read },
                new() { UserId = userId, BookId = 2, Status = ReadingListStatus.Read },
                new() { UserId = userId, BookId = 3, Status = ReadingListStatus.Read },
            };

            this.data.AddRange(readingLists);
            await this.data.SaveChangesAsync();

            var result = await this.readingListService.AllAsync(userId, status, 1, 2);

            result.Items.Should().HaveCount(2);
            result.TotalItems.Should().Be(3);
            result.PageIndex.Should().Be(1);
            result.PageSize.Should().Be(2);
        }

        [Fact]
        public async Task AllAsync_ShouldReturnEmptyList_IfNoBooksMatchCriteria()
        {
            foreach (var rl in this.data.ReadingLists)
            {
                this.data.Remove(rl);
            }

            await this.data.SaveChangesAsync();

            var status = "CurrentlyReading";
            var userId = "current-user-id";

            var readingLists = new List<ReadingList>()
            {
                new() { UserId = userId, BookId = 1, Status = ReadingListStatus.Read },
                new() { UserId = userId, BookId = 2, Status = ReadingListStatus.Read },
            };

            this.data.AddRange(readingLists);
            await this.data.SaveChangesAsync();

            var result = await this.readingListService.AllAsync(userId, status, 1, 2);

            result.Items.Should().BeEmpty();
            result.TotalItems.Should().Be(0);
        }

        [Fact]
        public async Task AllAsync_ShouldThrowReadingListTypeException_IfStatusIsInvalid()
        {
            foreach (var rl in this.data.ReadingLists)
            {
                this.data.Remove(rl);
            }

            await this.data.SaveChangesAsync();

            var invalidStatus = "InvalidStatus";
            var userId = "current-user-id";

            var readingLists = new List<ReadingList>()
            {
                new() { UserId = userId, BookId = 1, Status = ReadingListStatus.Read },
                new() { UserId = userId, BookId = 2, Status = ReadingListStatus.Read },
            };

            this.data.AddRange(readingLists);
            await this.data.SaveChangesAsync();

            var allAsync = async () =>
            {
                await this.readingListService.AllAsync(userId, invalidStatus, 1, 2);
            };

            await allAsync
                .Should()
                .ThrowAsync<ReadingListTypeException>()
                .WithMessage($"{invalidStatus} is invalid reading list status type!");
        }

        [Fact]
        public async Task AllAsync_ShouldReturnCorrectPagination_WhenPageIndexIsBeyondRange()
        {
            foreach (var rl in this.data.ReadingLists)
            {
                this.data.Remove(rl);
            }

            await this.data.SaveChangesAsync();

            var status = "Read";
            var userId = "current-user-id";

            var readingLists = new List<ReadingList>()
            {
                new() { UserId = userId, BookId = 1, Status = ReadingListStatus.Read },
                new() { UserId = userId, BookId = 2, Status = ReadingListStatus.Read },
            };

            this.data.AddRange(readingLists);
            await this.data.SaveChangesAsync();

            var result = await this.readingListService.AllAsync(userId, status, 2, 2);

            result.Items.Should().BeEmpty();
            result.TotalItems.Should().Be(2);
            result.PageIndex.Should().Be(2);
            result.PageSize.Should().Be(2);
        }

        [Fact]
        public async Task AllAsync_ShouldReturnCorrectPagination_WhenPageSizeIsSmallerThanTotalItems()
        {
            foreach (var rl in this.data.ReadingLists)
            {
                this.data.Remove(rl);
            }

            await this.data.SaveChangesAsync();

            var status = "Read";
            var userId = "current-user-id";

            var readingLists = new List<ReadingList>()
            {
                new() { UserId = userId, BookId = 1, Status = ReadingListStatus.Read },
                new() { UserId = userId, BookId = 2, Status = ReadingListStatus.Read },
                new() { UserId = userId, BookId = 3, Status = ReadingListStatus.Read },
            };

            this.data.AddRange(readingLists);
            await this.data.SaveChangesAsync();

            var result = await this.readingListService.AllAsync(userId, status, 1, 2);

            result.Items.Should().HaveCount(2);
            result.TotalItems.Should().Be(3);
            result.PageIndex.Should().Be(1);
            result.PageSize.Should().Be(2);
        }

        private async Task PrepareDbAsync()
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

            var profiles = new UserProfile[]
            {
                new()
                {
                    UserId = "current-user-id",
                    FirstName = "Current",
                    LastName = "User",
                    ImageUrl = "https://www.shareicon.net/data/512x512/2016/05/24/770117_people_512x512.png",
                    PhoneNumber = "+1234567890",
                    DateOfBirth = new DateTime(1990, 05, 15),
                    SocialMediaUrl = "https://twitter.com/johndoe",
                    Biography = "Current User Profile.",
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

            var readingList = new ReadingList()
            {
                BookId = 1,
                UserId = "current-user-id",
                Status = ReadingListStatus.Read
            };

            if (!await this.data.Books.AnyAsync())
            {
                this.data.AddRange(books);
            }

            if (!await this.data.Profiles.AnyAsync())
            {
                this.data.AddRange(profiles);
            }

            if (!await this.data.ReadingLists.AnyAsync())
            {
                this.data.Add(readingList);
            }

            await this.data.SaveChangesAsync();
        }
    }
}
