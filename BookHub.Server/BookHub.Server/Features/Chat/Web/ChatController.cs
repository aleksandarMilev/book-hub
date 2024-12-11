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
    using UserProfile.Service.Models;

    using static Server.Common.ApiRoutes;

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

        [AllowAnonymous]
        [HttpGet(Id + ApiRoutes.Access)]
        public async Task<ActionResult<bool>> CanAccessChat(int id, string userId)
            => this.Ok(await this.service.CanAccessChatAsync(id, userId));

        [HttpGet(Id + ApiRoutes.Invited)]
        public async Task<ActionResult<bool>> IsInvited(int id, string userId)
            => this.Ok(await this.service.IsInvitedAsync(id, userId));

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateChatWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateChatServiceModel>(webModel);
            var id = await this.service.CreateAsync(serviceModel);

            return this.Created(nameof(this.Create), id);
        }

        [HttpPost(Id + ApiRoutes.Invite)]
        public async Task<ActionResult<(int, string)>> InviteUserToChat(int id, AddUserToChatWebModel model)
        {
            await this.service.InviteUserToChatAsync(id, model.ChatName, model.UserId);

            return this.Created(nameof(this.InviteUserToChat), id);
        }

        [HttpPost(ApiRoutes.AcceptInvite)]
        public async Task<ActionResult<ResultWith<PrivateProfileServiceModel>>> Accept(ProcessChatInvitationWebModel model)
        {
            var result = await this.service.AcceptAsync(
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
