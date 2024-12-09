namespace BookHub.Server.Tests.Services
{
    using AutoMapper;
    using Data;
    using Features.Notification.Data.Models;
    using Features.Notification.Mapper;
    using Features.Notification.Service;
    using Features.Notification.Service.Models;
    using FluentAssertions;
    using Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class NotificationServiceTests
    {
        private readonly BookHubDbContext data;

        private readonly Mock<ICurrentUserService> mockUserService;
        private readonly IMapper mapper;

        private readonly NotificationService notificationService;

        public NotificationServiceTests()
        {
            var options = new DbContextOptionsBuilder<BookHubDbContext>()
                .UseInMemoryDatabase(databaseName: "NotificationServiceInMemoryDatabase")
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

            this.mapper = new MapperConfiguration(cfg => cfg.AddProfile(new NotificationMapper())).CreateMapper();

            this.notificationService = new NotificationService(
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
        public async Task LastThreeAsync_ShouldReturnCollection_FromServiceModels()
        {
            var notifications = await this.notificationService.LastThreeAsync();

            notifications.Should().NotBeEmpty();
            notifications.Should().AllBeOfType(typeof(NotificationServiceModel));
            notifications.Count().Should().Be(3);
        }

        [Fact]
        public async Task LastThreeAsync_ShouldReturn_OnlyCurrentUserNotifications()
        {
            var notifications = await this.notificationService.LastThreeAsync();

            notifications.Should().OnlyContain(n => n.Message == "current-user message");
            notifications.Should().NotContain(n => n.Message == "mock message");
        }

        [Fact]
        public async Task LastThreeAsync_ShouldReturn_CorrectlyOrderedElements()
        {
            var notifications = await this.notificationService.LastThreeAsync();

            notifications
                .First()
                .CreatedOn
                .Should()
                .BeAfter(notifications.Skip(1).First().CreatedOn);

            notifications
                .Skip(1)
                .First()
                .CreatedOn
                .Should()
                .BeAfter(notifications.Skip(2).First().CreatedOn);
        }

        [Fact]
        public async Task AllAsync_ShouldReturnPaginatedNotifications_WithCorrectFilteringAndSorting()
        {
            var pageIndex = 1;
            var pageSize = 2;

            var result = await this.notificationService.AllAsync(pageIndex, pageSize);

            result.Items.Should().HaveCount(pageSize);
            result.Items.Should().OnlyContain(n => n.Message == "current-user message");  
            result.TotalItems.Should().Be(3); 

            result.Items.First().IsRead.Should().BeFalse();
            result
                .Items
                .First()
                .CreatedOn
                .Should()
                .BeAfter(result.Items.Skip(1).First().CreatedOn);
        }

        [Fact]
        public async Task CreateOnEntityCreationAsync_ShouldCreateNotification_WithCorrectData()
        {
            var resourceId = 1;
            var resourceType = "Book";
            var nameProp = "Pet Sematary";
            var receiverId = "receiver-id";

            var id = await this.notificationService.CreateOnEntityCreationAsync(
                resourceId,
                resourceType,
                nameProp,
                receiverId);

            var notification = await this.data.Notifications.FindAsync(id);
            notification.Should().NotBeNull();
            notification!.ResourceId.Should().Be(resourceId);
            notification!.ResourceType.Should().Be(resourceType);
            notification!.ReceiverId.Should().Be(receiverId);
            notification!.Message.Should().Be("current-user-username has created 'Pet Sematary'");
        }

        [Fact]
        public async Task CreateOnEntityApprovalStatusChangeAsync_ShouldCreateNotification_WithCorrectData_WhenIsApproved()
        {
            var resourceId = 1;
            var resourceType = "Book";
            var nameProp = "Pet Sematary";
            var receiverId = "receiver-id";
            var isApproved = true; 

            var id = await this.notificationService.CreateOnEntityApprovalStatusChangeAsync(
                resourceId,
                resourceType,
                nameProp,
                receiverId,
                isApproved);

            var notification = await this.data.Notifications.FindAsync(id);
            notification.Should().NotBeNull();
            notification!.ResourceId.Should().Be(resourceId);
            notification!.ResourceType.Should().Be(resourceType);
            notification!.ReceiverId.Should().Be(receiverId);
            notification!.Message.Should().Be("'Pet Sematary' has been approved");
        }

        [Fact]
        public async Task CreateOnEntityApprovalStatusChangeAsync_ShouldCreateNotification_WithCorrectData_WhenIsRejected()
        {
            var resourceId = 1;
            var resourceType = "Book";
            var nameProp = "Pet Sematary";
            var receiverId = "receiver-id";
            var isApproved = false;

            var id = await this.notificationService.CreateOnEntityApprovalStatusChangeAsync(
                resourceId,
                resourceType,
                nameProp,
                receiverId,
                isApproved);

            var notification = await this.data.Notifications.FindAsync(id);
            notification.Should().NotBeNull();
            notification!.ResourceId.Should().Be(resourceId);
            notification!.ResourceType.Should().Be(resourceType);
            notification!.ReceiverId.Should().Be(receiverId);
            notification!.Message.Should().Be("'Pet Sematary' has been rejected");
        }

        [Fact]
        public async Task CreateOnChatInvitationAsync_ShouldCreateNotification_WithCorrectData()
        {
            var chatId = 1;
            var chatName = "My Chat";
            var receiverId = "receiver-id";

            var id = await this.notificationService.CreateOnChatInvitationAsync(
                chatId,
                chatName,
                receiverId);

            var notification = await this.data.Notifications.FindAsync(id);
            notification.Should().NotBeNull();
            notification!.ResourceId.Should().Be(chatId);
            notification!.ReceiverId.Should().Be(receiverId);
            notification!.Message.Should().Be("current-user-username has invited you to join in 'My Chat'");
        }

        [Fact]
        public async Task CreateOnChatInvitationStatusChangedAsync_ShouldCreateNotification_WithCorrectData_WhenStatusIsAccepted()
        {
            var chatId = 1;
            var chatName = "My Chat";
            var receiverId = "receiver-id";
            var hasAccepted = true;

            var id = await this.notificationService.CreateOnChatInvitationStatusChangedAsync(
                chatId,
                chatName,
                receiverId,
                hasAccepted);

            var notification = await this.data.Notifications.FindAsync(id);
            notification.Should().NotBeNull();
            notification!.ResourceId.Should().Be(chatId);
            notification!.ReceiverId.Should().Be(receiverId);
            notification!.Message.Should().Be("current-user-username has accepted to join in 'My Chat'");
        }

        [Fact]
        public async Task CreateOnChatInvitationStatusChangedAsync_ShouldCreateNotification_WithCorrectData_WhenStatusIsNotAccepted()
        {
            var chatId = 1;
            var chatName = "My Chat";
            var receiverId = "receiver-id";
            var hasAccepted = false;

            var id = await this.notificationService.CreateOnChatInvitationStatusChangedAsync(
                chatId,
                chatName,
                receiverId,
                hasAccepted);

            var notification = await this.data.Notifications.FindAsync(id);
            notification.Should().NotBeNull();
            notification!.ResourceId.Should().Be(chatId);
            notification!.ReceiverId.Should().Be(receiverId);
            notification!.Message.Should().Be("current-user-username has rejected to join in 'My Chat'");
        }

        [Fact]
        public async Task MarkAsReadAsync_ShouldReturnErrorMessage_IfIdIsNotValid()
        {
            var chatId = 1;
            var chatName = "My Chat";
            var receiverId = "receiver-id";
            var hasAccepted = false;

            _ = await this.notificationService.CreateOnChatInvitationStatusChangedAsync(
                chatId,
                chatName,
                receiverId,
                hasAccepted);

            var invalidId = 129_213_972;

            var result = await this.notificationService.MarkAsReadAsync(invalidId);
            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Be($"Notification with Id: {invalidId} was not found!");
        }

        [Fact]
        public async Task MarkAsReadAsync_ShouldReturnErrorMessage_IfIdIsValid()
        {
            var chatId = 1;
            var chatName = "My Chat";
            var receiverId = "receiver-id";
            var hasAccepted = false;

            var id = await this.notificationService.CreateOnChatInvitationStatusChangedAsync(
                chatId,
                chatName,
                receiverId,
                hasAccepted);

            var result = await this.notificationService.MarkAsReadAsync(id);
            result.Succeeded.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();

            var notification = await this.data.Notifications.FindAsync(id);
            notification!.IsRead.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnErrorMessage_IfIdIsNotValid()
        {
            var chatId = 1;
            var chatName = "My Chat";
            var receiverId = "receiver-id";
            var hasAccepted = false;

            _ = await this.notificationService.CreateOnChatInvitationStatusChangedAsync(
                chatId,
                chatName,
                receiverId,
                hasAccepted);

            var invalidId = 129_213_972;

            var result = await this.notificationService.DeleteAsync(invalidId);
            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Be($"Notification with Id: {invalidId} was not found!");
        }

        [Fact]
        public async Task DeleteAsync_ShouldSetIsDeletedToTrue_IfIdIsValid()
        {
            var chatId = 1;
            var chatName = "My Chat";
            var receiverId = "receiver-id";
            var hasAccepted = false;

            var id = await this.notificationService.CreateOnChatInvitationStatusChangedAsync(
                chatId,
                chatName,
                receiverId,
                hasAccepted);

            var result = await this.notificationService.DeleteAsync(id);
            result.Succeeded.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();

            var notification = await this.data
                .Notifications
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(n => n.Id == id);

            notification!.IsDeleted.Should().BeTrue();
        }

        private async Task PrepareDbAsync()
        {
            if (await this.data.Notifications.AnyAsync())
            {
                return;
            }

            var notifications = new Notification[]
            {
                new()
                {
                    Id = 1,
                    ResourceId = 1,
                    ResourceType = "Book",
                    ReceiverId = "current-user-id",
                    Message = "current-user message",
                },
                new()
                {
                    Id = 2,
                    ResourceId = 1,
                    ResourceType = "Book",
                    ReceiverId = "other-user-id",
                    Message = "mock message",
                },
                new()
                {
                    Id = 3,
                    ResourceId = 1,
                    ResourceType = "Book",
                    ReceiverId = "current-user-id",
                    Message = "current-user message",
                },
                new()
                {
                    Id = 4,
                    ResourceId = 1,
                    ResourceType = "Book",
                    ReceiverId = "other-user-id",
                    Message = "mock message",
                },
                new()
                {
                    Id = 5,
                    ResourceId = 1,
                    ResourceType = "Book",
                    ReceiverId = "current-user-id",
                    Message = "current-user message",
                },
                new()
                {
                    Id = 6,
                    ResourceId = 1,
                    ResourceType = "Book",
                    ReceiverId = "other-user-id",
                    Message = "mock message",
                }
            };

            this.data.AddRange(notifications);
            await this.data.SaveChangesAsync();
        }
    }
}
