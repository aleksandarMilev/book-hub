namespace BookHub.Server.Tests.Services
{
    using Data;
    using Features.Authors.Data.Models;
    using Features.Authors.Service;
    using FluentAssertions;
    using Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class NationalityServiceTests
    {
        private readonly BookHubDbContext data;
        private readonly Mock<ICurrentUserService> mockUserService;

        private readonly NationalityService nationalityService;

        public NationalityServiceTests()
        {
            var options = new DbContextOptionsBuilder<BookHubDbContext>()
                .UseInMemoryDatabase(databaseName: "ArticleServiceInMemoryDatabase")
                .Options;

            this.mockUserService = new Mock<ICurrentUserService>();
            this.data = new BookHubDbContext(options, this.mockUserService.Object);

            this.nationalityService = new NationalityService(this.data);

            Task
                .Run(this.PrepareDbAsync)
                .GetAwaiter()
                .GetResult();
        }

        [Fact]
        public async Task NamesAsync_ShouldReturn_CollectionFromServiceModels()
        {
            var nationalities = await this.nationalityService.NamesAsync();

            nationalities.Should().NotBeEmpty();
            nationalities.Should().HaveCount(13);
            nationalities
                .Should()
                .ContainSingle(n => n.Id == 1 && n.Name == "Afghanistan");
        }

        private async Task PrepareDbAsync() 
        {
            if (await this.data.Nationalities.AnyAsync())
            {
                return;
            }

            var nationalities = new Nationality[]
            {
                new() { Id = 1, Name = "Afghanistan" },
                new() { Id = 2, Name = "Albania" },
                new() { Id = 3, Name = "Algeria" },
                new() { Id = 4, Name = "Andorra" },
                new() { Id = 5, Name = "Angola" },
                new() { Id = 6, Name = "Antigua and Barbuda" },
                new() { Id = 7, Name = "Argentina" },
                new() { Id = 8, Name = "Armenia" },
                new() { Id = 9, Name = "Australia" },
                new() { Id = 10, Name = "Austria" },
                new() { Id = 11, Name = "Azerbaijan" },
                new() { Id = 12, Name = "Bahamas" },
                new() { Id = 13, Name = "Bahrain" },
            };

            this.data.AddRange(nationalities);
            await this.data.SaveChangesAsync();
        }
    }
}
