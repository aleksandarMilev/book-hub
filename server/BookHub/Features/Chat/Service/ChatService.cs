namespace BookHub.Features.Chat.Service
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using BookHub.Data;
    using BookHub.Data.Models.Shared.ChatUser;
    using Infrastructure.Services.CurrentUser;
    using Infrastructure.Services.Result;
    using Data.Models;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Notification.Service;
    using Service.Models;
    using UserProfile.Service.Models;

    using static Common.Constants.ErrorMessages;

    public class ChatService(
        BookHubDbContext data,
        ICurrentUserService userService,
        INotificationService notificationService,
        IMapper mapper) : IChatService
    {
        private const string DefaultImageUrl = "https://pushfestival.ca/2015/wp-content/uploads/blogger/-rqsdeqC0mpU/UG5c0Xwk9hI/AAAAAAAAA7g/Q9psMuS468M/s1600/LiesbethBernaerts_HumanLibrary.jpg";

        private readonly BookHubDbContext data = data;
        private readonly ICurrentUserService userService = userService;
        private readonly INotificationService notificationService = notificationService;
        private readonly IMapper mapper = mapper;

        public async Task<ChatDetailsServiceModel?> Details(int id)
            => await this.data
                .Chats
                .ProjectTo<ChatDetailsServiceModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<IEnumerable<ChatServiceModel>> NotJoined(string userToJoinId)
            => await this.data
                .Chats
                .Where(c => 
                    c.CreatorId == this.userService.GetId() && 
                    !c.ChatsUsers.Any(cu => cu.UserId == userToJoinId))
                .ProjectTo<ChatServiceModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<bool> CanAccessChat(int chatId, string userId)
            => await this.data
                .ChatsUsers
                .AnyAsync(cu => cu.UserId == userId && cu.ChatId == chatId);

        public async Task<bool> IsInvited(int chatId, string userId)
              => await this.data
                  .ChatsUsers
                  .AnyAsync(cu => 
                      cu.UserId == userId && 
                      cu.ChatId == chatId &&
                      !cu.HasAccepted);

        public async Task<int> Create(CreateChatServiceModel model)
        {
            var creatorId = this.userService.GetId()!;

            model.ImageUrl ??= DefaultImageUrl;

            var chat = this.mapper.Map<Chat>(model);
            chat.CreatorId = creatorId;

            this.data.Add(chat);
            await this.data.SaveChangesAsync();

            _ = await this.CreateChatUserEntity(
                chat.Id,
                creatorId,
                true);

            return chat.Id;
        }

        public async Task<Result> Edit(int id, CreateChatServiceModel model)
        {
            var chat = await this.data
               .Chats
               .FindAsync(id);

            if (chat is null)
            {
                return string.Format(
                    DbEntityNotFound,
                    nameof(Chat),
                    id);
            }

            if (chat.CreatorId != this.userService.GetId())
            {
                return string.Format(
                    UnauthorizedDbEntityAction,
                    this.userService.GetUsername(),
                    nameof(Chat), 
                    id);
            }

            model.ImageUrl ??= DefaultImageUrl;
            this.mapper.Map(model, chat);

            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<Result> Delete(int id)
        {
            var chat = await this.data
                .Chats
                .FindAsync(id);

            if (chat is null)
            {
                return string.Format(
                    DbEntityNotFound,
                    nameof(Chat),
                    id);
            }

            if (chat.CreatorId != this.userService.GetId() &&
                !this.userService.IsAdmin())
            {
                return string.Format(
                    UnauthorizedDbEntityAction,
                    this.userService.GetUsername(),
                    nameof(Chat),
                    id);
            }

            this.data.Remove(chat);
            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<ResultWith<PrivateProfileServiceModel>> Accept(
            int chatId,
            string chatName,
            string chatCreatorId)
        {
            var invitedUserId = this.userService.GetId()!;

            var mapEntity = await this.data
                .ChatsUsers
                .FirstOrDefaultAsync(cu => cu.UserId == invitedUserId && cu.ChatId == chatId);

            if (mapEntity is null)
            {
                return string.Format(
                    DbEntityNotFound,
                    nameof(ChatUser),
                    $"{chatId}-{invitedUserId}");
            }

            mapEntity.HasAccepted = true;
            await this.data.SaveChangesAsync();

            _ = await this.notificationService
                .CreateOnChatInvitationStatusChanged(
                    chatId,
                    chatName,
                    chatCreatorId,
                    true);

            var profileModel = await this.data
                .Profiles
                .Where(p => p.UserId == invitedUserId)
                .ProjectTo<PrivateProfileServiceModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return ResultWith<PrivateProfileServiceModel>
                .Success(profileModel!);
        }

        public async Task<Result> Reject(
            int chatId,
            string chatName,
            string chatCreatorId)
        {
            var invitedUserId = this.userService.GetId();

            var mapEntity = await this.data
               .ChatsUsers
               .FirstOrDefaultAsync(cu => 
                    cu.UserId == invitedUserId && 
                    cu.ChatId == chatId);

            if (mapEntity is null)
            {
                return string.Format(
                    DbEntityNotFound,
                    nameof(ChatUser),
                    $"{chatId}-{invitedUserId}");
            }

            this.data.Remove(mapEntity);
            await this.data.SaveChangesAsync();

            _ = await this.notificationService
                .CreateOnChatInvitationStatusChanged(
                    chatId,
                    chatName,
                    chatCreatorId,
                    false);

            return true;
        }

        public async Task<Result> InviteUserToChat(
           int chatId,
           string chatName,
           string userToInviteId)
        {
            var chatCreatorId = await this.data
               .Chats
               .Where(c => c.Id == chatId)
               .Select(c => c.CreatorId)
               .FirstOrDefaultAsync();

            if (chatCreatorId != this.userService.GetId())
            {
                return string.Format(
                    UnauthorizedDbEntityAction,
                    this.userService.GetUsername(),
                    nameof(Chat),
                    chatId);
            }

            _ = await this.CreateChatUserEntity(chatId, userToInviteId, false);

            await this.notificationService.CreateOnChatInvitation(
                chatId,
                chatName,
                userToInviteId);

            return true;
        }

        public async Task<Result> RemoveUserFromChat(int chatId, string userToRemoveId)
        {
            var currentUserId = this.userService.GetId()!;

            var chatCreatorId = await this.data
                .Chats
                .Where(c => c.Id == chatId)
                .Select(c => c.CreatorId)
                .FirstOrDefaultAsync();

            var isNotCreator = chatCreatorId != currentUserId;
            var notToBeRemoved = userToRemoveId != currentUserId;

            if (isNotCreator && notToBeRemoved)
            {
                return string.Format(
                    UnauthorizedDbEntityAction,
                    this.userService.GetUsername(),
                    nameof(Chat),
                    chatId);
            }

            var mapEntity = await this.data
                .ChatsUsers
                .FirstOrDefaultAsync(cu =>
                    cu.UserId == userToRemoveId && 
                    cu.ChatId == chatId);

            if (mapEntity is null)
            {
                return string.Format(
                    DbEntityNotFound,
                    nameof(ChatUser),
                    $"{chatId}-{userToRemoveId}");
            }

            this.data.Remove(mapEntity);
            await this.data.SaveChangesAsync();

            return true;
        }

        private async Task<bool> CreateChatUserEntity(
            int chatId,
            string userId,
            bool hasAccepted)
        {
            var mapEntity = new ChatUser()
            {
                UserId = userId,
                ChatId = chatId,
                HasAccepted = hasAccepted
            };

            try
            {
                this.data.Add(mapEntity);
                await this.data.SaveChangesAsync();

                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }
    }
}
