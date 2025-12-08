namespace BookHub.Features.Chat.Web
{
    using AutoMapper;
    using BookHub.Common;
    using Infrastructure.Extensions;
    using Infrastructure.Services.Result;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Service;
    using Service.Models;
    using UserProfile.Service.Models;

    using static Common.Constants.ApiRoutes;

    [Authorize]
    public class ChatController(
        IChatService service,
        IMapper mapper) : ApiController
    {
        private readonly IChatService service = service;
        private readonly IMapper mapper = mapper;

        [HttpGet(Id)]
        public async Task<ActionResult<ChatDetailsServiceModel>> Details(int id)
           => this.Ok(await this.service.Details(id));

        [HttpGet(ApiRoutes.NotJoined)]
        public async Task<ActionResult<IEnumerable<ChatServiceModel>>> NotJoined(string userId)
            => this.Ok(await this.service.NotJoined(userId));

        [HttpGet(Id + ApiRoutes.Access)]
        public async Task<ActionResult<bool>> CanAccessChat(int id, string userId)
            => this.Ok(await this.service.CanAccessChat(id, userId));

        [HttpGet(Id + ApiRoutes.Invited)]
        public async Task<ActionResult<bool>> IsInvited(int id, string userId)
            => this.Ok(await this.service.IsInvited(id, userId));

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateChatWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateChatServiceModel>(webModel);
            var id = await this.service.Create(serviceModel);

            return this.Created(nameof(this.Create), id);
        }

        [HttpPut(Id)]
        public async Task<ActionResult<Result>> Edit(int id, CreateChatWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateChatServiceModel>(webModel);
            var result = await this.service.Edit(id, serviceModel);

            return this.NoContentOrBadRequest(result);
        }

        [HttpDelete(Id)]
        public async Task<ActionResult<Result>> Delete(int id)
        {
            var result = await this.service.Delete(id);

            return this.NoContentOrBadRequest(result);
        }

        [HttpPost(ApiRoutes.AcceptInvite)]
        public async Task<ActionResult<ResultWith<PrivateProfileServiceModel>>> Accept(ProcessChatInvitationWebModel model)
        {
            var result = await this.service.Accept(
                model.ChatId,
                model.ChatName,
                model.ChatCreatorId);

            if (result.Succeeded)
            {
                return this.Ok(result.Data);
            }

            return this.BadRequest(result.ErrorMessage);
        }

        [HttpPost(ApiRoutes.RejectInvite)]
        public async Task<ActionResult<Result>> Reject(ProcessChatInvitationWebModel model)
        {
            var result = await this.service.Reject(
                model.ChatId,
                model.ChatName,
                model.ChatCreatorId);

            return this.NoContentOrBadRequest(result);
        }

        [HttpPost(Id + ApiRoutes.Invite)]
        public async Task<ActionResult<int>> InviteUser(
           int id,
           AddUserToChatWebModel model)
        {
            var result = await this.service.InviteUserToChat(
                id,
                model.ChatName,
                model.UserId);

            if (result.Succeeded)
            {
                return this.Created(nameof(this.Created), id);
            }

            return this.BadRequest(new { errorMessage = result.ErrorMessage });
        }

        [HttpDelete(ApiRoutes.RemoveUser)]
        public async Task<ActionResult<Result>> RemoveUser(int chatId, string userId)
        {
            var result = await this.service.RemoveUserFromChat(chatId, userId);

            return this.NoContentOrBadRequest(result);
        }
    }
}
