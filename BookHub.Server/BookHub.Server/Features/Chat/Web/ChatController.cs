namespace BookHub.Server.Features.Chat.Web
{
    using AutoMapper;
    using BookHub.Server.Infrastructure.Services;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Service;
    using Service.Models;

    [Authorize]
    public class ChatController(
        IChatService service,
        IMapper mapper) : ApiController
    {
        private readonly IChatService service = service;
        private readonly IMapper mapper = mapper;

        [HttpGet("{id}")]
        public async Task<ActionResult<ChatDetailsServiceModel>> Details(int id)
           => this.Ok(await this.service.DetailsAsync(id));

        [HttpGet("chats-not-joined")]
        public async Task<ActionResult<IEnumerable<ChatServiceModel>>> ChatsNotJoined(string userId)
            => this.Ok(await this.service.ChatsNotJoinedAsync(userId));

        [HttpGet("{chatId}/access/{userId}")]
        public async Task<ActionResult<bool>> CanAccessChat(int chatId, string userId)
            => this.Ok(await this.service.CanAccessChatAsync(chatId, userId));

        [HttpGet("{chatId}/invited/{userId}")]
        public async Task<ActionResult<bool>> IsInvited(int chatId, string userId)
            => this.Ok(await this.service.IsInvitedAsync(chatId, userId));

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateChatWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateChatServiceModel>(webModel);
            var id = await this.service.CreateAsync(serviceModel);

            return this.Created(nameof(this.Create), id);
        }

        [HttpPost("{chatId}/invite")]
        public async Task<ActionResult<(int, string)>> InviteUserToChat(int chatId, AddUserToChatWebModel model)
        {
            var id = await this.service.InviteUserToChatAsync(
                chatId,
                model.ChatName,
                model.UserId);

            return this.Created(nameof(this.InviteUserToChat), id);
        }

        [HttpPost("invite/accept")]
        public async Task<ActionResult<Result>> Accept(ProcessChatInvitationWebModel model)
        {
            var result = await this.service.AcceptAsync(
                model.ChatId,
                model.ChatName,
                model.ChatCreatorId);

            return this.NoContentOrBadRequest(result);
        }

        [HttpPost("invite/reject")]
        public async Task<ActionResult<Result>> Reject(ProcessChatInvitationWebModel model)
        {
            var result = await this.service.RejectAsync(
                model.ChatId,
                model.ChatName,
                model.ChatCreatorId);

            return this.NoContentOrBadRequest(result);
        }

        [HttpDelete("remove-user")]
        public async Task<ActionResult<Result>> Remove(int chatId, string userId)
        {
            var result = await this.service.RemoveUserAsync(chatId, userId);

            return this.NoContentOrBadRequest(result);
        }

        [HttpPut("{id}")]
        public Task<IActionResult> Edit(int id, CreateChatWebModel webModel) 
            => throw new NotImplementedException();

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await this.service.DeleteAsync(id);

            return this.NoContentOrBadRequest(result);
        }
    }
}
