namespace BookHub.Server.Tests.Services
{
    using Areas.Admin.Service;
    using AutoMapper;
    using Data;
    using Data.Models.Shared.BookGenre;
    using Features.Authors.Data.Models;
    using Features.Book.Data.Models;
    using Features.Book.Mapper;
    using Features.Book.Service;
    using Features.Book.Service.Models;
    using Features.Genre.Data.Models;
    using Features.Notification.Service;
    using Features.UserProfile.Service;
    using FluentAssertions;
    using Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    using static Common.ErrorMessage;

    public class BookServiceTest 
    {
        private readonly BookHubDbContext data;

        private readonly Mock<ICurrentUserService> mockUserService;
        private readonly Mock<IAdminService> mockAdminService;
        private readonly Mock<INotificationService> mockNotificationService;
        private readonly Mock<IProfileService> mockProfileService;
        private readonly IMapper mapper;

        private readonly BookService bookService;

        public BookServiceTest()
        {
            var options = new DbContextOptionsBuilder<BookHubDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
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
                .Returns(true);

            this.data = new BookHubDbContext(options, this.mockUserService.Object);

            this.mockAdminService = new Mock<IAdminService>();
            this.mockNotificationService = new Mock<INotificationService>();
            this.mockProfileService = new Mock<IProfileService>();
            this.mapper = new MapperConfiguration(cfg => cfg.AddProfile(new BookMapper())).CreateMapper();

            this.bookService = new BookService(
                this.data,
                this.mockUserService.Object,
                this.mockAdminService.Object,
                this.mockNotificationService.Object,
                this.mockProfileService.Object,
                this.mapper
            );

            Task
                .Run(this.PrepareDbAsync)
                .GetAwaiter()
                .GetResult();
        }

        [Fact]
        public async Task TopThreeAsync_ShouldReturn_ThreeBooksOrderedByRating()
        {
            var topBooks = await this.bookService.TopThreeAsync();

            topBooks.Should().HaveCount(3);

            topBooks
                .First()
                .AverageRating
                .Should()
                .BeGreaterThanOrEqualTo(topBooks.Skip(1).First().AverageRating);

            topBooks
                .Skip(1)
                .First()
                .AverageRating
                .Should()
                .BeGreaterThanOrEqualTo(topBooks.Last().AverageRating);
        }

        [Fact]
        public async Task ByGenreAsync_ShouldReturn_PaginatedBooksOrderedByRating()
        {
            int genreId = 7;
            int page = 1;
            int pageSize = 2;

            var paginatedResult = await this.bookService.ByGenreAsync(genreId, page, pageSize);

            paginatedResult
                .Items
                .Should()
                .HaveCount(2);

            paginatedResult
                .Items
                .First()
                .AverageRating
                .Should()
                .BeGreaterThanOrEqualTo(paginatedResult.Items.Last().AverageRating);

            paginatedResult.Items.Should().HaveCount(2);
            paginatedResult.PageIndex.Should().Be(page);
            paginatedResult.PageSize.Should().Be(pageSize);
        }

        [Fact]
        public async Task DetailsAsync_ShouldReturn_BookDetailsServiceModel_WhenBookExists()
        {
            var bookId = 1;

            var result = await this.bookService.DetailsAsync(bookId);

            result.Should().NotBeNull();
            result!.Id.Should().Be(bookId);
            result!.Title.Should().Be("Pet Sematary");
        }

        [Fact]
        public async Task DetailsAsync_ShouldReturn_Null_WhenBookExistsAndIsNotApproved()
        {
            var bookId = 1;

            var book = await this.data.Books.FindAsync(bookId);
            book!.IsApproved = false;
            await this.data.SaveChangesAsync();

            var result = await this.bookService.DetailsAsync(bookId);

            result.Should().BeNull();

            book!.IsApproved = true;
            await this.data.SaveChangesAsync();
        }

        [Fact]
        public async Task DetailsAsync_ShouldReturn_Null_WhenBookDoesNotExist()
        {
            var bookId = 999;

            var result = await this.bookService.DetailsAsync(bookId);

            result.Should().BeNull();
        }

        [Fact]
        public async Task AdminDetailsAsync_ShouldReturn_BookDetailsServiceModel_WhenBookExists()
        {
            var bookId = 1;

            var result = await this.bookService.AdminDetailsAsync(bookId);

            result.Should().NotBeNull();
            result!.Id.Should().Be(bookId);
            result!.Title.Should().Be("Pet Sematary");
        }

        [Fact]
        public async Task AdminDetailsAsync_ShouldReturn_BookDetailsServiceModel_WhenBookExists_AndItIsNotApproved()
        {
            var bookId = 1;

            var book = await this.data.Books.FindAsync(bookId);
            book!.IsApproved = false;
            await this.data.SaveChangesAsync();

            var result = await this.bookService.AdminDetailsAsync(bookId);

            result.Should().NotBeNull();
            result!.Id.Should().Be(bookId);
            result!.Title.Should().Be("Pet Sematary");

            book!.IsApproved = true;
            await this.data.SaveChangesAsync();
        }

        [Fact]
        public async Task AdminDetailsAsync_ShouldReturn_Null_WhenBookDoesNotExist()
        {
            var bookId = 999;

            var result = await this.bookService.AdminDetailsAsync(bookId);

            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnBookId_AndSetIsApprovedToFalse_IfUserIsRegularUser()
        {
            this.SetIsAdmin(false);

            var model = new CreateBookServiceModel()
            {
                Title = "The Dead Zone",
                ShortDescription = "Dead Zone short description.",
                LongDescription =
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description," +
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description, " +
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description, " +
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/24/Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg/330px-Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg",
            };

            var bookId = await this.bookService.CreateAsync(model);

            var book = await this.data.Books.FindAsync(bookId);
            book.Should().NotBeNull();
            book!.IsApproved.Should().BeFalse();

            this.data.Remove(book);
            await this.data.SaveChangesAsync();
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnBookId_AndSetIsApprovedToFalse_IfUserIsAdmin()
        {
            var model = new CreateBookServiceModel()
            {
                Title = "The Dead Zone",
                ShortDescription = "Dead Zone short description.",
                LongDescription =
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description," +
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description, " +
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description, " +
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/24/Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg/330px-Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg",
            };


            var bookId = await this.bookService.CreateAsync(model);

            var book = await this.data.Books.FindAsync(bookId);
            book.Should().NotBeNull();
            book!.IsApproved.Should().BeTrue();

            this.data.Remove(book);
            await this.data.SaveChangesAsync();
        }

        [Fact]
        public async Task EditAsync_ShouldReturn_Success_WhenBookIsEditedSuccessfully()
        {
            var createModel = new CreateBookServiceModel()
            {
                Title = "The Dead Zone",
                ShortDescription = "Dead Zone short description.",
                LongDescription =
                    "Dead Zone long description,Dead Zone long description,Dead Zone long description," +
                    "Dead Zone long description,Dead Zone long description,Dead Zone long description, " +
                    "Dead Zone long description,Dead Zone long description,Dead Zone long description, " +
                    "Dead Zone long description,Dead Zone long description,Dead Zone long description",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/24/Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg/330px-Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg",
            };

            var bookId = await this.bookService.CreateAsync(createModel);

            var editModel = new CreateBookServiceModel()
            {
                AuthorId = 1,
                Title = "Edited",
                ShortDescription = "Dead Zone short description Edited.",
                LongDescription =
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description," +
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description, " +
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description, " +
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/24/Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg/330px-Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg",
            };

            var result = await this.bookService.EditAsync(bookId, editModel);

            result.Succeeded.Should().BeTrue();
            result.ErrorMessage.Should().Be(null);

            var updatedBook = await this.data.Books.FindAsync(bookId);
            updatedBook.Should().NotBeNull();
            updatedBook!.Title.Should().Be("Edited");
            updatedBook!.ShortDescription.Should().Be("Dead Zone short description Edited.");
            updatedBook!.AuthorId.Should().Be(1);
        }

        [Fact]
        public async Task EditAsync_ShouldReturn_Error_WhenBookNotFound()
        {
            var editModel = new CreateBookServiceModel()
            {
                AuthorId = 1,
                Title = "Edited",
                ShortDescription = "Dead Zone short description Edited.",
                LongDescription =
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description," +
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description, " +
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description, " +
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/24/Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg/330px-Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg",
            };
            var invalidId = 124_892_157;
            var result = await this.bookService.EditAsync(invalidId, editModel);

            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Be(string.Format(DbEntityNotFound, nameof(Book), invalidId));
        }

        [Fact]
        public async Task EditAsync_ShouldReturn_Error_WhenUserIsNotTheCreator()
        {
            var createModel = new CreateBookServiceModel()
            {
                Title = "The Dead Zone",
                ShortDescription = "Dead Zone short description.",
                LongDescription =
                    "Dead Zone long description,Dead Zone long description,Dead Zone long description," +
                    "Dead Zone long description,Dead Zone long description,Dead Zone long description, " +
                    "Dead Zone long description,Dead Zone long description,Dead Zone long description, " +
                    "Dead Zone long description,Dead Zone long description,Dead Zone long description",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/24/Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg/330px-Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg",
            };

            var bookId = await this.bookService.CreateAsync(createModel);

            var book = await this.data.Books.FindAsync(bookId);
            book!.CreatorId = "fake-creator-id";
            await this.data.SaveChangesAsync();

            var editModel = new CreateBookServiceModel()
            {
                AuthorId = 1,
                Title = "Edited",
                ShortDescription = "Dead Zone short description Edited.",
                LongDescription =
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description," +
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description, " +
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description, " +
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/24/Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg/330px-Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg",
            };

            var result = await this.bookService.EditAsync(bookId, editModel);

            result.Succeeded.Should().BeFalse();
            result
                .ErrorMessage
                .Should()
                .Be(string.Format(
                    UnauthorizedDbEntityAction,
                    "current-user-username",
                    nameof(Book),
                    bookId));
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturn_Success_WhenBookIsDeletedSuccessfully()
        {
            var createModel = new CreateBookServiceModel()
            {
                Title = "The Dead Zone",
                ShortDescription = "Dead Zone short description.",
                LongDescription =
                    "Dead Zone long description,Dead Zone long description,Dead Zone long description," +
                    "Dead Zone long description,Dead Zone long description,Dead Zone long description, " +
                    "Dead Zone long description,Dead Zone long description,Dead Zone long description, " +
                    "Dead Zone long description,Dead Zone long description,Dead Zone long description",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/24/Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg/330px-Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg",
            };

            var bookId = await this.bookService.CreateAsync(createModel);
            var result = await this.bookService.DeleteAsync(bookId);

            result.Succeeded.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();

            var bookIgnoredFilter = await this.data
                .Books
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(b => b.Id == bookId);

            bookIgnoredFilter!.IsDeleted.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturn_Error_WhenBookNotFound()
        {
            var invalidId = 213_422_580;
            var result = await this.bookService.DeleteAsync(invalidId);

            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Be(string.Format(DbEntityNotFound, nameof(Book), invalidId));
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturn_Error_WhenUserIsNotCreatorOrAdmin()
        {
            this.SetIsAdmin(false);

            var createModel = new CreateBookServiceModel()
            {
                Title = "The Dead Zone",
                ShortDescription = "Dead Zone short description.",
                LongDescription =
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description," +
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description, " +
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description, " +
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/24/Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg/330px-Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg",
            };

            var bookId = await this.bookService.CreateAsync(createModel);
            var book = await this.data.Books.FindAsync(bookId);
            book!.CreatorId = "fake-creator-id";
            await this.data.SaveChangesAsync();

            var result = await this.bookService.DeleteAsync(bookId);

            result.Succeeded.Should().BeFalse();
            result
                .ErrorMessage
                .Should()
                .Be(string.Format(
                    UnauthorizedDbEntityAction,
                    "current-user-username",
                    nameof(Book),
                    book.Id));
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturn_Error_WhenUserIsNotCreatorButIsAdmin()
        {
            this.SetIsAdmin(true);

            var createModel = new CreateBookServiceModel()
            {
                Title = "The Dead Zone",
                ShortDescription = "Dead Zone short description.",
                LongDescription =
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description," +
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description, " +
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description, " +
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/24/Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg/330px-Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg",
            };

            var bookId = await this.bookService.CreateAsync(createModel);
            var book = await this.data.Books.FindAsync(bookId);
            book!.CreatorId = "fake-creator-id";
            await this.data.SaveChangesAsync();

            var result = await this.bookService.DeleteAsync(bookId);

            result.Succeeded.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();
        }

        [Fact]
        public async Task ApproveAsync_ShouldApproveBook()
        {
            this.SetIsAdmin(false);

            var createModel = new CreateBookServiceModel()
            {
                Title = "The Dead Zone",
                ShortDescription = "Dead Zone short description.",
                LongDescription =
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description," +
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description, " +
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description, " +
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/24/Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg/330px-Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg",
            };

            var bookId = await this.bookService.CreateAsync(createModel);

            this.SetIsAdmin(true);

            var result = await this.bookService.ApproveAsync(bookId);

            result.Succeeded.Should().BeTrue();

            var approvedBook = await this.data.Books.FindAsync(bookId);

            approvedBook.Should().NotBeNull();
            approvedBook!.IsApproved.Should().BeTrue();
        }

        [Fact]
        public async Task ApproveAsync_ShouldReturnError_IfBookNotFound()
        {
            this.SetIsAdmin(false);

            var createModel = new CreateBookServiceModel()
            {
                Title = "The Dead Zone",
                ShortDescription = "Dead Zone short description.",
                LongDescription =
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description," +
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description, " +
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description, " +
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/24/Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg/330px-Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg",
            };

            var bookId = await this.bookService.CreateAsync(createModel);

            this.SetIsAdmin(true);

            var invalidId = 211_244_858;
            var result = await this.bookService.ApproveAsync(invalidId);

            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Be(string.Format(DbEntityNotFound, nameof(Book), invalidId));

            var book = await this.data.Books.FindAsync(bookId);
            book!.IsApproved.Should().BeFalse();
        }

        [Fact]
        public async Task RejectAsync_ShouldDeleteTheBook()
        {
            this.SetIsAdmin(false);

            var createModel = new CreateBookServiceModel()
            {
                Title = "The Dead Zone",
                ShortDescription = "Dead Zone short description.",
                LongDescription =
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description," +
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description, " +
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description, " +
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/24/Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg/330px-Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg",
            };

            var bookId = await this.bookService.CreateAsync(createModel);

            this.SetIsAdmin(true);

            var result = await this.bookService.RejectAsync(bookId);

            result.Succeeded.Should().BeTrue();

            var rejectedBook = await this.data
                .Books
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(b => b.Id == bookId);

            rejectedBook!.IsDeleted.Should().BeTrue();
        }

        [Fact]
        public async Task RejectAsync_ShouldReturnError_IfBookNotFound()
        {
            this.SetIsAdmin(false);

            var createModel = new CreateBookServiceModel()
            {
                Title = "The Dead Zone",
                ShortDescription = "Dead Zone short description.",
                LongDescription =
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description," +
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description, " +
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description, " +
                   "Dead Zone long description,Dead Zone long description,Dead Zone long description",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/24/Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg/330px-Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg",
            };

            var bookId = await this.bookService.CreateAsync(createModel);

            this.SetIsAdmin(true);

            var invalidId = 211_244_858;
            var result = await this.bookService.RejectAsync(invalidId);

            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Be(string.Format(DbEntityNotFound, nameof(Book), invalidId));

            var book = await this.data.Books.FindAsync(bookId);
            book!.IsApproved.Should().BeFalse();
        }

        private void SetIsAdmin(bool isAdmin)
            => this.mockUserService
                .Setup(x => x.IsAdmin())
                .Returns(isAdmin);

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

            if (!await this.data.Books.AnyAsync())
            {
                this.data.AddRange(books);
                await this.data.SaveChangesAsync();
            }

            var genres = new Genre[]
            {
                new()
                {
                    Id = 1,
                    Name = "Horror",
                    Description =
                        "Horror fiction is designed to scare, unsettle, or horrify readers. It explores themes of fear and the unknown, " +
                        "often incorporating supernatural elements like ghosts, monsters, or curses. The genre can also delve into the darker aspects " +
                        "of human psychology, portraying paranoia, obsession, and moral corruption. Subgenres include Gothic horror, psychological horror, " +
                        "and splatterpunk, each offering unique ways to evoke dread. Settings often amplify the tension, ranging from haunted houses to " +
                        "desolate landscapes, while the stories frequently address societal fears and existential questions.",
                    ImageUrl = "https://org-dcmp-staticassets.s3.us-east-1.amazonaws.com/posterimages/13453_1.jpg"
                },
                new()
                {
                    Id = 2,
                    Name = "Science Fiction",
                    Description =
                        "Science fiction explores futuristic, scientific, and technological themes, challenging readers to consider the possibilities and " +
                        "consequences of innovation. These stories often involve space exploration, artificial intelligence, time travel, or parallel " +
                        "universes. Beyond the speculative elements, science fiction frequently tackles ethical dilemmas, societal transformations, and " +
                        "the human condition. Subgenres include cyberpunk, space opera, and hard science fiction, each offering distinct visions of the future. " +
                        "The genre invites readers to imagine the impact of progress and to ponder humanity’s place in the cosmos.",
                    ImageUrl = "https://www.editoreric.com/greatlit/litgraphics/book-spiral-galaxy.jpg"
                },
                new()
                {
                    Id = 3,
                    Name = "Fantasy",
                    Description =
                        "Fantasy stories transport readers to magical realms filled with mythical creatures, enchanted objects, and epic quests. These tales " +
                        "often feature battles between good and evil, drawing upon folklore, mythology, and the human imagination. Characters may wield powerful " +
                        "magic or undertake journeys of self-discovery in richly crafted worlds. Subgenres like high fantasy, urban fantasy, and dark fantasy " +
                        "provide diverse settings and tones, appealing to a wide range of readers. Themes of heroism, destiny, and transformation are central to " +
                        "the genre, offering both escape and inspiration.",
                    ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT5EcrB6fhai5L3-7Ted6fZgxUjCti0W4avrA&s"
                },
                 new()
                {
                    Id = 4,
                    Name = "Mystery",
                    Description =
                        "Mystery fiction is a puzzle-driven genre that engages readers with suspense and intrigue. The narrative typically revolves around solving " +
                        "a crime, uncovering hidden truths, or exposing a web of deceit. Protagonists range from amateur sleuths to seasoned detectives, each " +
                        "navigating clues, red herrings, and unexpected twists. Subgenres such as noir, cozy mysteries, and legal thrillers cater to varied tastes. " +
                        "Mystery stories often delve into human motives and societal dynamics, providing a satisfying journey toward uncovering the truth.",
                    ImageUrl = "https://celadonbooks.com/wp-content/uploads/2020/03/what-is-a-mystery.jpg"
                },
                new()
                {
                    Id = 5,
                    Name = "Romance",
                    Description =
                        "Romance novels celebrate the complexities of love and relationships, weaving stories of passion, connection, and emotional growth. " +
                        "They can be set in diverse contexts, from historical periods to fantastical worlds, and often feature characters overcoming personal or " +
                        "external obstacles to find happiness. Subgenres like contemporary romance, historical romance, and paranormal romance offer unique flavors " +
                        "and settings. The genre emphasizes emotional resonance, with narratives that inspire hope and affirm the power of love.",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/3/36/Hammond-SS10.jpg"
                },
                new()
                {
                    Id = 6,
                    Name = "Thriller",
                    Description =
                        "Thrillers are characterized by their fast-paced, high-stakes plots designed to keep readers on edge. They often involve life-and-death " +
                        "scenarios, sinister conspiracies, or relentless antagonists. The genre thrives on tension and unexpected twists, with protagonists racing " +
                        "against time to prevent disaster. Subgenres like psychological thrillers, spy thrillers, and action thrillers cater to diverse interests. " +
                        "The stories explore themes of survival, justice, and moral ambiguity, delivering an adrenaline-fueled reading experience.",
                    ImageUrl = "https://celadonbooks.com/wp-content/uploads/2019/10/what-is-a-thriller-1024x768.jpg"
                },
                new()
                {
                    Id = 7,
                    Name = "Adventure",
                    Description =
                        "Adventure stories are dynamic tales of action, exploration, and survival. Protagonists often face daunting challenges, traversing " +
                        "uncharted territories or overcoming perilous odds. The genre celebrates courage, resilience, and the human spirit, taking readers on " +
                        "exhilarating journeys. From treasure hunts to epic battles, adventure fiction encompasses diverse settings and narratives. It appeals to " +
                        "those who crave excitement and the thrill of discovery.",
                    ImageUrl = "https://thumbs.dreamstime.com/b/open-book-ship-sailing-waves-concept-reading-adventure-literature-generative-ai-270347849.jpg"
                },
                new()
                {
                    Id = 8,
                    Name = "Historical fiction",
                    Description =
                        "Historical fiction immerses readers in the past, blending factual events with fictional narratives to create vivid portrayals of bygone eras. " +
                        "These stories illuminate the lives, struggles, and triumphs of people from different times, providing insight into cultural, social, and " +
                        "political contexts. Subgenres include historical romance, historical mysteries, and alternate histories, each offering unique perspectives. " +
                        "The genre enriches our understanding of history while engaging us with compelling characters and plots.",
                    ImageUrl = "https://celadonbooks.com/wp-content/uploads/2020/03/Historical-Fiction-scaled.jpg"
                },
                new()
                {
                    Id = 9,
                    Name = "Biography",
                    Description =
                        "Biographies chronicle the lives of real individuals, offering intimate portraits of their experiences, achievements, and legacies. These works " +
                        "range from comprehensive life stories to focused accounts of specific events or periods. Biographies can inspire, inform, and provide deep " +
                        "insight into historical or contemporary figures. Autobiographies and memoirs, subgenres of biography, allow subjects to share their own " +
                        "narratives, adding personal depth to the genre.",
                    ImageUrl = "https://i0.wp.com/uspeakgreek.com/wp-content/uploads/2024/01/biography.webp?fit=780%2C780&ssl=1"
                },
                new()
                {
                    Id = 10,
                    Name = "Self-help",
                    Description =
                        "Self-help books are guides to personal growth, offering practical advice for improving one’s life. Topics range from mental health and " +
                        "relationships to productivity and spiritual fulfillment. The genre emphasizes empowerment, providing readers with strategies and tools for " +
                        "achieving goals and overcoming challenges. Subgenres include motivational literature, mindfulness guides, and career development books, " +
                        "catering to diverse needs and aspirations.",
                    ImageUrl = "https://www.wellnessroadpsychology.com/wp-content/uploads/2024/05/Self-Help.jpg"
                },
                new()
                {
                    Id = 11,
                    Name = "Non-fiction",
                    Description =
                        "Non-fiction encompasses works rooted in factual information, offering insights into real-world topics. It spans memoirs, investigative journalism, " +
                        "essays, and academic studies, covering subjects like history, science, culture, and politics. The genre educates and engages readers, often " +
                        "challenging perceptions and broadening understanding. Non-fiction can be narrative-driven or expository, appealing to those seeking knowledge " +
                        "or a deeper connection to reality.",
                    ImageUrl = "https://pickbestbook.com/wp-content/uploads/2023/06/Nonfiction-Literature-1.png"
                },
                new()
                {
                    Id = 12,
                    Name = "Poetry",
                    Description =
                        "Poetry is a literary form that condenses emotions, thoughts, and imagery into carefully chosen words, often structured with rhythm and meter. " +
                        "It explores universal themes such as love, nature, grief, and introspection, offering readers profound and evocative experiences. " +
                        "From traditional sonnets and haikus to free verse and spoken word, poetry captivates through its ability to articulate the inexpressible, " +
                        "creating deep emotional resonance and intellectual reflection.",
                    ImageUrl = "https://assets.ltkcontent.com/images/9037/examples-of-poetry-genres_7abbbb2796.jpg"
                },
                new()
                {
                    Id = 13,
                    Name = "Drama",
                    Description =
                        "Drama fiction delves into emotional and relational conflicts, portraying the complexities of human interactions and emotions. " +
                        "It emphasizes character development and nuanced storytelling, often exploring themes of love, betrayal, identity, and societal struggles. " +
                        "Drama offers readers a lens into the intricacies of the human experience, whether through tragic, romantic, or morally ambiguous narratives. " +
                        "Its focus on realism and emotional depth creates stories that resonate deeply with audiences.",
                    ImageUrl = "https://basudewacademichub.in/wp-content/uploads/2024/02/drama-literature-solution.png"
                },
                new()
                {
                    Id = 14,
                    Name = "Children's",
                    Description =
                        "Children's literature is crafted to captivate and inspire young readers with imaginative worlds, moral lessons, and relatable characters. " +
                        "These stories often emphasize themes of curiosity, friendship, and bravery, delivering messages of kindness, resilience, and growth. " +
                        "From whimsical picture books to adventurous chapter books, children's fiction nurtures creativity and fosters a lifelong love of reading, " +
                        "helping young minds explore both real and fantastical realms.",
                    ImageUrl = "https://media.vanityfair.com/photos/598888671dc63c45b7b1db6e/master/w_2560%2Cc_limit/MAG-0817-Wild-Things-a.jpg"
                },
                new()
                {
                    Id = 15,
                    Name = "Young Adult",
                    Description =
                        "Young Adult (YA) fiction speaks to the unique experiences and challenges of adolescence, addressing themes such as identity, first love, " +
                        "friendship, and coming of age. These stories often feature relatable protagonists navigating personal growth, societal expectations, and " +
                        "emotional upheaval. Subgenres such as fantasy, dystopian, and contemporary YA provide diverse backdrops for these journeys, resonating with " +
                        "readers through authentic and engaging storytelling that reflects their own struggles and triumphs.",
                    ImageUrl = "https://m.media-amazon.com/images/I/81xRLF1KCAL._AC_UF1000,1000_QL80_.jpg"
                },
                new()
                {
                    Id = 16,
                    Name = "Comedy",
                    Description =
                        "Comedy fiction aims to entertain and delight readers through humor, satire, and absurdity. It uses wit and clever storytelling to highlight " +
                        "human follies, societal quirks, or surreal situations. From lighthearted escapades to biting social commentary, comedy encompasses a range " +
                        "of tones and styles. The genre often brings laughter and joy, offering an escape from the mundane while sometimes delivering thought-provoking " +
                        "messages in the guise of humor.",
                    ImageUrl = "https://mandyevebarnett.com/wp-content/uploads/2017/12/humor.jpg?w=640"
                },
                new()
                {
                    Id = 17,
                    Name = "Graphic Novel",
                    Description =
                        "Graphic novels seamlessly blend visual art and narrative storytelling, using a combination of text and illustrations to convey complex plots and emotions. " +
                        "This versatile format spans a wide array of genres, including superhero tales, memoirs, historical epics, and science fiction. Graphic novels " +
                        "offer an immersive reading experience, appealing to diverse audiences through their ability to convey vivid imagery and intricate storylines " +
                        "that are as impactful as traditional prose.",
                    ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSb0THovTlPB_nRl3RY6TsbWD4R2qEC-TQSAg&s"
                },
                new()
                {
                    Id = 18,
                    Name = "Other",
                    Description =
                        "The 'Other' genre serves as a home for unconventional, experimental, or cross-genre works that defy traditional categorization. " +
                        "This category embraces innovation and diversity, welcoming stories that push the boundaries of storytelling, structure, and style. " +
                        "From hybrid narratives to avant-garde experiments, 'Other' offers a platform for unique voices and creative expressions " +
                        "that don’t fit neatly into predefined genres.",
                    ImageUrl = "https://www.98thpercentile.com/hubfs/388x203%20(4).png"
                },
                new()
                {
                    Id = 19,
                    Name = "Dystopian",
                    Description =
                        "Dystopian fiction paints a grim portrait of societies marred by oppression, inequality, or disaster, often set in a future shaped by " +
                        "catastrophic events or authoritarian regimes. These cautionary tales explore themes like survival, rebellion, and the loss of humanity, " +
                        "serving as critiques of political, social, and environmental trends. Subgenres such as post-apocalyptic and cyber-dystopia examine " +
                        "the fragility of civilization and the consequences of unchecked power or technological overreach.",
                    ImageUrl = "https://www.ideology-theory-practice.org/uploads/1/3/5/5/135563566/050_orig.jpg"
                },
                new()
                {
                    Id = 20,
                    Name = "Spirituality",
                    Description =
                        "Spirituality books delve into the deeper questions of existence, faith, and the human soul, offering insights and practices to " +
                        "nurture inner peace and personal growth. They often explore themes of mindfulness, self-awareness, and connection to a higher " +
                        "power or universal energy. From philosophical reflections to practical guides, these works resonate with readers seeking" +
                        "inspiration, understanding, and spiritual fulfillment across diverse traditions and belief systems.",
                    ImageUrl = "https://m.media-amazon.com/images/I/61jxcM3UskL._AC_UF1000,1000_QL80_.jpg"
                },
                new()
                {
                    Id = 21,
                    Name = "Crime",
                    Description =
                        "Crime fiction is centered around the investigation of a crime, often focusing on the detection of criminals or the pursuit of " +
                        "justice. It may include detectives, police officers, or amateur sleuths solving crimes like murder, theft, or corruption. " +
                        "The genre can involve suspense, action, and exploration of moral dilemmas surrounding law and order. Subgenres include hardboiled" +
                        "crime, cozy mysteries, and police procedurals, all providing different approaches to solving crimes and investigating human behavior.",
                    ImageUrl = "https://img.tpt.cloud/nextavenue/uploads/2019/04/Crime-Fiction-Savvy-Sleuths-Over-50_53473532.inside.1200x775.jpg"
                },

                new()
                {
                    Id = 22,
                    Name = "Urban Fiction",
                    Description =
                        "Urban fiction explores life in modern, often gritty urban settings, focusing on the struggles, relationships, and experiences of " +
                        "people in cities. This genre frequently addresses themes like poverty, crime, social injustice, and community dynamics. " +
                        "It can incorporate elements of drama, romance, and even horror, often portraying the challenges of urban life with raw, " +
                        "unflinching realism. Urban fiction is popular in contemporary literature and often includes characters from marginalized communities.",
                    ImageUrl = "https://frugalbookstore.net/cdn/shop/collections/Urban-Fiction.png?v=1724599745&width=480"
                },

                new()
                {
                    Id = 23,
                    Name = "Fairy Tale",
                    Description =
                        "Fairy tale fiction involves magical or fantastical stories often set in a world where magic and mythical creatures exist. " +
                        "These stories typically follow a clear moral arc, with characters who experience trials or transformation before achieving a happy " +
                        "ending. Fairy tales often feature archetypal characters like witches, princes, and princesses, and they explore themes of " +
                        "good vs. evil, justice, and personal growth. Many fairy tales have been passed down through generations, and the genre continues to " +
                        "inspire modern adaptations and retellings.",
                    ImageUrl = "https://news.syr.edu/wp-content/uploads/2023/09/enchanting_fairy_tale_woodland_onto_a_castle_an.original-scaled.jpg"
                },

                new()
                {
                    Id = 24,
                    Name = "Epic",
                    Description =
                        "Epic fiction is characterized by large-scale, grand narratives often centered around heroic characters or monumental events. " +
                        "Epics typically focus on the struggles and triumphs of protagonists who undergo significant personal or societal change. " +
                        "These stories often span extensive periods of time and encompass entire civilizations, exploring themes like war, leadership, and cultural " +
                        "identity. Classic examples include *The Iliad* and *The Odyssey*, with modern epics continuing to explore the human experience in vast, " +
                        "sweeping terms.",
                    ImageUrl = "https://i0.wp.com/joncronshaw.com/wp-content/uploads/2024/01/DALL%C2%B7E-2024-01-17-09.05.10-A-magical-and-enchanting-landscape-for-a-fantasy-blog-post-featuring-an-ancient-castle-perched-on-a-high-cliff-a-vast-mystical-forest-with-towering.png?fit=1200%2C686&ssl=1"
                },

                new()
                {
                    Id = 25,
                    Name = "Political Fiction",
                    Description =
                        "Political fiction uses stories to explore, criticize, or comment on political systems, ideologies, and power dynamics. These narratives " +
                        "often examine how political structures affect individuals and societies, focusing on themes of corruption, revolution, and social change. " +
                        "Political fiction can include dystopian novels, satires, and thrillers, offering commentary on both contemporary and historical politics. " +
                        "Through these stories, authors challenge readers to think critically about the systems that govern their lives.",
                    ImageUrl = "https://markelayat.com/wp-content/uploads/elementor/thumbs/Political-Fiction-ft-image-qwo9yzatn5xk8t34vvqfivz2ed7zuj5lccn9ylm7bc.png"
                },

                new()
                {
                    Id = 26,
                    Name = "Philosophical Fiction",
                    Description =
                        "Philosophical fiction delves into profound questions about existence, ethics, free will, and the nature of reality. These novels often " +
                        "explore abstract ideas and are driven by deep intellectual themes rather than plot or action. Philosophical fiction may follow characters " +
                        "who engage in critical thinking, self-reflection, or existential crises. These works often question the meaning of life, morality, " +
                        "and consciousness, and they can be a blend of both fiction and philosophy, prompting readers to consider their own beliefs and perspectives.",
                    ImageUrl = "https://i.gr-assets.com/images/S/compressed.photo.goodreads.com/books/1546103428i/5297._UX160_.jpg"
                },
                new()
                {
                    Id = 27,
                    Name = "True Crime",
                    Description =
                        "True crime fiction is based on real-life criminal events, recounting the details of notorious crimes, investigations, and trials. " +
                        "It often focuses on infamous cases, delving into the psychology of criminals, the detectives or journalists who solve the cases, and the " +
                        "social impact of the crime. True crime often incorporates extensive research and interviews, giving readers an inside look at the " +
                        "complexities of real-life crime and law enforcement. These works can be chilling and thought-provoking, blending elements of mystery, drama," +
                        "and historical non-fiction.",
                    ImageUrl = "https://is1-ssl.mzstatic.com/image/thumb/Podcasts221/v4/00/07/67/000767b5-bad1-5d78-db34-373363ec6b3e/mza_8962416523973028402.jpg/1200x1200bf.webp"
                },
                new()
                {
                    Id = 28,
                    Name = "Satire",
                    Description =
                        "Satire is a genre that uses humor, irony, and exaggeration to criticize or mock individuals, institutions, or societal norms. " +
                        "It often employs wit and sarcasm to highlight the flaws and absurdities of the subject being criticized, sometimes with the intent of " +
                        "provoking thought or promoting change. Satirical works can cover a wide range of topics, including politics, culture, and human nature, " +
                        "and can be both lighthearted or dark in tone. Famous examples include works like Gulliver's Travels and Catch-22.",
                    ImageUrl = "https://photos.demandstudios.com/getty/article/64/32/529801877.jpg"
                },
                new()
                {
                    Id = 29,
                    Name = "Psychological Fiction",
                    Description =
                        "Psychological fiction delves into the inner workings of the mind, exploring complex emotional states, mental illness, " +
                        "and the psychological effects of personal trauma, relationships, and societal pressures. These works often focus on character " +
                        "development and the emotional or mental struggles of the protagonists, rather than external events. Psychological fiction can blur the lines " +
                        "between reality and illusion, questioning perceptions and exploring the deeper layers of human consciousness. It often presents " +
                        "challenging and sometimes disturbing narratives about identity and self-perception. Notable examples include The Bell Jar and The Catcher in the Rye.",
                    ImageUrl = "https://literaturelegends.com/wp-content/uploads/2023/08/psychological.jpg"
                },
                new()
                {
                    Id = 30,
                    Name = "Supernatural",
                    Description =
                        "Supernatural fiction explores phenomena beyond the natural world, often incorporating ghosts, spirits, vampires, or otherworldly beings. " +
                        "These works blend elements of horror, fantasy, and the unexplained, and often delve into themes of life after death, paranormal activity, and other mystifying occurrences. " +
                        "The supernatural genre captivates readers with its portrayal of eerie events and the unknown, often blurring the line between reality and the mystical. Examples include works like The Haunting of Hill House and The Turn of the Screw.",
                    ImageUrl = "https://fully-booked.ca/wp-content/uploads/2024/02/evolution-of-paranormal-fiction-1024x576.jpg"
                },
                new()
                {
                    Id = 31,
                    Name = "Gothic Fiction",
                    Description =
                        "Gothic fiction is characterized by its dark, eerie atmosphere, and often involves elements of horror, mystery, and the supernatural. " +
                        "These stories typically feature gloomy, decaying settings such as castles, mansions, or haunted landscapes, and often include tragic or macabre themes. " +
                        "Gothic fiction focuses on emotions like fear, dread, and despair, and explores the darker sides of human nature. Famous examples include works like Wuthering Heights and Frankenstein.",
                    ImageUrl = "https://bookstr.com/wp-content/uploads/2022/09/V8mj92.webp"
                },
                new()
                {
                    Id = 32,
                    Name = "Magical Realism",
                    Description =
                        "Magical realism blends elements of magic or the supernatural with a realistic narrative, creating a world where extraordinary events occur within ordinary settings. " +
                        "This genre often explores themes of identity, culture, and human experience, and it is marked by the seamless integration of magical elements into everyday life. " +
                        "Prominent examples include books like One Hundred Years of Solitude and The House of the Spirits.",
                    ImageUrl = "https://www.world-defined.com/wp-content/uploads/2024/04/Magic-Realism-Books-978x652-1.webp"
                },
                new()
                {
                    Id = 33,
                    Name = "Dark Fantasy",
                    Description =
                        "Dark fantasy combines elements of fantasy with a sense of horror, despair, and the supernatural. " +
                        "These stories often take place in dark, gritty worlds where magic, danger, and moral ambiguity challenge the characters. " +
                        "Dark fantasy blends the fantastical with the disturbing, creating a sense of dread and unease. Examples include books like The Dark Tower series and A Song of Ice and Fire.",
                    ImageUrl = "https://miro.medium.com/v2/resize:fit:1024/1*VU5O34UlH-1SXZkEnL0dyg.jpeg"
                }
            };

            if (!await this.data.Genres.AnyAsync())
            {
                this.data.AddRange(genres);
                await this.data.SaveChangesAsync();
            }

            var bookGenres = new BookGenre[]
            {
                //pet sematary
                new() { BookId = 1, GenreId = 1 }, //horror
                new() { BookId = 1, GenreId = 4 }, //mystery
                new() { BookId = 1, GenreId = 6 }, //thriller
                new() { BookId = 1, GenreId = 30 }, //supernatural
                //harry potter and the deathly hallows
                new() { BookId = 2, GenreId = 3 }, //fantasy
                new() { BookId = 2, GenreId = 7 }, //adventure
                new() { BookId = 2, GenreId = 14 }, //children
                new() { BookId = 2, GenreId = 15 }, //young adult
                new() { BookId = 2, GenreId = 32 }, //magical realism
                //lord of the rings
                new() { BookId = 3, GenreId = 3 }, //fantasy
                new() { BookId = 3, GenreId = 7 }, //adventure
                new() { BookId = 3, GenreId = 24 }, //epic
                //1984
                new() { BookId = 4, GenreId = 19 }, //dystopian
                new() { BookId = 4, GenreId = 25 }, //Political fiction
                new() { BookId = 4, GenreId = 28 }, //satire
                new() { BookId = 4, GenreId = 29 }, //psychological fiction
                //one flew over a cockoo's nest
                new() { BookId = 5, GenreId = 13 }, //drama
                new() { BookId = 5, GenreId = 19 }, //dystopian
                new() { BookId = 5, GenreId = 26 }, //Philosophical fiction
                new() { BookId = 5, GenreId = 28 }, //satire
                new() { BookId = 5, GenreId = 29 }, //psychological fiction
            };

            if (!await this.data.BooksGenres.AnyAsync())
            {
                this.data.AddRange(bookGenres);
                await this.data.SaveChangesAsync();
            }

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
                        "have been adapted into feature films, miniseries, television series, and comic books. Notable works include Carrie, The Shining, " +
                        "IT, Misery, Pet Sematary, and The Dark Tower series. King is renowned for his ability to create compelling, complex characters " +
                        "and for his mastery of building suspenseful, intricately woven narratives. Aside from his novels, he has written nearly " +
                        "200 short stories, most of which have been compiled into collections such as Night Shift, Skeleton Crew, and " +
                        "Everything's Eventual. He has received numerous awards for his contributions to literature, including the " +
                        "National Book Foundation's Medal for Distinguished Contribution to American Letters in 2003. King often writes under " +
                        "the pen name 'Richard Bachman,' a pseudonym he used to publish early works such as Rage and The Long Walk. He lives in " +
                        "Bangor, Maine, with his wife, fellow novelist Tabitha King, and continues to write and inspire new generations of " +
                        "readers and writers.",
                    PenName = "Richard Bachman",
                    AverageRating = 4.7,
                    NationalityId = 182,
                    Gender = Gender.Male,
                    BornAt = new DateTime(1947, 09, 21),
                    IsApproved = true
                 },
            };

            if (!await this.data.Authors.AnyAsync())
            {
                this.data.AddRange(authors);
            }
        }
    }
}
