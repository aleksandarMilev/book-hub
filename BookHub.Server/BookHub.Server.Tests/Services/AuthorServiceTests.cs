namespace BookHub.Server.Tests.Services
{
    using Areas.Admin.Service;
    using AutoMapper;
    using Data;
    using Features.Authors.Data.Models;
    using Features.Authors.Mapper;
    using Features.Authors.Service;
    using Features.Authors.Service.Models;
    using Features.Notification.Service;
    using Features.UserProfile.Service;
    using FluentAssertions;
    using Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    using static Common.ErrorMessage;

    public class AuthorServiceTests
    {
        private readonly BookHubDbContext data;

        private readonly Mock<ICurrentUserService> mockUserService;
        private readonly Mock<IAdminService> mockAdminService;
        private readonly Mock<INotificationService> mockNotificationService;
        private readonly Mock<IProfileService> mockProfileService;
        private readonly IMapper mapper;

        private readonly AuthorService authorService;

        public AuthorServiceTests()
        {
            var options = new DbContextOptionsBuilder<BookHubDbContext>()
                .UseInMemoryDatabase(databaseName: "AuthorServiceInMemoryDatabase")
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

            this.mockAdminService = new Mock<IAdminService>();
            this.mockNotificationService = new Mock<INotificationService>();
            this.mockProfileService = new Mock<IProfileService>();
            this.mapper = new MapperConfiguration(cfg => cfg.AddProfile(new AuthorMapper())).CreateMapper();

            this.authorService = new AuthorService(
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
        public async Task NamesAsync_ShouldReturn_AuthorNames()
        {
            var names = await this.authorService.NamesAsync();

            names.Should().NotBeEmpty();

            names.Should().Contain(a => a.Name == "Stephen King");
            names.Should().Contain(a => a.Id == 4);
        }

        [Fact]
        public async Task TopThreeAsync_ShouldReturn_TopThreeAuthors_ByAverageRating()
        {
            var topAuthors = await this.authorService.TopThreeAsync();

            topAuthors.Should().HaveCount(3);

            topAuthors.First().AverageRating.Should().Be(4.9);
            topAuthors
                .Skip(1)
                .First()
                .AverageRating
                .Should()
                .BeGreaterThanOrEqualTo(topAuthors.Skip(2).First().AverageRating);

            var expectedNames = new[] { "George Orwell", "Joanne Rowling", "Stephen King" };
            topAuthors
                .Select(a => a.Name)
                .Should()
                .BeEquivalentTo(expectedNames, options => options.WithStrictOrdering());
        }

        [Fact]
        public async Task DetailsAsync_ShouldReturnNull_WhenAuthorDoesNotExist()
        {
            var invalidId = 999_212_214; 

            var author = await this.authorService.DetailsAsync(invalidId);

            author.Should().BeNull();
        }

        [Fact]
        public async Task AdminDetailsAsync_ShouldReturnNull_WhenAuthorDoesNotExist()
        {
            var invalidId = 999_212_214;

            var author = await this.authorService.AdminDetailsAsync(invalidId);

            author.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateAuthor_AndAlso_SetIsApprovedToFalse_IfCurrentUserIsNotAdmin()
        {
            var createModel = new CreateAuthorServiceModel()
            {
                Name = "Bram Stoker",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/34/Bram_Stoker_1906.jpg/640px-Bram_Stoker_1906.jpg",
                Biography =
                   "Abraham 'Bram' Stoker (November 8, 1847 – April 20, 1912) was an Irish author best known for his Gothic horror novel Dracula" +
                   "Abraham 'Bram' Stoker (November 8, 1847 – April 20, 1912) was an Irish author best known for his Gothic horror novel Dracula,",
                PenName = "Bram Stoker",
                NationalityId = 78,
                Gender = "male"
            };

            var id = await this.authorService.CreateAsync(createModel);

            var author = await this.data.Authors.FindAsync(id);
            author!.Should().NotBeNull();
            author!.Name.Should().Be("Bram Stoker");
            author!.NationalityId.Should().Be(78);
            author!.Gender.Should().Be(Gender.Male);

            author!.IsApproved.Should().BeFalse();
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateAuthor_AndAlso_SetIsApprovedToTrue_IfCurrentUserIsAdmin()
        {
            this.SetIsAdmin(true);

            var createModel = new CreateAuthorServiceModel()
            {
                Name = "Bram Stoker",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/34/Bram_Stoker_1906.jpg/640px-Bram_Stoker_1906.jpg",
                Biography =
                   "Abraham 'Bram' Stoker (November 8, 1847 – April 20, 1912) was an Irish author best known for his Gothic horror novel Dracula" +
                   "Abraham 'Bram' Stoker (November 8, 1847 – April 20, 1912) was an Irish author best known for his Gothic horror novel Dracula,",
                PenName = "Bram Stoker",
                NationalityId = 78,
                Gender = "male"
            };

            var id = await this.authorService.CreateAsync(createModel);

            var author = await this.data.Authors.FindAsync(id);
            author!.Should().NotBeNull();
            author!.Name.Should().Be("Bram Stoker");
            author!.NationalityId.Should().Be(78);
            author!.Gender.Should().Be(Gender.Male);

            author!.IsApproved.Should().BeTrue();

            this.SetIsAdmin(false);
        }

        [Fact]
        public async Task EditAsync_ShouldEditAuthor_WhenAuthorExists_AndUserIsTheCreator()
        {
            var createModel = new CreateAuthorServiceModel()
            {
                Name = "Bram Stoker",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/34/Bram_Stoker_1906.jpg/640px-Bram_Stoker_1906.jpg",
                Biography = 
                    "Abraham 'Bram' Stoker (November 8, 1847 – April 20, 1912) was an Irish author best known for his Gothic horror novel Dracula" + 
                    "Abraham 'Bram' Stoker (November 8, 1847 – April 20, 1912) was an Irish author best known for his Gothic horror novel Dracula,",
                PenName = "Bram Stoker",
                NationalityId = 78,
                Gender = "male"
            };

            var id = await this.authorService.CreateAsync(createModel);

            var editModel = new CreateAuthorServiceModel()
            {
                Name = "Edit",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/34/Bram_Stoker_1906.jpg/640px-Bram_Stoker_1906.jpg",
                Biography = "Abraham 'Bram' Stoker (November 8, 1847 – April 20, 1912) was an Irish author best known for his Gothic horror novel Dracula,",
                PenName = null,
                NationalityId = 78,
                Gender = "female"
            };

            var result = await this.authorService.EditAsync(id, editModel);
            result.Succeeded.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();

            var author = await this.data.Authors.FindAsync(id);
            author!.Should().NotBeNull();
            author!.Name.Should().Be("Edit");
            author!.PenName.Should().BeNull();
        }

        [Fact]
        public async Task EditAsync_ShouldReturnResultWithError_WhenAuthorDoesNotExist()
        {
            var createModel = new CreateAuthorServiceModel()
            {
                Name = "Bram Stoker",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/34/Bram_Stoker_1906.jpg/640px-Bram_Stoker_1906.jpg",
                Biography =
                   "Abraham 'Bram' Stoker (November 8, 1847 – April 20, 1912) was an Irish author best known for his Gothic horror novel Dracula" +
                   "Abraham 'Bram' Stoker (November 8, 1847 – April 20, 1912) was an Irish author best known for his Gothic horror novel Dracula,",
                PenName = "Bram Stoker",
                NationalityId = 78,
                Gender = "male"
            };

            _ = await this.authorService.CreateAsync(createModel);

            var editModel = new CreateAuthorServiceModel()
            {
                Name = "Edit",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/34/Bram_Stoker_1906.jpg/640px-Bram_Stoker_1906.jpg",
                Biography = "Abraham 'Bram' Stoker (November 8, 1847 – April 20, 1912) was an Irish author best known for his Gothic horror novel Dracula,",
                PenName = null,
                NationalityId = 78,
                Gender = "female"
            };

            var invalidId = 124_843_322;
            var result = await this.authorService.EditAsync(invalidId, editModel);
            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Be(string.Format(DbEntityNotFound, nameof(Author), invalidId));
        }

        [Fact]
        public async Task EditAsync_ShouldReturnResultWithError_WhenCurrentUser_IsNotTheAuthorCreator()
        {
            var createModel = new CreateAuthorServiceModel()
            {
                Name = "Bram Stoker",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/34/Bram_Stoker_1906.jpg/640px-Bram_Stoker_1906.jpg",
                Biography =
                   "Abraham 'Bram' Stoker (November 8, 1847 – April 20, 1912) was an Irish author best known for his Gothic horror novel Dracula" +
                   "Abraham 'Bram' Stoker (November 8, 1847 – April 20, 1912) was an Irish author best known for his Gothic horror novel Dracula,",
                PenName = "Bram Stoker",
                NationalityId = 78,
                Gender = "male"
            };

            var id = await this.authorService.CreateAsync(createModel);

            var author = await this.data.Authors.FindAsync(id);
            author!.CreatorId = "fake-user-id";
            await this.data.SaveChangesAsync();

            var editModel = new CreateAuthorServiceModel()
            {
                Name = "Edit",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/34/Bram_Stoker_1906.jpg/640px-Bram_Stoker_1906.jpg",
                Biography = "Abraham 'Bram' Stoker (November 8, 1847 – April 20, 1912) was an Irish author best known for his Gothic horror novel Dracula,",
                PenName = null,
                NationalityId = 78,
                Gender = "female"
            };

            var result = await this.authorService.EditAsync(id, editModel);
            result.Succeeded.Should().BeFalse();
            result
                .ErrorMessage
                .Should()
                .Be(string.Format(
                    UnauthorizedDbEntityAction,
                    "current-user-username",
                    nameof(Author),
                    id));
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteAuthor_WhenAuthorExists_AndUserIsTheCreator()
        {
            var createModel = new CreateAuthorServiceModel()
            {
                Name = "Bram Stoker",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/34/Bram_Stoker_1906.jpg/640px-Bram_Stoker_1906.jpg",
                Biography =
                 "Abraham 'Bram' Stoker (November 8, 1847 – April 20, 1912) was an Irish author best known for his Gothic horror novel Dracula" +
                 "Abraham 'Bram' Stoker (November 8, 1847 – April 20, 1912) was an Irish author best known for his Gothic horror novel Dracula,",
                PenName = "Bram Stoker",
                NationalityId = 78,
                Gender = "male"
            };

            var id = await this.authorService.CreateAsync(createModel);

            var author = await this.data.Authors.FindAsync(id);
            author!.CreatorId = "current-user-id";
            await this.data.SaveChangesAsync();

            var result = await this.authorService.DeleteAsync(id);
            result.Succeeded.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();

            var deletedDbEntity = await this.data
                .Authors
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(a => a.Id == id);

            deletedDbEntity!.IsDeleted.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnResultWithError_WhenAuthorDoesNotExist()
        {
            var createModel = new CreateAuthorServiceModel()
            {
                Name = "Bram Stoker",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/34/Bram_Stoker_1906.jpg/640px-Bram_Stoker_1906.jpg",
                Biography =
                "Abraham 'Bram' Stoker (November 8, 1847 – April 20, 1912) was an Irish author best known for his Gothic horror novel Dracula" +
                "Abraham 'Bram' Stoker (November 8, 1847 – April 20, 1912) was an Irish author best known for his Gothic horror novel Dracula,",
                PenName = "Bram Stoker",
                NationalityId = 78,
                Gender = "male"
            };

            _ = await this.authorService.CreateAsync(createModel);

            var invalidId = 132_124_142;
            var result = await this.authorService.DeleteAsync(invalidId);
            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Be(string.Format(DbEntityNotFound, nameof(Author), invalidId));
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnResultWithError_WhenCurrentUser_IsNotTheAuthorCreator_AndIsNotAdmin()
        {
            var createModel = new CreateAuthorServiceModel()
            {
                Name = "Bram Stoker",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/34/Bram_Stoker_1906.jpg/640px-Bram_Stoker_1906.jpg",
                Biography =
                  "Abraham 'Bram' Stoker (November 8, 1847 – April 20, 1912) was an Irish author best known for his Gothic horror novel Dracula" +
                  "Abraham 'Bram' Stoker (November 8, 1847 – April 20, 1912) was an Irish author best known for his Gothic horror novel Dracula,",
                PenName = "Bram Stoker",
                NationalityId = 78,
                Gender = "male"
            };

            var id = await this.authorService.CreateAsync(createModel);

            var author = await this.data.Authors.FindAsync(id);
            author!.CreatorId = "fake-user-id";
            await this.data.SaveChangesAsync();

            var result = await this.authorService.DeleteAsync(id);
            result.Succeeded.Should().BeFalse();
            result
                .ErrorMessage
                .Should()
                .Be(string.Format(
                    UnauthorizedDbEntityAction,
                    "current-user-username",
                    nameof(Author),
                    id));
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnResultWithError_WhenCurrentUser_IsNotTheAuthorCreator_ButIsAdmin()
        {
            this.SetIsAdmin(true);

            var createModel = new CreateAuthorServiceModel()
            {
                Name = "Bram Stoker",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/34/Bram_Stoker_1906.jpg/640px-Bram_Stoker_1906.jpg",
                Biography =
                  "Abraham 'Bram' Stoker (November 8, 1847 – April 20, 1912) was an Irish author best known for his Gothic horror novel Dracula" +
                  "Abraham 'Bram' Stoker (November 8, 1847 – April 20, 1912) was an Irish author best known for his Gothic horror novel Dracula,",
                PenName = "Bram Stoker",
                NationalityId = 78,
                Gender = "male"
            };

            var id = await this.authorService.CreateAsync(createModel);

            var author = await this.data.Authors.FindAsync(id);
            author!.CreatorId = "fake-user-id";
            await this.data.SaveChangesAsync();

            var result = await this.authorService.DeleteAsync(id);
            result.Succeeded.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();

            var deletedDbEntity = await this.data
                .Authors
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(a => a.Id == id);

            deletedDbEntity!.IsDeleted.Should().BeTrue();

            this.SetIsAdmin(false);
        }

        [Fact]
        public async Task ApproveAsync_ShouldReturnResultWithError_WhenAuthorDoesNotExist()
        {
            var invalidId = 132_124_142;
            var result = await this.authorService.ApproveAsync(invalidId);
            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Be(string.Format(DbEntityNotFound, nameof(Author), invalidId));
        }

        [Fact]
        public async Task ApproveAsync_ShouldReturnResultWithError_WhenCurrentUser_IsNotAdmin()
        {
            var createModel = new CreateAuthorServiceModel()
            {
                Name = "Bram Stoker",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/34/Bram_Stoker_1906.jpg/640px-Bram_Stoker_1906.jpg",
                Biography =
                  "Abraham 'Bram' Stoker (November 8, 1847 – April 20, 1912) was an Irish author best known for his Gothic horror novel Dracula" +
                  "Abraham 'Bram' Stoker (November 8, 1847 – April 20, 1912) was an Irish author best known for his Gothic horror novel Dracula,",
                PenName = "Bram Stoker",
                NationalityId = 78,
                Gender = "male"
            };

            var id = await this.authorService.CreateAsync(createModel);

            var result = await this.authorService.ApproveAsync(id);
            result.Succeeded.Should().BeFalse();
            result
                .ErrorMessage
                .Should()
                .Be(string.Format(
                    UnauthorizedDbEntityAction,
                    "current-user-username",
                    nameof(Author),
                    id));
        }

        [Fact]
        public async Task ApproveAsync_ShouldSetIsApprovedToTrue_WhenCurrentUser_IsAdmin_AndAuthorExists()
        {
            this.SetIsAdmin(true);

            var createModel = new CreateAuthorServiceModel()
            {
                Name = "Bram Stoker",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/34/Bram_Stoker_1906.jpg/640px-Bram_Stoker_1906.jpg",
                Biography =
                  "Abraham 'Bram' Stoker (November 8, 1847 – April 20, 1912) was an Irish author best known for his Gothic horror novel Dracula" +
                  "Abraham 'Bram' Stoker (November 8, 1847 – April 20, 1912) was an Irish author best known for his Gothic horror novel Dracula,",
                PenName = "Bram Stoker",
                NationalityId = 78,
                Gender = "male"
            };

            var id = await this.authorService.CreateAsync(createModel);

            var result = await this.authorService.ApproveAsync(id);
            result.Succeeded.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();

            var approvedEntity = await this.data.Authors.FindAsync(id);
            approvedEntity.Should().NotBeNull();
            approvedEntity!.IsApproved.Should().BeTrue();

            this.SetIsAdmin(false);
        }

        [Fact]
        public async Task RejectAsync_ShouldReturnResultWithError_WhenAuthorDoesNotExist()
        {
            var invalidId = 132_124_142;
            var result = await this.authorService.RejectAsync(invalidId);
            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Be(string.Format(DbEntityNotFound, nameof(Author), invalidId));
        }

        [Fact]
        public async Task RejectAsync_ShouldReturnResultWithError_WhenCurrentUser_IsNotAdmin()
        {
            var createModel = new CreateAuthorServiceModel()
            {
                Name = "Bram Stoker",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/34/Bram_Stoker_1906.jpg/640px-Bram_Stoker_1906.jpg",
                Biography =
                  "Abraham 'Bram' Stoker (November 8, 1847 – April 20, 1912) was an Irish author best known for his Gothic horror novel Dracula" +
                  "Abraham 'Bram' Stoker (November 8, 1847 – April 20, 1912) was an Irish author best known for his Gothic horror novel Dracula,",
                PenName = "Bram Stoker",
                NationalityId = 78,
                Gender = "male"
            };

            var id = await this.authorService.CreateAsync(createModel);

            var result = await this.authorService.RejectAsync(id);
            result.Succeeded.Should().BeFalse();
            result
                .ErrorMessage
                .Should()
                .Be(string.Format(
                    UnauthorizedDbEntityAction,
                    "current-user-username",
                    nameof(Author),
                    id));
        }

        [Fact]
        public async Task RejectAsync_ShouldDeleteTheAuthor_WhenCurrentUser_IsAdmin_AndAuthorExists()
        {
            this.SetIsAdmin(true);

            var createModel = new CreateAuthorServiceModel()
            {
                Name = "Bram Stoker",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/34/Bram_Stoker_1906.jpg/640px-Bram_Stoker_1906.jpg",
                Biography =
                  "Abraham 'Bram' Stoker (November 8, 1847 – April 20, 1912) was an Irish author best known for his Gothic horror novel Dracula" +
                  "Abraham 'Bram' Stoker (November 8, 1847 – April 20, 1912) was an Irish author best known for his Gothic horror novel Dracula,",
                PenName = "Bram Stoker",
                NationalityId = 78,
                Gender = "male"
            };

            var id = await this.authorService.CreateAsync(createModel);

            var result = await this.authorService.RejectAsync(id);
            result.Succeeded.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();

            var deletedEntity = await this.data
                .Authors
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(a => a.Id == id);

            deletedEntity!.IsDeleted.Should().BeTrue();

            this.SetIsAdmin(false);
        }

        private void SetIsAdmin(bool isAdmin)
            => this.mockUserService
                .Setup(x => x.IsAdmin())
                .Returns(isAdmin);

        private async Task PrepareDbAsync()
        {
            if (await this.data.Authors.AnyAsync())
            {
                return;
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

                new()
                {
                    Id = 2,
                    Name = "Joanne Rowling",
                    ImageUrl = "https://stories.jkrowling.com/wp-content/uploads/2021/09/Shot-B-105_V2_CROP-e1630873059779.jpg",
                    Biography =
                        "Joanne Rowling (born July 31, 1965), known by her pen name J.K. Rowling, is a British author and philanthropist. " +
                        "She is best known for writing the Harry Potter series, a seven-book fantasy saga that has become a global phenomenon. " +
                        "The series has sold over 600 million copies, been translated into 84 languages, and inspired a massive multimedia franchise, " +
                        "including blockbuster films, stage plays, video games, and theme parks. Notable works include Harry Potter and the " +
                        "Philosopher's Stone, Harry Potter and the Deathly Hallows, and The Tales of Beedle the Bard. Rowling's writing is praised for its " +
                        "imaginative world-building, compelling characters, and exploration of themes such as love, loyalty, and the battle between good " +
                        "and evil. After completing the Harry Potter series, Rowling transitioned to writing for adults, debuting with The Casual Vacancy, " +
                        "a contemporary social satire. She also writes crime fiction under the pseudonym Robert Galbraith, authoring the acclaimed " +
                        "Cormoran Strike series. Rowling has received numerous awards and honors for her literary achievements, including the " +
                        "Order of the British Empire (OBE) for services to children’s literature. She is an advocate for various charitable causes " +
                        "and founded the Volant Charitable Trust to combat social inequality. Rowling lives in Scotland with her family and continues " +
                        "to write, inspiring readers of all ages with her imaginative storytelling and philanthropy.",
                    PenName = "J. K. Rowling",
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
                        "John Ronald Reuel Tolkien (January 3, 1892 – September 2, 1973) was an English writer, philologist, and academic. He is best " +
                        "known as the author of The Hobbit and The Lord of the Rings, two of the most beloved works in modern fantasy literature. " +
                        "Tolkien's work has had a profound impact on the fantasy genre, establishing many of the conventions and archetypes that define it " +
                        "today. Set in the richly detailed world of Middle-earth, his stories feature intricate mythologies, languages, and histories, " +
                        "reflecting his scholarly expertise in philology and medieval studies. The Hobbit (1937) was a critical and commercial success, " +
                        "leading to the epic sequel The Lord of the Rings trilogy (1954–1955), which has sold over 150 million copies and been adapted " +
                        "into award-winning films by director Peter Jackson. Tolkien also authored The Silmarillion, a collection of myths and legends " +
                        "that expand the lore of Middle-earth. He served as a professor of Anglo-Saxon at the University of Oxford, where he was a member " +
                        "of the literary group The Inklings, alongside C.S. Lewis. Tolkien's contributions to literature earned him global acclaim and a " +
                        "lasting legacy as the 'father of modern fantasy.' He passed away in 1973, but his works continue to captivate readers worldwide.",
                    PenName = "J.R.R Tolkien",
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
                        "Ken Elton Kesey (September 17, 1935 – November 10, 2001) was an American novelist, essayist, and countercultural figure. He is " +
                        "best known for his debut novel One Flew Over the Cuckoo's Nest (1962), which explores themes of individuality, authority, and " +
                        "mental health. The novel became an instant classic, earning critical acclaim for its portrayal of life inside a psychiatric " +
                        "institution and inspiring a celebrated 1975 film adaptation starring Jack Nicholson that won five Academy Awards. " +
                        "Kesey was deeply involved in the counterculture movement of the 1960s, becoming a key figure among the Merry Pranksters, " +
                        "a group famous for their cross-country bus trip chronicled in Tom Wolfe's The Electric Kool-Aid Acid Test. " +
                        "His second novel, Sometimes a Great Notion (1964), was praised for its ambitious narrative structure and portrayal of family " +
                        "dynamics. Kesey's work often reflects his fascination with the human condition, rebellion, and the nature of freedom. " +
                        "In addition to his literary career, Kesey experimented with psychedelics, drawing inspiration from his participation in government " +
                        "experiments and his own personal experiences. He remained an influential voice in American literature and culture, inspiring " +
                        "generations of readers and writers to question societal norms and explore new perspectives. Kesey passed away in 2001, leaving " +
                        "behind a legacy of bold storytelling and a spirit of rebellion.",
                    PenName = null,
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
                        "Eric Arthur Blair (June 25, 1903 – January 21, 1950), known by his pen name George Orwell, was an English novelist, essayist, " +
                        "journalist, and critic. Orwell is best known for his works Animal Farm (1945) and Nineteen Eighty-Four (1949), both of which " +
                        "are considered cornerstones of modern English literature and have had a lasting impact on political thought. Animal Farm, " +
                        "an allegorical novella satirizing the Russian Revolution and the rise of Stalinism, has become a standard in literature about " +
                        "totalitarianism and political corruption. Nineteen Eighty-Four, a dystopian novel set in a totalitarian society controlled by " +
                        "Big Brother, has influenced political discourse around surveillance, propaganda, and government control. Orwell's writings often s" +
                        "explore themes of social injustice, totalitarianism, and the misuse of power. He was an ardent critic of fascism and communism " +
                        "and was deeply involved in political activism, including fighting in the Spanish Civil War, which influenced his strong " +
                        "anti-authoritarian views. Orwell's style is known for its clarity, precision, and biting social commentary, and his work continues " +
                        "to be relevant and influential in discussions on politics, language, and individual rights. Orwell passed away in 1950 at the age " +
                        "of 46, but his work remains widely read and influential, especially in the context of discussions surrounding state power, civil " +
                        "liberties, and the role of the individual in society.",
                    PenName = "George Orwell",
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

            if (await this.data.Nationalities.AnyAsync())
            {
                return;
            }

            this.data.Add(new Nationality { Id = 78, Name = "Ireland" });
            await this.data.SaveChangesAsync();
        }
    }
}
