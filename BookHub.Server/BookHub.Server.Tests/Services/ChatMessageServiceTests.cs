namespace BookHub.Server.Tests.Services
{
    using AutoMapper;
    using Data;
    using Data.Models.Shared.ChatUser;
    using Features.Chat.Data.Models;
    using Features.Chat.Mapper;
    using Features.Chat.Service;
    using Features.Chat.Service.Models;
    using Features.Identity.Data.Models;
    using Features.Notification.Service;
    using Features.UserProfile.Data.Models;
    using Features.UserProfile.Service.Models;
    using FluentAssertions;
    using Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class ChatMessageServiceTests
    {
        private readonly BookHubDbContext data;

        private readonly Mock<ICurrentUserService> mockUserService;
        private readonly IMapper mapper;

        private readonly ChatMessageService chatMessageService;

        public ChatMessageServiceTests()
        {
            var options = new DbContextOptionsBuilder<BookHubDbContext>()
                .UseInMemoryDatabase(databaseName: "ChatMessageServiceInMemoryDatabase")
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

            this.mapper = new MapperConfiguration(cfg => cfg.AddProfile(new ChatMapper())).CreateMapper();

            this.chatMessageService = new ChatMessageService(
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
        public async Task CreateAsync_ShouldCreateMessage_AndAlso_ShouldReturnServiceModel()
        {
            var messageContent = "TESTTESTEST";

            var model = new CreateChatMessageServiceModel()
            {
                ChatId = 1,
                Message = messageContent
            };

            var resultServiceModel = await this.chatMessageService.CreateAsync(model);

            resultServiceModel.Should().NotBeNull();
            resultServiceModel.Id.Should().BeGreaterThan(0);
            resultServiceModel.Message.Should().Be(messageContent);
            resultServiceModel.SenderId.Should().Be("current-user-id");
            resultServiceModel.SenderName.Should().Be("Current User");
            resultServiceModel.CreatedOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            resultServiceModel.ModifiedOn.Should().BeNull();
        }

        [Fact]
        public async Task EditAsync_ShouldEditTheMessage_AndAlso_ShouldResultWithSuccessAndReturnServiceModel()
        {
            var messageContent = "TESTTESTEST";

            var createModel = new CreateChatMessageServiceModel()
            {
                ChatId = 1,
                Message = messageContent
            };

            var createdServiceModel = await this.chatMessageService.CreateAsync(createModel);

            var editModel = new CreateChatMessageServiceModel()
            {
                ChatId = 1,
                Message = "EDITEDITEDIT"
            };

            var result = await this.chatMessageService.EditAsync(createdServiceModel.Id, editModel);
            result.Succeeded.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();

            var editedServiceModel = result.Data!;
            editedServiceModel.Should().NotBeNull();
            editedServiceModel.Id.Should().BeGreaterThan(0);
            editedServiceModel.Message.Should().Be("EDITEDITEDIT");
            editedServiceModel.SenderId.Should().Be("current-user-id");
            editedServiceModel.SenderName.Should().Be("Current User");
            editedServiceModel.ModifiedOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public async Task EditAsync_ShouldReturnResultWithErrorMessage_IfMessageNotFound()
        {
            var messageContent = "TESTTESTEST";

            var createModel = new CreateChatMessageServiceModel()
            {
                ChatId = 1,
                Message = messageContent
            };

            _ = await this.chatMessageService.CreateAsync(createModel);

            var editModel = new CreateChatMessageServiceModel()
            {
                ChatId = 1,
                Message = "EDITEDITEDIT"
            };

            var invalidId = 421_325_903;
            var result = await this.chatMessageService.EditAsync(invalidId, editModel);
            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Be($"ChatMessage with Id: {invalidId} was not found!");
        }

        [Fact]
        public async Task EditAsync_ShouldReturnResultWithErrorMessage_IfCurrentUserIsNotMessageCreator()
        {
            var messageContent = "TESTTESTEST";

            var createModel = new CreateChatMessageServiceModel()
            {
                ChatId = 1,
                Message = messageContent
            };

            var createdServiceModel = await this.chatMessageService.CreateAsync(createModel);

            var editModel = new CreateChatMessageServiceModel()
            {
                ChatId = 1,
                Message = "EDITEDITEDIT"
            };

            this.SetCurrentUserId("fake-current-user");
            var result = await this.chatMessageService.EditAsync(createdServiceModel.Id, editModel);
            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Be($"current-user-username can not modify ChatMessage with Id: {createdServiceModel.Id}!");
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteTheMessage()
        {
            var messageContent = "TESTTESTEST";

            var createModel = new CreateChatMessageServiceModel()
            {
                ChatId = 1,
                Message = messageContent
            };

            var createdServiceModel = await this.chatMessageService.CreateAsync(createModel);

            var result = await this.chatMessageService.DeleteAsync(createdServiceModel.Id);
            result.Succeeded.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();

            var deletedMessage = await this.data
                .ChatMessages
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(m => m.Id == createdServiceModel.Id);

            deletedMessage!.IsDeleted.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnResultWithErrorMessage_IfMessageNotFound()
        {
            var messageContent = "TESTTESTEST";

            var createModel = new CreateChatMessageServiceModel()
            {
                ChatId = 1,
                Message = messageContent
            };

            _ = await this.chatMessageService.CreateAsync(createModel);

            var invalidId = 421_325_903;
            var result = await this.chatMessageService.DeleteAsync(invalidId);
            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Be($"ChatMessage with Id: {invalidId} was not found!");
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnResultWithErrorMessage_IfCurrentUserIsNotMessageCreator()
        {
            var messageContent = "TESTTESTEST";

            var createModel = new CreateChatMessageServiceModel()
            {
                ChatId = 1,
                Message = messageContent
            };

            var createdServiceModel = await this.chatMessageService.CreateAsync(createModel);

            this.SetCurrentUserId("fake-current-user");
            var result = await this.chatMessageService.DeleteAsync(createdServiceModel.Id);
            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Be($"current-user-username can not modify ChatMessage with Id: {createdServiceModel.Id}!");
        }

        private void SetCurrentUserId(string userId)
            => this.mockUserService
                .Setup(x => x.GetId())
                .Returns(userId);

        private async Task PrepareDbAsync()
        {
            if (!await this.data.Chats.AnyAsync())
            {
                var users = new User[]
                {
                    new()
                    {
                        Id = "current-user-id",
                        UserName = "current-user-username",
                        Email = "current@user.mail"
                    },
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
                    }
                };

                this.data.AddRange(users);

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
                        Biography = "Current user bio.",
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

                this.data.AddRange(profiles);

                var chats = new Chat[]
                {
                    new()
                    {
                        Id = 1,
                        Name = "Chat 1",
                        ImageUrl = "https://mock-image.com",
                        CreatorId = "user1Id"
                    },
                    new()
                    {
                        Id = 2,
                        Name = "Chat 2",
                        ImageUrl = "https://mock-image.com",
                        CreatorId = "user2Id"
                    },
                    new()
                    {
                        Id = 3,
                        Name = "Chat 3",
                        ImageUrl = "https://mock-image.com",
                        CreatorId = "user3Id"
                    },
                    new()
                    {
                        Id = 4,
                        Name = "Chat 4",
                        ImageUrl = "https://mock-image.com",
                        CreatorId = "current-user-id"
                    }
                };

                this.data.AddRange(chats);

                var chatsUsers = new ChatUser[]
                {
                    new()
                    {
                        ChatId = 1,
                        UserId = "user1Id",
                        HasAccepted = true
                    },
                    new()
                    {
                        ChatId = 1,
                        UserId = "user2Id",
                        HasAccepted = true
                    },
                    new()
                    {
                        ChatId = 1,
                        UserId = "user3Id",
                        HasAccepted = false
                    },

                    new()
                    {
                        ChatId = 2,
                        UserId = "user2Id",
                        HasAccepted = true
                    },
                    new()
                    {
                        ChatId = 2,
                        UserId = "user1Id",
                        HasAccepted = true
                    },
                    new()
                    {
                        ChatId = 2,
                        UserId = "user3Id",
                        HasAccepted = false
                    },

                    new()
                    {
                        ChatId = 3,
                        UserId = "user3Id",
                        HasAccepted = true
                    },
                    new()
                    {
                        ChatId = 3,
                        UserId = "user1Id",
                        HasAccepted = false
                    },
                    new()
                    {
                        ChatId = 3,
                        UserId = "user2Id",
                        HasAccepted = true
                    },

                    new()
                    {
                        ChatId = 4,
                        UserId = "current-user-id",
                        HasAccepted = true
                    },
                    new()
                    {
                        ChatId = 4,
                        UserId = "user1Id",
                        HasAccepted = true
                    },
                    new()
                    {
                        ChatId = 4,
                        UserId = "user2Id",
                        HasAccepted = false
                    }
                };

                this.data.AddRange(chatsUsers);

                var chatMessages = new ChatMessage[]
                {
                    new()
                    {
                        Id = 1,
                        ChatId = 1,
                        SenderId = "user1Id",
                        Message = "First for chat 1 from user1Id"
                    },
                    new()
                    {
                        Id = 2,
                        ChatId = 1,
                        SenderId = "user1Id",
                        Message = "Second for chat 1 from user1Id"
                    },
                    new()
                    {
                        Id = 3,
                        ChatId = 1,
                        SenderId = "user2Id",
                        Message = "First for chat 1 from user2Id"
                    },
                    new()
                    {
                        Id = 4,
                        ChatId = 2,
                        SenderId = "user2Id",
                        Message = "First for chat 2 from user2Id"
                    },
                    new()
                    {
                        Id = 5,
                        ChatId = 2,
                        SenderId = "user2Id",
                        Message = "Second for chat 2 from user2Id"
                    }
                    ,new()
                    {
                        Id = 6,
                        ChatId = 2,
                        SenderId = "user1Id",
                        Message = "First for chat 2 from user1Id"
                    },
                    new()
                    {
                        Id = 7,
                        ChatId = 3,
                        SenderId = "user3Id",
                        Message = "First for chat 3 from user3Id"
                    },
                    new()
                    {
                        Id = 8,
                        ChatId = 3,
                        SenderId = "user3Id",
                        Message = "Second for chat 3 from user3Id"
                    },
                    new()
                    {
                        Id = 9,
                        ChatId = 3,
                        SenderId = "user2Id",
                        Message = "First for chat 3 from user2Id"
                    },
                    new()
                    {
                        Id = 10,
                        ChatId = 4,
                        SenderId = "current-user-id",
                        Message = "First for chat 4 from current-user-id"
                    },
                    new()
                    {
                        Id = 11,
                        ChatId = 4,
                        SenderId = "current-user-id",
                        Message = "Second for chat 4 from current-user-id"
                    },
                    new()
                    {
                        Id = 12,
                        ChatId = 4,
                        SenderId = "user1Id",
                        Message = "First for chat 3 from user1Id"
                    }
                };

                this.data.AddRange(chatMessages);

                await this.data.SaveChangesAsync();
            }
        }
    }
}
