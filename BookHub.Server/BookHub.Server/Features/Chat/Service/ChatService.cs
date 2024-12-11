namespace BookHub.Server.Features.Chat.Service
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using BookHub.Server.Features.UserProfile.Service.Models;
    using Data.Models;
    using Infrastructure.Services;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Notification.Service;
    using Server.Data;
    using Server.Data.Models.Shared.ChatUser;
    using Service.Models;

    using static Common.ErrorMessage;

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

        public async Task<IEnumerable<ChatServiceModel>> AllAsync()
            => await this.data
                .Chats
                .ProjectTo<ChatServiceModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<IEnumerable<ChatServiceModel>> NotJoinedAsync(string userToJoinId)
            => await this.data
                .Chats
                .Where(c => 
                    c.CreatorId == this.userService.GetId() && 
                    !c.ChatsUsers.Any(cu => cu.UserId == userToJoinId)
                )
                .ProjectTo<ChatServiceModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<bool> CanAccessChatAsync(int chatId, string userId)
            => await this.data
                .ChatsUsers
                .AnyAsync(cu => cu.UserId == userId && cu.ChatId == chatId);

        public async Task<bool> IsInvitedAsync(int chatId, string userId)
              => await this.data
                  .ChatsUsers
                  .AnyAsync(cu => 
                      cu.UserId == userId && 
                      cu.ChatId == chatId &&
                      !cu.HasAccepted);

        public async Task<ChatDetailsServiceModel?> DetailsAsync(int id)
            => await this.data
                .Chats
                .ProjectTo<ChatDetailsServiceModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<int> CreateAsync(CreateChatServiceModel model)
        {
            var creatorId = this.userService.GetId()!;

            model.ImageUrl ??= DefaultImageUrl;

            var chat = this.mapper.Map<Chat>(model);
            chat.CreatorId = creatorId;

            this.data.Add(chat);
            await this.data.SaveChangesAsync();

            _ = await this.CreateChatUserEntityAsync(chat.Id, creatorId, true);

            return chat.Id;
        }

        public async Task InviteUserToChatAsync(int chatId, string chatName, string userId)
        {
            _ = await this.CreateChatUserEntityAsync(chatId, userId, false);

            await this.notificationService.CreateOnChatInvitationAsync(chatId, chatName, userId);
        }

        public async Task<ResultWith<PrivateProfileServiceModel>> AcceptAsync(int chatId, string chatName, string chatCreatorId)
        {
            var invitedUserId = this.userService.GetId()!;

            var mapEntity = await this.data
                .ChatsUsers
                .FirstOrDefaultAsync(cu => cu.UserId == invitedUserId && cu.ChatId == chatId);

            if (mapEntity is null)
            {
                return string.Format(DbEntityNotFound, nameof(ChatUser), $"{chatId}-{invitedUserId}");
            }

            mapEntity.HasAccepted = true;
            await this.data.SaveChangesAsync();

            _ = await this.notificationService
                .CreateOnChatInvitationStatusChangedAsync(
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

        public async Task<Result> RejectAsync(int chatId, string chatName, string chatCreatorId)
        {
            var invitedUserId = this.userService.GetId();

            var mapEntity = await this.data
               .ChatsUsers
               .FirstOrDefaultAsync(cu => cu.UserId == invitedUserId && cu.ChatId == chatId);

            if (mapEntity is null)
            {
                return string.Format(DbEntityNotFound, nameof(ChatUser), $"{chatId}-{invitedUserId}");
            }

            this.data.Remove(mapEntity);
            await this.data.SaveChangesAsync();

            _ = await this.notificationService
                .CreateOnChatInvitationStatusChangedAsync(
                    chatId,
                    chatName,
                    chatCreatorId,
                    false);

            return true;
        }

        public async Task<Result> RemoveUserAsync(int chatId, string userToRemoveId)
        {
            var mapEntity = await this.data
                .ChatsUsers
                .FirstOrDefaultAsync(cu => cu.UserId == userToRemoveId && cu.ChatId == chatId);

            if (mapEntity is null)
            {
                return string.Format(DbEntityNotFound, nameof(ChatUser), $"{chatId}-{userToRemoveId}");
            }

            this.data.Remove(mapEntity);
            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<Result> EditAsync(int id, CreateChatServiceModel model)
        {
            var chat = await this.data
               .Chats
               .FindAsync(id);

            if (chat is null)
            {
                return string.Format(DbEntityNotFound, nameof(Chat), id);
            }

            if (chat.CreatorId != this.userService.GetId() &&
                !this.userService.IsAdmin())
            {
                return string.Format(
                    UnauthorizedDbEntityAction,
                    this.userService.GetUsername(),
                    nameof(ChatUser), 
                    id);
            }

            model.ImageUrl ??= DefaultImageUrl;

            this.mapper.Map(model, chat);
            await this.data.SaveChangesAsync();

            return true;
        }
           

        public async Task<Result> DeleteAsync(int id)
        {
            var chat = await this.data
                .Chats
                .FindAsync(id);

            if (chat is null)
            {
                return string.Format(DbEntityNotFound, nameof(Chat), id);
            }

            if (chat.CreatorId != this.userService.GetId() &&
                !this.userService.IsAdmin())
            {
                return string.Format(
                    UnauthorizedDbEntityAction,
                    this.userService.GetUsername(),
                    nameof(ChatUser),
                    id);
            }

            this.data.Remove(chat);
            await this.data.SaveChangesAsync();

            return true;
        }

        private async Task<bool> CreateChatUserEntityAsync(int chatId, string userId, bool hasAccepted)
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
