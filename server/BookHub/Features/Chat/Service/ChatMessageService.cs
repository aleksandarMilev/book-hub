namespace BookHub.Features.Chat.Service
{
    using AutoMapper;
    using BookHub.Data;
    using BookHub.Infrastructure.Services.CurrentUser;
    using BookHub.Infrastructure.Services.Result;
    using Data.Models;
    using Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;
    using Service.Models;

    using static Common.Constants.ErrorMessages;

    public class ChatMessageService(
        BookHubDbContext data,
        ICurrentUserService userService,
        IMapper mapper) : IChatMessageService
    {
        private readonly BookHubDbContext data = data;
        private readonly ICurrentUserService userService = userService;
        private readonly IMapper mapper = mapper;

        public async Task<ChatMessageServiceModel> Create(CreateChatMessageServiceModel model)
        {
            var userId = this.userService.GetId()!;

            var message = this.mapper.Map<ChatMessage>(model);
            message.SenderId = userId;

            this.data.Add(message);
            await this.data.SaveChangesAsync();

            var profile = await this.data
                .Profiles
                .Where(p => p.UserId == userId)
                .Select(p => new
                {
                    Name = p.FirstName + " " + p.LastName,
                    Image = p.ImagePath
                })
                .FirstOrDefaultAsync();

            var serviceModel = new ChatMessageServiceModel()
            {
                Id = message.Id,
                Message = message.Message,
                SenderId = message.SenderId,
                SenderName = profile!.Name,
                SenderImageUrl = profile!.Image,
                CreatedOn = message.CreatedOn
            };

            return serviceModel;
        }

        public async Task<ResultWith<ChatMessageServiceModel>> Edit(
            int id, 
            CreateChatMessageServiceModel model)
        {
            var userId = this.userService.GetId();

            var message = await this.data
                .ChatMessages
                .FindAsync(id);

            if (message is null)
            {
                return string.Format(
                    DbEntityNotFound,
                    nameof(ChatMessage),
                    id);
            }

            if (message.SenderId != userId)
            {
                return string.Format(
                    UnauthorizedDbEntityAction,
                    this.userService.GetUsername(),
                    nameof(ChatMessage),
                    id);
            }

            this.mapper.Map(model, message);
            await this.data.SaveChangesAsync();

            var profile = await this.data
               .Profiles
               .Where(p => p.UserId == userId)
               .Select(p => new
               {
                   Name = p.FirstName + " " + p.LastName,
                   Image = p.ImagePath
               })
               .FirstOrDefaultAsync();

            var serviceModel = new ChatMessageServiceModel()
            {
                Id = message.Id,
                Message = message.Message,
                SenderId = message.SenderId,
                SenderName = profile!.Name,
                SenderImageUrl = profile!.Image,
                CreatedOn = message.CreatedOn,
                ModifiedOn = message.ModifiedOn!.Value
            };

            return ResultWith<ChatMessageServiceModel>
                .Success(serviceModel);
        }

        public async Task<Result> Delete(int id)
        {
            var message = await this.data
                .ChatMessages
                .FindAsync(id);

            if (message is null)
            {
                return string.Format(
                    DbEntityNotFound,
                    nameof(ChatMessage),
                    id);
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
