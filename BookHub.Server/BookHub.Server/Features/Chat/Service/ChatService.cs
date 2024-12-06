namespace BookHub.Server.Features.Chat.Service
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using Infrastructure.Services;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Service.Models;

    using static Common.Constants.DefaultValues;
    using static Common.Messages.Error.Chat;

    public class ChatService(
        BookHubDbContext data,
        ICurrentUserService userService,
        IMapper mapper) : IChatService
    {
        private readonly BookHubDbContext data = data;
        private readonly ICurrentUserService userService = userService;
        private readonly IMapper mapper = mapper;

        public async Task<IEnumerable<ChatServiceModel>> AllAsync()
            => await this.data
                .Chats
                .ProjectTo<ChatServiceModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<IEnumerable<ChatServiceModel>> ChatsNotJoinedAsync(string userToJoinId)
            => await this.data
                .Chats
                .Where(c => 
                    c.CreatorId == this.userService.GetId() && 
                    !c.ChatsUsers.Any(cu => cu.UserId == userToJoinId)
                )
                .ProjectTo<ChatServiceModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<ChatDetailsServiceModel?> DetailsAsync(int id)
            => await this.data
                .Chats
                .ProjectTo<ChatDetailsServiceModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<int> CreateAsync(CreateChatServiceModel model)
        {
            var creatorId = this.userService.GetId()!;

            model.ImageUrl ??= DefaultChatImageUrl;

            var chat = this.mapper.Map<Chat>(model);
            chat.CreatorId = creatorId;

            this.data.Add(chat);
            await this.data.SaveChangesAsync();

            _ = await this.CreateChatUserEntityAsync(chat.Id, creatorId);

            return chat.Id;
        }

        public async Task<(int, string)> AddUserToChatAsync(int chatId, string userId)
            => await this.CreateChatUserEntityAsync(chatId, userId);

        public Task<Result> EditAsync(int id) 
            => throw new NotImplementedException();

        public async Task<Result> DeleteAsync(int id)
        {
            var chat = await this.data
                .Chats
                .FindAsync(id);

            if (chat is null)
            {
                return ChatNotFound;
            }

            if (!this.userService.IsAdmin() && 
                chat.CreatorId != this.userService.GetId())
            {
                return UnauthorizedChatDelete;
            }

            this.data.Remove(chat);
            await this.data.SaveChangesAsync();

            return true;
        }

        private async Task<(int, string)> CreateChatUserEntityAsync(int chatId, string userId)
        {
            var mapEntity = new ChatUser()
            {
                UserId = userId,
                ChatId = chatId
            };

            try
            {
                this.data.Add(mapEntity);
                await this.data.SaveChangesAsync();

                return (chatId, userId);
            }
            catch (SqlException)
            {
                throw new InvalidOperationException("This user is already a participant in the chat!");
            }
        }
    }
}
