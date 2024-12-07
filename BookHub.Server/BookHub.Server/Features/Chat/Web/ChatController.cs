namespace BookHub.Server.Features.Chat.Web
{
    using AutoMapper;
    using Infrastructure.Extensions;
    using Infrastructure.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Service;
    using Service.Models;

    using static Common.Constants.ApiRoutes.CommonRoutes;

    [Authorize]
    public class ChatController(
        IChatService service,
        IMapper mapper) : ApiController
    {
        private readonly IChatService service = service;
        private readonly IMapper mapper = mapper;

        [HttpGet(Id)]
        public async Task<ActionResult<ChatDetailsServiceModel>> Details(int id)
           => this.Ok(await this.service.DetailsAsync(id));

        [HttpGet(ApiRoutes.NotJoined)]
        public async Task<ActionResult<IEnumerable<ChatServiceModel>>> NotJoined(string userId)
            => this.Ok(await this.service.NotJoinedAsync(userId));

        [HttpGet(Id + ApiRoutes.Access + Id)]
        public async Task<ActionResult<bool>> CanAccessChat(int chatId, string userId)
            => this.Ok(await this.service.CanAccessChatAsync(chatId, userId));

        [HttpGet(Id + ApiRoutes.Invited + Id)]
        public async Task<ActionResult<bool>> IsInvited(int chatId, string userId)
            => this.Ok(await this.service.IsInvitedAsync(chatId, userId));

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateChatWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateChatServiceModel>(webModel);
            var id = await this.service.CreateAsync(serviceModel);

            return this.Created(nameof(this.Create), id);
        }

        [HttpPost(Id + ApiRoutes.Invite)]
        public async Task<ActionResult<(int, string)>> InviteUserToChat(int chatId, AddUserToChatWebModel model)
        {
            var id = await this.service.InviteUserToChatAsync(
                chatId,
                model.ChatName,
                model.UserId);

            return this.Created(nameof(this.InviteUserToChat), id);
        }

        [HttpPost(ApiRoutes.AcceptInvite)]
        public async Task<ActionResult<Result>> Accept(ProcessChatInvitationWebModel model)
        {
            var result = await this.service.AcceptAsync(
                model.ChatId,
                model.ChatName,
                model.ChatCreatorId);

            return this.NoContentOrBadRequest(result);
        }

        [HttpPost(ApiRoutes.RejectInvite)]
        public async Task<ActionResult<Result>> Reject(ProcessChatInvitationWebModel model)
        {
            var result = await this.service.RejectAsync(
                model.ChatId,
                model.ChatName,
                model.ChatCreatorId);

            return this.NoContentOrBadRequest(result);
        }

        [HttpDelete(ApiRoutes.RemoveUser)]
        public async Task<ActionResult<Result>> Remove(int chatId, string userId)
        {
            var result = await this.service.RemoveUserAsync(chatId, userId);

            return this.NoContentOrBadRequest(result);
        }

        [HttpPut(Id)]
        public async Task<ActionResult<Result>> Edit(int id, CreateChatWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateChatServiceModel>(webModel);
            var result = await this.service.EditAsync(id, serviceModel);

            return this.NoContentOrBadRequest(result);
        }

        [HttpDelete(Id)]
        public async Task<ActionResult<Result>> Delete(int id)
        {
            var result = await this.service.DeleteAsync(id);

            return this.NoContentOrBadRequest(result);
        }
    }
}
