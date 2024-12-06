namespace BookHub.Server.Features.Chat.Web
{
    using AutoMapper;
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

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<ChatDetailsServiceModel>> Details(int id)
           => this.Ok(await this.service.DetailsAsync(id));

        [HttpGet("chats-not-joined")]
        public async Task<ActionResult<IEnumerable<ChatServiceModel>>> ChatsNotJoined(string userId)
            => this.Ok(await this.service.ChatsNotJoinedAsync(userId));

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateChatWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateChatServiceModel>(webModel);
            var id = await this.service.CreateAsync(serviceModel);

            return this.Created(nameof(this.Create), id);
        }

        [AllowAnonymous]
        [HttpPost("{chatId}/users")]
        public async Task<ActionResult<(int, string)>> AddUserToChat(int chatId, AddUserToChatWebModel model)
        {
            var id = await this.service.AddUserToChatAsync(chatId, model.UserId);

            return this.Created(nameof(this.AddUserToChat), id);
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
