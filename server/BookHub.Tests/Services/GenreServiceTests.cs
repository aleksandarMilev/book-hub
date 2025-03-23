namespace BookHub.Tests.Services
{
    using AutoMapper;
    using Data;
    using Features.Genre.Data.Models;
    using Features.Genre.Mapper;
    using Features.Genre.Service;
    using Features.Genre.Service.Models;
    using FluentAssertions;
    using Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class GenreServiceTests
    {
        private readonly BookHubDbContext data;

        private readonly Mock<ICurrentUserService> mockUserService;
        private readonly IMapper mapper;

        private readonly GenreService genreService;

        public GenreServiceTests()
        {
            var options = new DbContextOptionsBuilder<BookHubDbContext>()
                .UseInMemoryDatabase(databaseName: "GenreServiceInMemoryDatabase")
                .Options;

            this.mockUserService = new Mock<ICurrentUserService>();
            this.data = new BookHubDbContext(options, this.mockUserService.Object);

            this.mapper = new MapperConfiguration(cfg => cfg.AddProfile(new GenreMapper())).CreateMapper();

            this.genreService = new GenreService(this.data, this.mapper);

            Task
                .Run(this.PrepareDb)
                .GetAwaiter()
                .GetResult();
        }

        [Fact]
        public async Task Names_ShouldReturn_CollectionFromServiceModels()
        {
            var genres = await this.genreService.Names();

            genres.Should().NotBeEmpty();
            genres.Should().AllBeOfType(typeof(GenreNameServiceModel));
            genres.Should().HaveCount(5);
            genres
                .Should()
                .ContainSingle(g => g.Id == 1 && g.Name == "Horror");
        }

        [Fact]
        public async Task Details_ShouldReturnNull_IfIdIsInvalid()
        {
            var invalidId = 142_325_214;
            var genre = await this.genreService.Details(invalidId);

            genre.Should().BeNull();
        }

        [Fact]
        public async Task Details_ShouldReturnServiceModel_IfIdIsValid()
        {
            var id = 1;
            var genre = await this.genreService.Details(id);

            genre.Should().NotBeNull();
            genre.Should().BeOfType(typeof(GenreDetailsServiceModel));
            genre!.Name.Should().Be("Horror");
            genre!.Description.Should().Contain("Horror fiction is designed to scare");
        }

        private async Task PrepareDb()
        {
            if (await this.data.Genres.AnyAsync())
            {
                return;
            }

            var genres = new Genre[]
            {
                new()
                {
                    Id = 1,
                    Name = "Horror",
                    Description = "Horror fiction is designed to scare, unsettle, or horrify readers unknown",
                    ImageUrl = "https://org-dcmp-staticassets.s3.us-east-1.amazonaws.com/posterimages/13453_1.jpg"
                },
                new()
                {
                    Id = 2,
                    Name = "Science Fiction",
                    Description = "Science fiction explores futuristic, scientific, and technological themes",
                    ImageUrl = "https://www.editoreric.com/greatlit/litgraphics/book-spiral-galaxy.jpg"
                },
                new()
                {
                    Id = 3,
                    Name = "Fantasy",
                    Description = "Fantasy stories transport readers to magical realms filled with ts",
                    ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT5EcrB6fhai5L3-7Ted6fZgxUjCti0W4avrA&s"
                },
                new()
                {
                    Id = 4,
                    Name = "Mystery",
                    Description = "Mystery fiction is a puzzle-driven genre that engages reintrigue",
                    ImageUrl = "https://celadonbooks.com/wp-content/uploads/2020/03/what-is-a-mystery.jpg"
                },
                new()
                {
                    Id = 5,
                    Name = "Romance",
                    Description = "Romance novels celebrate the complexities of love and and emotional growth",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/3/36/Hammond-SS10.jpg"
                },
            };

            this.data.AddRange(genres);
            await this.data.SaveChangesAsync();
        }
    }
}
