namespace BookHub.Server.Features.Chat.Web
{
    using AutoMapper;
    using Infrastructure.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using Models;
    using Service;
    using Service.Models;

    using static Server.Common.ApiRoutes;

    public class ChatMessageController(
        IChatMessageService service,
        ICurrentUserService userService,
        IHubContext<ChatHub> hubContext,
        IMapper mapper) : ApiController
    {
        private readonly IChatMessageService service = service;
        private readonly ICurrentUserService userService = userService;
        private readonly IHubContext<ChatHub> hubContext = hubContext;
        private readonly IMapper mapper = mapper;

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateChatMessageWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateChatMessageServiceModel>(webModel);
            var id = await this.service.CreateAsync(serviceModel);

            await this.hubContext
                .Clients
                .Group(webModel.ChatId.ToString())
                .SendAsync("ReceiveMessage", this.userService.GetId(), webModel.Message);

            return this.Created(nameof(this.Create), id);
        }

        [HttpPut(Id)]
        public async Task<ActionResult<Result>> Edit(int id, CreateChatMessageWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateChatMessageServiceModel>(webModel);
            var result = await this.service.EditAsync(id, serviceModel);

            if (result.Succeeded)
            {
                await this.hubContext
                    .Clients
                    .Group(webModel.ChatId.ToString())
                    .SendAsync("EditMessage", id, webModel.Message);

                return this.NoContent();
            }


            return this.BadRequest(result.ErrorMessage);
        }

        [HttpDelete(Id)]
        public async Task<ActionResult<Result>> Delete(int id)
        {
            var result = await this.service.DeleteAsync(id);

            if (result.Succeeded)
            {
                await this.hubContext
                    .Clients
                    .Group(id.ToString())
                    .SendAsync("DeleteMessage", id);

                return this.NoContent();
            }

            return this.BadRequest(result.ErrorMessage);
        }
    }
}
