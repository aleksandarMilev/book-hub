namespace BookHub.Server.Features.Chat.Service
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;
    using Service.Models;

    using static Common.Messages.Error.ChatMessage;

    public class ChatMessageService(
        BookHubDbContext data,
        ICurrentUserService userService,
        IMapper mapper) : IChatMessageService
    {
        private readonly BookHubDbContext data = data;
        private readonly ICurrentUserService userService = userService;
        private readonly IMapper mapper = mapper;

        public async Task<IEnumerable<ChatMessageServiceModel>> AllForChatAsync(int chatId)
            //=> await this.data
            //    .ChatMessages
            //    .Where(c => c.ChatId == chatId)
            //    .ProjectTo<ChatMessageServiceModel>(this.mapper.ConfigurationProvider)
            //    .ToListAsync();
            => throw new NotImplementedException();

        public async Task<int> CreateAsync(CreateChatMessageServiceModel model)
        {
            var message = this.mapper.Map<ChatMessage>(model);
            message.SenderId = this.userService.GetId()!;

            this.data.Add(message);
            await this.data.SaveChangesAsync();

            return message.Id;
        }

        public async Task<Result> EditAsync(int id, CreateChatMessageServiceModel model)
        {
            var message = await this.data
                .ChatMessages
                .FindAsync(id);

            if (message is null)
            {
                return MessageNotFound;
            }

            if (!this.userService.IsAdmin() &&
               message.SenderId != this.userService.GetId())
            {
                return UnauthorizedMessageEdit;
            }

            this.mapper.Map(model, message);
            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var message = await this.data
                .ChatMessages
                .FindAsync(id);

            if (message is null)
            {
                return MessageNotFound;
            }

            if (!this.userService.IsAdmin() &&
               message.SenderId != this.userService.GetId())
            {
                return UnauthorizedMessageDelete;
            }

            this.data.Remove(message);
            await this.data.SaveChangesAsync();

            return true;
        }
    }
}
