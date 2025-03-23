namespace BookHub.Tests.Services
{
    using AutoMapper;
    using Data;
    using Features.Article.Data.Models;
    using Features.Article.Mapper;
    using Features.Article.Service;
    using Features.Article.Service.Models;
    using FluentAssertions;
    using Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    using static Common.ErrorMessage;

    public class ArticleServiceTests
    {
        private readonly BookHubDbContext data;

        private readonly Mock<ICurrentUserService> mockUserService;
        private readonly IMapper mapper;

        private readonly ArticleService articleService;

        public ArticleServiceTests()
        {
            var options = new DbContextOptionsBuilder<BookHubDbContext>()
                .UseInMemoryDatabase(databaseName: "ArticleServiceInMemoryDatabase")
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

            this.mapper = new MapperConfiguration(cfg => cfg.AddProfile(new ArticleMapper())).CreateMapper();

            this.articleService = new ArticleService(
                this.data,
                this.mockUserService.Object,
                this.mapper
            );

            Task
                .Run(this.PrepareDb)
                .GetAwaiter()
                .GetResult();
        }

        [Fact]
        public async Task Details_ShouldReturnNull_IfArticleWithThisIdDoesNotExists()
        {
            var invalidId = 123_124_214;
            var article = await this.articleService.Details(invalidId);
            article.Should().BeNull();
        }

        [Fact]
        public async Task Details_ShouldReturnTheCorrectArticle_IfIdIsValid_AndAlso_ShouldIncrementViewCount()
        {
            var id = 1;
            var article = await this.articleService.Details(id);
            article.Should().NotBeNull();
            article!.Title.Should().Be("Exploring the Haunting Depths of Pet Sematary");

            var prevViewCount = article.Views;

            var anotherRequest = await this.articleService.Details(id);

            (anotherRequest!.Views - prevViewCount).Should().Be(1);
        }

        [Fact]
        public async Task Create_ShouldCreateArticle_AndReturnId()
        {
            var model = new CreateArticleServiceModel()
            {
                Title = "TestTestTest",
                Introduction = "TestTestTestTestTestTestTestTestTest.",
                Content =
                        "Stephen King's Pet Sematary is not just a horror story; it is a poignant exploration of grief, the inevitability of death, and the dark corners of the human psyche. " +
                        "At its heart, the novel is about Louis Creed, a young father who moves with his wife, Rachel, and their two children to a rural house in Maine. Soon after arriving, they discover a " +
                        "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                        "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                        "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                        "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                        "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting "
                    ,
                ImageUrl = "https://thedailytexan.com/wp-content/uploads/2023/09/pet_sematary_bloodlines_4press-1200x800.jpg",
            };

            var id = await this.articleService.Create(model);
            id.Should().BeGreaterThan(0);

            var article = await this.data.Articles.FindAsync(id);
            article.Should().NotBeNull();
            article!.Title.Should().Be("TestTestTest");
        }

        [Fact]
        public async Task Edit_ShouldReturnResultWithError_IfIdIsNotValid()
        {
            var createModel = new CreateArticleServiceModel()
            {
                Title = "TestTestTest",
                Introduction = "TestTestTestTestTestTestTestTestTest.",
                Content =
                        "Stephen King's Pet Sematary is not just a horror story; it is a poignant exploration of grief, the inevitability of death, and the dark corners of the human psyche. " +
                        "At its heart, the novel is about Louis Creed, a young father who moves with his wife, Rachel, and their two children to a rural house in Maine. Soon after arriving, they discover a " +
                        "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                        "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                        "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                        "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                        "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting "
                    ,
                ImageUrl = "https://thedailytexan.com/wp-content/uploads/2023/09/pet_sematary_bloodlines_4press-1200x800.jpg",
            };

            _ = await this.articleService.Create(createModel);

            var editModel = new CreateArticleServiceModel()
            {
                Title = "EditEditEdit",
                Introduction = "EditEditEditEditEditEditEditEditEdit.",
                Content =
                     "Stephen King's Pet Sematary is not just a horror story; it is a poignant exploration of grief, the inevitability of death, and the dark corners of the human psyche. " +
                     "At its heart, the novel is about Louis Creed, a young father who moves with his wife, Rachel, and their two children to a rural house in Maine. Soon after arriving, they discover a " +
                     "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                     "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                     "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                     "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                     "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting "
                 ,
                ImageUrl = "https://thedailytexan.com/wp-content/uploads/2023/09/pet_sematary_bloodlines_4press-1200x800.jpg",
            };

            var invalidId = 143_239_849;
            var result = await this.articleService.Edit(invalidId, editModel);
            result.Succeeded.Should().BeFalse();
            result
                .ErrorMessage
                .Should()
                .Be(string.Format(DbEntityNotFound, nameof(Article), invalidId));
        }

        [Fact]
        public async Task Edit_ShouldReturnResultWithError_IfCurrentUserIsNotAdmin()
        {
            var createModel = new CreateArticleServiceModel()
            {
                Title = "TestTestTest",
                Introduction = "TestTestTestTestTestTestTestTestTest.",
                Content =
                        "Stephen King's Pet Sematary is not just a horror story; it is a poignant exploration of grief, the inevitability of death, and the dark corners of the human psyche. " +
                        "At its heart, the novel is about Louis Creed, a young father who moves with his wife, Rachel, and their two children to a rural house in Maine. Soon after arriving, they discover a " +
                        "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                        "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                        "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                        "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                        "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting "
                    ,
                ImageUrl = "https://thedailytexan.com/wp-content/uploads/2023/09/pet_sematary_bloodlines_4press-1200x800.jpg",
            };

            var id = await this.articleService.Create(createModel);

            var editModel = new CreateArticleServiceModel()
            {
                Title = "EditEditEdit",
                Introduction = "EditEditEditEditEditEditEditEditEdit.",
                Content =
                     "Stephen King's Pet Sematary is not just a horror story; it is a poignant exploration of grief, the inevitability of death, and the dark corners of the human psyche. " +
                     "At its heart, the novel is about Louis Creed, a young father who moves with his wife, Rachel, and their two children to a rural house in Maine. Soon after arriving, they discover a " +
                     "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                     "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                     "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                     "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                     "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting "
                 ,
                ImageUrl = "https://thedailytexan.com/wp-content/uploads/2023/09/pet_sematary_bloodlines_4press-1200x800.jpg",
            };

            var result = await this.articleService.Edit(id, editModel);
            result.Succeeded.Should().BeFalse();
            result
                .ErrorMessage
                .Should()
                .Be(string.Format(
                    UnauthorizedDbEntityAction,
                    "current-user-username",
                    nameof(Article),
                    id));
        }

        [Fact]
        public async Task Edit_ShouldEditTheArticle_AndReturnResultWithSuccess_IfCurrentUserIsAdmin()
        {
            this.SetIsAdmin(true);

            var createModel = new CreateArticleServiceModel()
            {
                Title = "TestTestTest",
                Introduction = "TestTestTestTestTestTestTestTestTest.",
                Content =
                        "Stephen King's Pet Sematary is not just a horror story; it is a poignant exploration of grief, the inevitability of death, and the dark corners of the human psyche. " +
                        "At its heart, the novel is about Louis Creed, a young father who moves with his wife, Rachel, and their two children to a rural house in Maine. Soon after arriving, they discover a " +
                        "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                        "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                        "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                        "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                        "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting "
                    ,
                ImageUrl = "https://thedailytexan.com/wp-content/uploads/2023/09/pet_sematary_bloodlines_4press-1200x800.jpg",
            };

            var id = await this.articleService.Create(createModel);

            var editModel = new CreateArticleServiceModel()
            {
                Title = "EditEditEdit",
                Introduction = "EditEditEditEditEditEditEditEditEdit.",
                Content =
                     "Stephen King's Pet Sematary is not just a horror story; it is a poignant exploration of grief, the inevitability of death, and the dark corners of the human psyche. " +
                     "At its heart, the novel is about Louis Creed, a young father who moves with his wife, Rachel, and their two children to a rural house in Maine. Soon after arriving, they discover a " +
                     "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                     "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                     "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                     "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                     "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting "
                 ,
                ImageUrl = "https://thedailytexan.com/wp-content/uploads/2023/09/pet_sematary_bloodlines_4press-1200x800.jpg",
            };

            var result = await this.articleService.Edit(id, editModel);
            result.Succeeded.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();

            var article = await this.data.Articles.FindAsync(id);
            article!.Title.Should().Be("EditEditEdit");
            article!.Introduction.Should().Be("EditEditEditEditEditEditEditEditEdit.");

            this.SetIsAdmin(false);
        }

        [Fact]
        public async Task Delete_ShouldReturnResultWithError_IfIdIsNotValid()
        {
            var invalidId = 143_239_849;
            var result = await this.articleService.Delete(invalidId);
            result.Succeeded.Should().BeFalse();
            result
                .ErrorMessage
                .Should()
                .Be(string.Format(DbEntityNotFound, nameof(Article), invalidId));
        }

        [Fact]
        public async Task Delete_ShouldReturnResultWithError_IfCurrentUserIsNotAdmin()
        {
            var model = new CreateArticleServiceModel()
            {
                Title = "TestTestTest",
                Introduction = "TestTestTestTestTestTestTestTestTest.",
                Content =
                        "Stephen King's Pet Sematary is not just a horror story; it is a poignant exploration of grief, the inevitability of death, and the dark corners of the human psyche. " +
                        "At its heart, the novel is about Louis Creed, a young father who moves with his wife, Rachel, and their two children to a rural house in Maine. Soon after arriving, they discover a " +
                        "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                        "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                        "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                        "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                        "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting "
                    ,
                ImageUrl = "https://thedailytexan.com/wp-content/uploads/2023/09/pet_sematary_bloodlines_4press-1200x800.jpg",
            };

            var id = await this.articleService.Create(model);

            var result = await this.articleService.Delete(id);
            result.Succeeded.Should().BeFalse();
            result
                .ErrorMessage
                .Should()
                .Be(string.Format(
                    UnauthorizedDbEntityAction,
                    "current-user-username",
                    nameof(Article),
                    id));
        }

        [Fact]
        public async Task Delete_ShouldDeleteTheArticle_AndReturnResultWithSuccess_IfCurrentUserIsAdmin()
        {
            this.SetIsAdmin(true);

            var createModel = new CreateArticleServiceModel()
            {
                Title = "TestTestTest",
                Introduction = "TestTestTestTestTestTestTestTestTest.",
                Content =
                        "Stephen King's Pet Sematary is not just a horror story; it is a poignant exploration of grief, the inevitability of death, and the dark corners of the human psyche. " +
                        "At its heart, the novel is about Louis Creed, a young father who moves with his wife, Rachel, and their two children to a rural house in Maine. Soon after arriving, they discover a " +
                        "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                        "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                        "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                        "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                        "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting "
                    ,
                ImageUrl = "https://thedailytexan.com/wp-content/uploads/2023/09/pet_sematary_bloodlines_4press-1200x800.jpg",
            };

            var id = await this.articleService.Create(createModel);

            var result = await this.articleService.Delete(id);
            result.Succeeded.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();

            var article = await this.data
                .Articles
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(a => a.Id == id);

            article!.IsDeleted.Should().BeTrue();

            this.SetIsAdmin(false);
        }

        private void SetIsAdmin(bool isAdmin)
            => this.mockUserService
                .Setup(x => x.IsAdmin())
                .Returns(isAdmin);

        private async Task PrepareDb()
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
                        "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                        "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                        "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting " +
                        "mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting "
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
                        "the magical world but also through the very heart of what it means to face evil, to stand up for what is right, and to embrace the unknown. " +
                        "the magical world but also through the very heart of what it means to face evil, to stand up for what is right, and to embrace the unknown. " +
                        "the magical world but also through the very heart of what it means to face evil, to stand up for what is right, and to embrace the unknown. "
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
            };

            if (await this.data.Articles.AnyAsync())
            {
                return;
            }

            this.data.AddRange(articles);
            await this.data.SaveChangesAsync();
        }
    }
}
