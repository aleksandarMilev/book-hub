namespace BookHub.Server.Features.Chat.Service
{
    using AutoMapper;
    using Data.Models;
    using Infrastructure.Services;
    using Server.Data;
    using Service.Models;

    using static Common.ErrorMessage;

    public class ChatMessageService(
        BookHubDbContext data,
        ICurrentUserService userService,
        IMapper mapper) : IChatMessageService
    {
        private readonly BookHubDbContext data = data;
        private readonly ICurrentUserService userService = userService;
        private readonly IMapper mapper = mapper;

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
                return string.Format(DbEntityNotFound, nameof(ChatMessage), id);
            }

            if (message.SenderId != this.userService.GetId())
            {
                return string.Format(
                    UnauthorizedDbEntityAction,
                    this.userService.GetUsername(),
                    nameof(ChatMessage),
                    id);
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
                return string.Format(DbEntityNotFound, nameof(ChatMessage), id);
            }

            if (message.SenderId != this.userService.GetId())
            {
                return string.Format(
                    UnauthorizedDbEntityAction,
                    this.userService.GetUsername(),
                    nameof(ChatMessage),
                    id);
            }

            this.data.Remove(message);
            await this.data.SaveChangesAsync();

            return true;
        }
    }
}
