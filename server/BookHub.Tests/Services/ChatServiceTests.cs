namespace BookHub.Tests.Services
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

    public class ChatServiceTests
    {
        private readonly BookHubDbContext data;

        private readonly Mock<ICurrentUserService> mockUserService;
        private readonly Mock<INotificationService> mockNotificationService;
        private readonly IMapper mapper;

        private readonly ChatService chatService;

        public ChatServiceTests()
        {
            var options = new DbContextOptionsBuilder<BookHubDbContext>()
                .UseInMemoryDatabase(databaseName: "ChatServiceInMemoryDatabase")
                .Options;

            this.mockUserService = new Mock<ICurrentUserService>();
            this.mockNotificationService = new Mock<INotificationService>();

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

            this.chatService = new ChatService(
                this.data,
                this.mockUserService.Object,
                this.mockNotificationService.Object,
                this.mapper
            );

            Task
                .Run(this.PrepareDb)
                .GetAwaiter()
                .GetResult();
        }

        [Fact]
        public async Task NotJoined_ShouldReturnCollectionFromServiceModels_WhereUserHasNotJoined()
        {
            var userToJoinId = "user3Id";

            var result = await this.chatService.NotJoined(userToJoinId);
            result.Should().NotBeNull();
            result.Should().AllBeOfType(typeof(ChatServiceModel));
            result.Should().Contain(c => c.Id == 4);
            result.Should().Contain(c => c.Name == "Chat 4");
        }

        [Fact]
        public async Task NotJoined_ShouldNotReturn_ChatsWhereUserJoined()
        {
            var userToJoinId = "user3Id";

            var result = await this.chatService.NotJoined(userToJoinId);
            result.Should().NotBeNull();
            result.Should().NotContain(c => c.Id == 1 && c.Id == 2 && c.Id == 3);
        }

        [Fact]
        public async Task CanAccessChat_ShouldReturn_True_WhenTheUserIsMemberOfTheChat()
        {
            var chatId = 1;
            var userId = "user1Id"; 

            var result = await this.chatService.CanAccessChat(chatId, userId);
            result.Should().BeTrue();
        }

        [Fact]
        public async Task CanAccessChat_ShouldReturnFalse_WhenTheUserIsNotMemberOfTheChat()
        {
            var chatId = 1;
            var userId = "current-user-id";

            var result = await this.chatService.CanAccessChat(chatId, userId);
            result.Should().BeFalse();
        }

        [Fact]
        public async Task CanAccessChat_ShouldReturnFalse_WhenChatDoesNotExist()
        {
            var invalidChatId = 214_325_945; 
            var userId = "user1Id"; 

            var result = await this.chatService.CanAccessChat(invalidChatId, userId);
            result.Should().BeFalse();
        }

        [Fact]
        public async Task CanAccessChat_ShouldReturn_False_WhenUserDoesNotExist()
        {
            var chatId = 1; 
            var userId = "invalid-user-id"; 

            var result = await this.chatService.CanAccessChat(chatId, userId);
            result.Should().BeFalse();
        }

        [Fact]
        public async Task IsInvited_ShouldReturnTrue_WhenUserIsInvitedButHasNotAccepted()
        {
            var chatId = 1; 
            var userId = "user3Id"; 

            var result = await this.chatService.IsInvited(chatId, userId);
            result.Should().BeTrue();
        }

        [Fact]
        public async Task IsInvited_ShouldReturnFalse_WhenUserIsNotInvited()
        {
            var chatId = 2;
            var userId = "current-user-id";

            var result = await this.chatService.IsInvited(chatId, userId);
            result.Should().BeFalse();
        }

        [Fact]
        public async Task IsInvited_ShouldReturn_FalseWhenUserHasAlreadyAcceptedInvitation()
        {
            var chatId = 1; 
            var userId = "user2Id"; 

            var result = await this.chatService.IsInvited(chatId, userId);
            result.Should().BeFalse();
        }

        [Fact]
        public async Task IsInvited_ShouldReturn_False_WhenChatDoesNotExist()
        {
            var invalidChatId = 214_325_945;
            var userId = "user3Id";

            var result = await this.chatService.IsInvited(invalidChatId, userId);
            result.Should().BeFalse();
        }

        [Fact]
        public async Task IsInvited_ShouldReturn_False_WhenUserDoesNotExist()
        {
            var chatId = 1; 
            var userId = "invalid-user-id"; 

            var result = await this.chatService.IsInvited(chatId, userId);
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Details_ShouldReturnChatDetailsServiceModel_WhenChatExists()
        {
            var chatId = 1;

            var result = await this.chatService.Details(chatId);
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(ChatDetailsServiceModel));
            result!.Id.Should().Be(1);
            result.Name.Should().Be("Chat 1");
            result.ImageUrl.Should().Be("https://mock-image.com");
            result.CreatorId.Should().Be("user1Id");
        }

        [Fact]
        public async Task Details_ShouldReturnNull_WhenChatDoesNotExist()
        {
            var invalidChatId = 214_325_945;

            var result = await this.chatService.Details(invalidChatId);
            result.Should().BeNull();
        }

        [Fact]
        public async Task Create_ShouldCreateChat_WithValidData()
        {
            var chatModel = new CreateChatServiceModel()
            {
                Name = "Chat 5",
                ImageUrl = "https://mock-chat-image"
            };

            var createdChatId = await this.chatService.Create(chatModel);

            var createdChat = await this.data.Chats.FindAsync(createdChatId);

            createdChat.Should().NotBeNull();
            createdChat!.Name.Should().Be("Chat 5");
            createdChat.ImageUrl.Should().Be("https://mock-chat-image");
            createdChat.CreatorId.Should().Be("current-user-id");
        }

        [Fact]
        public async Task Create_ShouldUseDefaultImageUrl_WhenImageUrlIsNull()
        {
            var createModel = new CreateChatServiceModel() { Name = "Chat 5" };

            var createdChatId = await this.chatService.Create(createModel);

            var createdChat = await this.data.Chats.FindAsync(createdChatId);
            createdChat.Should().NotBeNull();
            createdChat!.Name.Should().Be("Chat 5");
            createdChat.ImageUrl.Should().Be("https://pushfestival.ca/2015/wp-content/uploads/blogger/-rqsdeqC0mpU/UG5c0Xwk9hI/AAAAAAAAA7g/Q9psMuS468M/s1600/LiesbethBernaerts_HumanLibrary.jpg");
            createdChat.CreatorId.Should().Be("current-user-id");
        }

        [Fact]
        public async Task Create_ShouldAddCreatorAsParticipant()
        {
            var chatModel = new CreateChatServiceModel() { Name = "Chat 5" };

            var createdChatId = await this.chatService.Create(chatModel);

            var creator = await this.data
                .ChatsUsers
                .FirstOrDefaultAsync(cu => cu.ChatId == createdChatId && cu.UserId == "current-user-id");

            creator.Should().NotBeNull();
            creator!.HasAccepted.Should().BeTrue();
        }


        [Fact]
        public async Task Edit_ShouldUpdateChat_AndAlso_ReturnResultWithSuccess_WhenCalledByCreator()
        {
            var createModel = new CreateChatServiceModel() { Name = "Chat 5" };
            var id = await this.chatService.Create(createModel);

            var editModel = new CreateChatServiceModel()
            {
                Name = "Edit Name",
                ImageUrl = "https://mock-edited.com"
            };

            var result = await this.chatService.Edit(id, editModel);
            result.Succeeded.Should().BeTrue();

            var editedChat = await this.data
                .Chats
                .FindAsync(id);

            editedChat!.Should().NotBeNull();
            editedChat!.Name.Should().Be(editModel.Name);
            editedChat!.ImageUrl.Should().Be(editModel.ImageUrl);
        }

        [Fact]
        public async Task Edit_ShouldUseDefaultImage_WhenImageUrlIsNotProvided()
        {
            var createModel = new CreateChatServiceModel() { Name = "Chat 5" };
            var id = await this.chatService.Create(createModel);

            var editModel = new CreateChatServiceModel() { Name = "Edit Name" };

            _ = await this.chatService.Edit(id, editModel);
            var updatedChat = await this.data.Chats.FindAsync(id);
            updatedChat.Should().NotBeNull();
            updatedChat!.ImageUrl.Should().Be("https://pushfestival.ca/2015/wp-content/uploads/blogger/-rqsdeqC0mpU/UG5c0Xwk9hI/AAAAAAAAA7g/Q9psMuS468M/s1600/LiesbethBernaerts_HumanLibrary.jpg");
        }

        [Fact]
        public async Task Edit_ShouldReturnResultWithErrorMessage_WhenChatDoesNotExists()
        {
            var createModel = new CreateChatServiceModel() { Name = "Chat 5" };
            _ = await this.chatService.Create(createModel);

            var editModel = new CreateChatServiceModel()
            {
                Name = "Edit Name",
                ImageUrl = "https://mock-edited.com"
            };

            this.SetCurrentUserId("fake-id");

            var invalidId = 124_623_219;
            var result = await this.chatService.Edit(invalidId, editModel);
            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Be($"Chat with Id: {invalidId} was not found!");
        }

        [Fact]
        public async Task Edit_ShouldReturnResultWithErrorMessage_WhenCalledByNonCreator()
        {
            var createModel = new CreateChatServiceModel() { Name = "Chat 5" };
            var id = await this.chatService.Create(createModel);

            var editModel = new CreateChatServiceModel()
            {
                Name = "Edit Name",
                ImageUrl = "https://mock-edited.com"
            };

            this.SetCurrentUserId("fake-id");

            var result = await this.chatService.Edit(id, editModel);
            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Be($"current-user-username can not modify Chat with Id: {id}!");
        }

        [Fact]
        public async Task Delete_ShouldDeleteChat_WhenCalledByCreator()
        {
            var createModel = new CreateChatServiceModel() { Name = "Chat 5" };
            var id = await this.chatService.Create(createModel);

            var result = await this.chatService.Delete(id);

            result.Succeeded.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();

            var deletedChat = await this.data
                .Chats
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(c => c.Id == id);

            deletedChat!.IsDeleted.Should().BeTrue();
        }

        [Fact]
        public async Task Delete_ShouldDeleteChat_WhenCalledByAdmin()
        {
            var createModel = new CreateChatServiceModel() { Name = "Chat 5" };
            var id = await this.chatService.Create(createModel);

            this.SetCurrentUserId("fake-id");
            this.SetCurrentUserIsAdmin(true);

            var result = await this.chatService.Delete(id);

            result.Succeeded.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();

            var deletedChat = await this.data
              .Chats
              .IgnoreQueryFilters()
              .FirstOrDefaultAsync(c => c.Id == id);

            deletedChat!.IsDeleted.Should().BeTrue();
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenChatDoesNotExist()
        {
            var createModel = new CreateChatServiceModel() { Name = "Chat 5" };
            _ = await this.chatService.Create(createModel);

            var invalidId = 412_341_532;
            var result = await this.chatService.Delete(invalidId);

            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Be($"Chat with Id: {invalidId} was not found!");
        }

        [Fact]
        public async Task Delete_ShouldReturnResultWithErrorMessage_WhenCalledByNonCreatorAndNonAdmin()
        {
            var createModel = new CreateChatServiceModel() { Name = "Chat 5" };
            var id = await this.chatService.Create(createModel);

            this.SetCurrentUserId("fake-id");

            var result = await this.chatService.Delete(id);

            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Be($"current-user-username can not modify Chat with Id: {id}!");
        }

        [Fact]
        public async Task InviteUserToChat_ShouldCreateMapEntity()
        {
            var createModel = new CreateChatServiceModel() { Name = "Chat 5" };
            var chatId = await this.chatService.Create(createModel);

            var userId = "fake-id";

            var result = await this.chatService.InviteUserToChat(chatId, createModel.Name, userId);
            result.Succeeded.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();

            var mapEntity = await this.data
                .ChatsUsers
                .FirstOrDefaultAsync(cu => cu.UserId == userId && cu.ChatId == chatId);

            mapEntity.Should().NotBeNull();
            mapEntity!.UserId.Should().Be(userId);
            mapEntity.ChatId.Should().Be(chatId);
            mapEntity.HasAccepted.Should().BeFalse();
        }

        [Fact]
        public async Task InviteUserToChat_ShouldReturnResultWithError_IfCurrentUserIsNotTheChatCreator()
        {
            var createModel = new CreateChatServiceModel() { Name = "Chat 5" };
            var chatId = await this.chatService.Create(createModel);

            this.SetCurrentUserId("fake-current");
            var userId = "fake-id";

            var result = await this.chatService.InviteUserToChat(chatId, createModel.Name, userId);
            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Be($"current-user-username can not modify Chat with Id: {chatId}!");

            var mapEntity = await this.data
                .ChatsUsers
                .FirstOrDefaultAsync(cu => cu.UserId == userId && cu.ChatId == chatId);

            mapEntity.Should().BeNull();
        }

        [Fact]
        public async Task Accept_ShouldReturnPrivateProfileServiceModel_AndAlso_ShouldSetHasAcceptedToTrueInTheMapEntity()
        {
            var creatorId = "current-user-id";

            var createModel = new CreateChatServiceModel() { Name = "Chat 5" };
            var chatId = await this.chatService.Create(createModel);

            var userToInviteId = "user1Id";
            await this.chatService.InviteUserToChat(chatId, createModel.Name, userToInviteId);

            this.SetCurrentUserId(userToInviteId);

            var result = await this.chatService.Accept(chatId, createModel.Name, creatorId);
            result.Succeeded.Should().BeTrue();
            result!.Data!.Id.Should().Be("user1Id");
            result!.Data!.Should().BeOfType(typeof(PrivateProfileServiceModel));
            result!.Data!.FirstName.Should().Be("John");

            var mapEntity = await this.data
                .ChatsUsers
                .FirstOrDefaultAsync(cu => cu.UserId == userToInviteId && cu.ChatId == chatId);

            mapEntity.Should().NotBeNull();
            mapEntity!.UserId.Should().Be(userToInviteId);
            mapEntity.ChatId.Should().Be(chatId);
            mapEntity.HasAccepted.Should().BeTrue();
        }

        [Fact]
        public async Task Accept_ShouldReturnResultWithErrorMessage_IfMapEntityNotFound()
        {
            var creatorId = "current-user-id";

            var createModel = new CreateChatServiceModel() { Name = "Chat 5" };
            var chatId = await this.chatService.Create(createModel);

            var userToInviteId = "user1Id";
            await this.chatService.InviteUserToChat(chatId, createModel.Name, userToInviteId);

            var invalidUserId = "invalid-user-id";
            this.SetCurrentUserId(invalidUserId);

            var result = await this.chatService.Accept(chatId, createModel.Name, creatorId);
            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Be($"ChatUser with Id: {chatId}-{invalidUserId} was not found!");
        }

        [Fact]
        public async Task Reject_ShouldReturnResultWithSuccess_AndAlso_ShouldDeleteTheMapEntity()
        {
            var creatorId = "current-user-id";

            var createModel = new CreateChatServiceModel() { Name = "Chat 5" };
            var chatId = await this.chatService.Create(createModel);

            var userToInviteId = "user1Id";
            await this.chatService.InviteUserToChat(chatId, createModel.Name, userToInviteId);

            this.SetCurrentUserId(userToInviteId);

            var result = await this.chatService.Reject(chatId, createModel.Name, creatorId);
            result.Succeeded.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();

            var mapEntity = await this.data
                .ChatsUsers
                .FirstOrDefaultAsync(cu => cu.UserId == userToInviteId && cu.ChatId == chatId);

            mapEntity.Should().BeNull();
        }

        [Fact]
        public async Task Reject_ShouldReturnResultWithErrorMessage_IfMapEntityNotFound()
        {
            var creatorId = "current-user-id";

            var createModel = new CreateChatServiceModel() { Name = "Chat 5" };
            var chatId = await this.chatService.Create(createModel);

            var userToInviteId = "user1Id";
            await this.chatService.InviteUserToChat(chatId, createModel.Name, userToInviteId);

            var invalidUserId = "invalid-user-id";
            this.SetCurrentUserId(invalidUserId);

            var result = await this.chatService.Reject(chatId, createModel.Name, creatorId);
            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Be($"ChatUser with Id: {chatId}-{invalidUserId} was not found!");
        }

        [Fact]
        public async Task RemoveUserFromChat_ShouldReturnResultWithSuccess_AndAlso_ShouldDeleteTheMapEntity_IfCurrentUserIsTheChatCreator()
        {
            var creatorId = "current-user-id";

            var createModel = new CreateChatServiceModel() { Name = "Chat 5" };
            var chatId = await this.chatService.Create(createModel);

            var userToInviteId = "user1Id";
            await this.chatService.InviteUserToChat(chatId, createModel.Name, userToInviteId);

            this.SetCurrentUserId(userToInviteId);
            _ = await this.chatService.Accept(chatId, createModel.Name, creatorId);

            this.SetCurrentUserId(creatorId);
            var result = await this.chatService.RemoveUserFromChat(chatId, userToInviteId);
            result.Succeeded.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();

            var mapEntity = await this.data
                .ChatsUsers
                .FirstOrDefaultAsync(cu => cu.UserId == userToInviteId && cu.ChatId == chatId);

            mapEntity.Should().BeNull();
        }

        [Fact]
        public async Task RemoveUserFromChat_ShouldReturnResultWithSuccess_AndAlso_ShouldDeleteTheMapEntity_IfCurrentUserWantsToLeaveTheChat()
        {
            var creatorId = "current-user-id";

            var createModel = new CreateChatServiceModel() { Name = "Chat 5" };
            var chatId = await this.chatService.Create(createModel);

            var userToInviteId = "user1Id";
            await this.chatService.InviteUserToChat(chatId, createModel.Name, userToInviteId);

            this.SetCurrentUserId(userToInviteId);
            _ = await this.chatService.Accept(chatId, createModel.Name, creatorId);

            this.SetCurrentUserId(userToInviteId);
            var result = await this.chatService.RemoveUserFromChat(chatId, userToInviteId);
            result.Succeeded.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();

            var mapEntity = await this.data
                .ChatsUsers
                .FirstOrDefaultAsync(cu => cu.UserId == userToInviteId && cu.ChatId == chatId);

            mapEntity.Should().BeNull();
        }

        [Fact]
        public async Task RemoveUserFromChat_ShouldReturnResultWithErrorMessage_IfCurrentUserIsNotTheChatCreator_AndAlso_IsNotCurrentUserIsNoTryingToLeaveTheChat()
        {
            var creatorId = "current-user-id";

            var createModel = new CreateChatServiceModel() { Name = "Chat 5" };
            var chatId = await this.chatService.Create(createModel);

            var userToInviteId = "user1Id";
            await this.chatService.InviteUserToChat(chatId, createModel.Name, userToInviteId);

            this.SetCurrentUserId(userToInviteId);
            _ = await this.chatService.Accept(chatId, createModel.Name, creatorId);

            this.SetCurrentUserId("fake-current");
            var result = await this.chatService.RemoveUserFromChat(chatId, userToInviteId);
            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Be($"current-user-username can not modify Chat with Id: {chatId}!");

            var mapEntity = await this.data
                .ChatsUsers
                .FirstOrDefaultAsync(cu => cu.UserId == userToInviteId && cu.ChatId == chatId);

            mapEntity.Should().NotBeNull();
        }

        private void SetCurrentUserId(string userId)
            => this.mockUserService
                .Setup(x => x.GetId())
                .Returns(userId);
      
        private void SetCurrentUserIsAdmin(bool isAdmin)
           => this.mockUserService
               .Setup(x => x.IsAdmin())
               .Returns(isAdmin);

        private async Task PrepareDb()
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
