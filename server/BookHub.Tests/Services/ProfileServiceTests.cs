namespace BookHub.Tests.Services
{
    using AutoMapper;
    using Common;
    using Data;
    using Features.Identity.Data.Models;
    using Features.UserProfile.Data.Models;
    using Features.UserProfile.Mapper;
    using Features.UserProfile.Service;
    using Features.UserProfile.Service.Models;
    using FluentAssertions;
    using Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class ProfileServiceTests
    {
        private readonly BookHubDbContext data;

        private readonly Mock<ICurrentUserService> mockUserService;
        private readonly IMapper mapper;

        private readonly ProfileService profileService;

        public ProfileServiceTests()
        {
            var options = new DbContextOptionsBuilder<BookHubDbContext>()
                .UseInMemoryDatabase(databaseName: "ProfileServiceInMemoryDatabase")
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

            this.mapper = new MapperConfiguration(cfg => cfg.AddProfile(new ProfileMapper())).CreateMapper();

            this.profileService = new ProfileService(
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
        public async Task HasProfile_ShouldReturnTrue_WhenUserHasProfile()
        {
            var userId = "user1Id";
            this.SetCurrentUserId(userId);

            var result = await this.profileService.HasProfile();
            result.Should().BeTrue();
        }

        [Fact]
        public async Task HasProfile_ShouldReturnFalse_WhenUserDoesNotHaveProfile()
        {
            var userId = "user6Id";
            this.SetCurrentUserId(userId);

            var result = await this.profileService.HasProfile();
            result.Should().BeFalse();
        }

        [Fact]
        public async Task HasProfile_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            var userId = "invalidId";
            this.SetCurrentUserId(userId);

            var result = await this.profileService.HasProfile();
            result.Should().BeFalse();
        }

        [Fact]
        public async Task HasMoreThanFiveCurrentlyReading_ShouldReturnTrue_WhenCurrentlyReadingBooksCountIsFive()
        {
            var userId = "user4Id";
            this.SetCurrentUserId(userId);

            var result = await this.profileService.HasMoreThanFiveCurrentlyReading(userId);
            result.Should().BeTrue();
        }

        [Fact]
        public async Task HasMoreThanFiveCurrentlyReading_ShouldReturnFalse_WhenCurrentlyReadingBooksCountIsLessThanFive()
        {
            var userId = "user1Id";
            this.SetCurrentUserId(userId);

            var result = await this.profileService.HasMoreThanFiveCurrentlyReading(userId);
            result.Should().BeFalse();
        }

        [Fact]
        public async Task HasMoreThanFiveCurrentlyReading_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            var invalidId = "invalid-id";
            this.SetCurrentUserId(invalidId);

            var result = await this.profileService.HasMoreThanFiveCurrentlyReading(invalidId);
            result.Should().BeFalse();
        }

        [Fact]
        public async Task TopThree_ShouldReturnTopThreeProfiles_ByActivityScore()
        {
            var expectedProfiles = await this.data.Profiles
                .OrderByDescending(p => p.CreatedBooksCount + p.CreatedAuthorsCount + p.ReviewsCount)
                .Take(3)
                .ToListAsync();

            var actualProfiles = (await this.profileService.TopThree()).ToList();
            actualProfiles.Should().HaveCount(3);
            actualProfiles[0].Id.Should().Be(expectedProfiles[0].UserId);
            actualProfiles[1].Id.Should().Be(expectedProfiles[1].UserId);
            actualProfiles[2].Id.Should().Be(expectedProfiles[2].UserId); 
        }

        [Fact]
        public async Task Mine_ShouldReturnProfile_WhenCurrentUserHasProfile()
        {
            var userId = "user1Id";
            this.SetCurrentUserId(userId);

            var result = await this.profileService.Mine();
            result.Should().NotBeNull();
            result!.Id.Should().Be(userId);
            result!.FirstName.Should().Be("John"); 
            result!.LastName.Should().Be("Doe");
        }

        [Fact]
        public async Task Mine_ShouldReturnNull_WhenCurrentUserDoesNotHaveProfile()
        {
            var userId = "user6Id";
            this.SetCurrentUserId(userId);

            var result = await this.profileService.Mine();
            result.Should().BeNull();
        }

        [Fact]
        public async Task Mine_ShouldReturnNull_WhenCurrentUserDoesNotExist()
        {
            var userId = "invalidId";
            this.SetCurrentUserId(userId);

            var result = await this.profileService.Mine();
            result.Should().BeNull();
        }

        [Fact]
        public async Task OtherUser_ShouldReturnProfile_WhenUserExistsAndProfileIsPublic()
        {
            var userId = "user1Id";
            this.SetCurrentUserId(userId);

            var result = await this.profileService.OtherUser(userId);
            result.Should().NotBeNull();
            result.Should().BeOfType<ProfileServiceModel>();
            result!.Id.Should().Be(userId);
            result!.FirstName.Should().Be("John");
            result!.IsPrivate.Should().BeFalse();
        }

        [Fact]
        public async Task OtherUser_ShouldReturnPrivateProfileModel_WhenUserExistsAndProfileIsPrivate()
        {
            var userId = "user4Id";
            this.SetCurrentUserId(userId);

            var result = await this.profileService.OtherUser(userId);
            result.Should().NotBeNull();
            result.Should().BeOfType<PrivateProfileServiceModel>();
            result!.Id.Should().Be(userId);
        }

        [Fact]
        public async Task OtherUser_ShouldReturnNull_WhenUserDoesNotExist()
        {
            var userId = "invalidId";
            this.SetCurrentUserId(userId);

            var result = await this.profileService.OtherUser(userId);
            result.Should().BeNull();
        }

        [Fact]
        public async Task Create_ShouldCreateProfile_WhenModelIsValid()
        {
            var createProfileModel = new CreateProfileServiceModel()
            {
                FirstName = "Test",
                LastName = "User",
                ImageUrl = "https://mock-image.com/",
                PhoneNumber = "+123456789",
                DateOfBirth = "01 01 1999",
                SocialMediaUrl = "https://mock-media.com/",
                Biography = "Test user bio here.",
                IsPrivate = false,
            };

            var currentUserId = "current-user-id-again";
            this.SetCurrentUserId(currentUserId);

            var result = await this.profileService.Create(createProfileModel);
            result.Should().Be(currentUserId);

            var profile = await this.data
                .Profiles
                .FirstOrDefaultAsync(p => p.UserId == currentUserId);

            profile.Should().NotBeNull();
            profile!.FirstName.Should().Be(createProfileModel.FirstName);
            profile.LastName.Should().Be(createProfileModel.LastName);
            profile.ImageUrl.Should().Be(createProfileModel.ImageUrl);
            profile.PhoneNumber.Should().Be(createProfileModel.PhoneNumber);
            profile.DateOfBirth.Should().Be(new DateTime(1999, 01, 01));
            profile.SocialMediaUrl.Should().Be(createProfileModel.SocialMediaUrl);
            profile.Biography.Should().Be(createProfileModel.Biography);
            profile.IsPrivate.Should().Be(createProfileModel.IsPrivate);
        }

        [Fact]
        public async Task Edit_ShouldReturnErrorMessage_WhenProfileNotFound()
        {
            var currentUserId = "current-user-id-again-again";
            this.SetCurrentUserId(currentUserId);

            var createProfileModel = new CreateProfileServiceModel()
            {
                FirstName = "Test",
                LastName = "User",
                ImageUrl = "https://mock-image.com/",
                PhoneNumber = "+123456789",
                DateOfBirth = "01 01 1999",
                SocialMediaUrl = "https://mock-media.com/",
                Biography = "Test user bio here.",
                IsPrivate = false,
            };

            _ = await this.profileService.Create(createProfileModel);

            var invalidId = "invalid-id";
            this.SetCurrentUserId(invalidId);

            var editProfileModel = new CreateProfileServiceModel()
            {
                FirstName = "Edit",
                LastName = "Edit",
                ImageUrl = "https://mock-image.com/",
                PhoneNumber = "+987654321",
                DateOfBirth = "01 01 1999",
                SocialMediaUrl = "https://mock-media.com/",
                Biography = "edit bio text is here.",
                IsPrivate = true,
            };

            var result = await this.profileService.Edit(editProfileModel);
            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Be($"UserProfile with Id: {invalidId} was not found!");
        }

        [Fact]
        public async Task Edit_ShouldEditProfile_WhenProfileExists()
        {
            var currentUserId = "current-user-id-again-again-again";
            this.SetCurrentUserId(currentUserId);

            var createProfileModel = new CreateProfileServiceModel()
            {
                FirstName = "Test",
                LastName = "User",
                ImageUrl = "https://mock-image.com/",
                PhoneNumber = "+123456789",
                DateOfBirth = "01 01 1999",
                SocialMediaUrl = "https://mock-media.com/",
                Biography = "Test user bio here.",
                IsPrivate = false,
            };

            _ = await this.profileService.Create(createProfileModel);

            var editProfileModel = new CreateProfileServiceModel()
            {
                FirstName = "Edit",
                LastName = "Edit",
                ImageUrl = "https://mock-image.com/",
                PhoneNumber = "+987654321",
                DateOfBirth = "01 01 1999",
                SocialMediaUrl = "https://mock-media.com/",
                Biography = "edit bio text is here.",
                IsPrivate = true,
            };

            var result = await this.profileService.Edit(editProfileModel);
            result.Succeeded.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();

            var profileEdited = await this.data.Profiles.FindAsync(currentUserId);
            profileEdited.Should().NotBeNull();
            profileEdited!.FirstName.Should().Be(editProfileModel.FirstName);
            profileEdited.LastName.Should().Be(editProfileModel.LastName);
            profileEdited.ImageUrl.Should().Be(editProfileModel.ImageUrl);
            profileEdited.PhoneNumber.Should().Be(editProfileModel.PhoneNumber);
            profileEdited.DateOfBirth.Should().Be(new DateTime(1999, 01, 01));
            profileEdited.SocialMediaUrl.Should().Be(editProfileModel.SocialMediaUrl);
            profileEdited.Biography.Should().Be(editProfileModel.Biography);
            profileEdited.IsPrivate.Should().Be(editProfileModel.IsPrivate);
        }

        [Fact]
        public async Task Delete_ShouldReturnTrue_WhenProfileExists()
        {
            var createProfileModel = new CreateProfileServiceModel()
            {
                FirstName = "Test",
                LastName = "User",
                ImageUrl = "https://mock-image.com/",
                PhoneNumber = "+123456789",
                DateOfBirth = "01 01 1999",
                SocialMediaUrl = "https://mock-media.com/",
                Biography = "Test user bio here.",
                IsPrivate = false,
            };

            var currentUserId = "current-user-id-again-and-again";
            this.SetCurrentUserId(currentUserId);

            var id = await this.profileService.Create(createProfileModel);

            var result = await this.profileService.Delete();
            result.Succeeded.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();

            var deletedProfile = await this.data
                .Profiles
                .FirstOrDefaultAsync(p => p.UserId == id);

            deletedProfile.Should().BeNull();
        }

        [Fact]
        public async Task Delete_ShouldReturnErrorMessage_WhenProfileDoesNotExist()
        {
            var createProfileModel = new CreateProfileServiceModel()
            {
                FirstName = "Test",
                LastName = "User",
                ImageUrl = "https://mock-image.com/",
                PhoneNumber = "+123456789",
                DateOfBirth = "01 01 1999",
                SocialMediaUrl = "https://mock-media.com/",
                Biography = "Test user bio here.",
                IsPrivate = false,
            };

            var currentUserId = "current-user-id-again-and-again";
            this.SetCurrentUserId(currentUserId);

            _ = await this.profileService.Create(createProfileModel);

            var invalidId = "invalidId";
            this.SetCurrentUserId(invalidId);

            var result = await this.profileService.Delete();
            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Be($"UserProfile with Id: {invalidId} was not found!");
        }

        [Fact]
        public async Task UpdateCount_ShouldUpdateProfileCount_WhenValidUserIdAndPropertyName()
        {
            var userId = "user1Id"; 
            var propertyName = "ReviewsCount"; 
            this.SetCurrentUserId(userId);

            var initialCount = await this.data.Profiles
                .Where(p => p.UserId == userId)
                .Select(p => p.ReviewsCount)
                .FirstOrDefaultAsync();

            await this.profileService.UpdateCount(userId, propertyName, count => count + 1);

            var updatedCount = await this.data.Profiles
                .Where(p => p.UserId == userId)
                .Select(p => p.ReviewsCount)
                .FirstOrDefaultAsync();

            updatedCount.Should().Be(initialCount + 1);
        }

        [Fact]
        public async Task UpdateCount_ShouldThrowDbEntityNotFoundException_WhenUserNotFound()
        {
            var invalidId = "invalid-id";
            this.SetCurrentUserId(invalidId);

            var propertyName = "ReviewsCount";

            var updateCountAsync = async () =>
            {
                await this.profileService.UpdateCount(invalidId, propertyName, x => ++x);
            };

            await updateCountAsync
                .Should()
                .ThrowAsync<DbEntityNotFoundException<string>>()
                .WithMessage($"UserProfile with Id: {invalidId} was not found!");
        }

        [Fact]
        public async Task UpdateCount_ShouldThrowArgumentException_WhenInvalidPropertyName()
        {
            var id = "user2Id";
            var propertyName = "InvalidPropName";
            this.SetCurrentUserId(id);

            var updateCountAsync = async () =>
            {
                await this.profileService.UpdateCount(id, propertyName, x => ++x);
            };

            await updateCountAsync
                .Should()
                .ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UpdateCount_ShouldNotChangeOtherProperties()
        {
            var userId = "user2Id";
            var propertyName = "ToReadBooksCount"; 
            this.SetCurrentUserId(userId);

            var initialToReadBooksCount = await this.data.Profiles
                .Where(p => p.UserId == userId)
                .Select(p => p.ToReadBooksCount)
                .FirstOrDefaultAsync();

            await this.profileService.UpdateCount(userId, propertyName, x => ++x);

            var updatedToReadBooksCount = await this.data.Profiles
                .Where(p => p.UserId == userId)
                .Select(p => p.ToReadBooksCount)
                .FirstOrDefaultAsync();

            updatedToReadBooksCount.Should().Be(initialToReadBooksCount + 1);

            var initialReviewsCount = await this.data.Profiles
                .Where(p => p.UserId == userId)
                .Select(p => p.ReviewsCount)
                .FirstOrDefaultAsync();

            var updatedReviewsCount = await this.data.Profiles
                .Where(p => p.UserId == userId)
                .Select(p => p.ReviewsCount)
                .FirstOrDefaultAsync();

            updatedReviewsCount.Should().Be(initialReviewsCount);
        }


        private void SetCurrentUserId(string userId)
            => this.mockUserService
                .Setup(x => x.GetId())
                .Returns(userId);

        private async Task PrepareDb()
        {
            if (!await this.data.Profiles.AnyAsync())
            {
                var users = new User[]
                {
                    new()
                    {
                        Id = "user1Id",
                        UserName = "user1name",
                        Email = "user1@mail.com"
                    },
                    new()
                    {
                        Id = "user2Id",
                        UserName = "user2name",
                        Email = "user2@mail.com"
                    },
                    new()
                    {
                        Id = "user3Id",
                        UserName = "user3name",
                        Email = "user3@mail.com"
                    },
                    new()
                    {
                        Id = "user4Id",
                        UserName = "user4name",
                        Email = "user4@mail.com"
                    },
                    new()
                    {
                        Id = "user5Id",
                        UserName = "user5name",
                        Email = "user5@mail.com"
                    },
                    new()
                    {
                        Id = "user6Id",
                        UserName = "user6name",
                        Email = "user6@mail.com"
                    }
                };

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
                    },
                    new()
                    {
                        UserId = "user4Id",
                        FirstName = "User",
                        LastName = "4 Name",
                        ImageUrl = "https://cdn1.iconfinder.com/data/icons/user-pictures/101/malecostume-512.png",
                        PhoneNumber = "4444444444",
                        DateOfBirth = new DateTime(2000, 11, 01),
                        SocialMediaUrl = "https://instagram.com/bobjohnson",
                        Biography = "user 4 bio is here.",
                        CreatedBooksCount = 0,
                        CreatedAuthorsCount = 0,
                        ReviewsCount = 14,
                        ReadBooksCount = 3,
                        ToReadBooksCount = 1,
                        CurrentlyReadingBooksCount = 5,
                        IsPrivate = true,
                    }
                };

                this.data.AddRange(users);
                this.data.AddRange(profiles);
                await this.data.SaveChangesAsync();
            }
        }
    }
}
