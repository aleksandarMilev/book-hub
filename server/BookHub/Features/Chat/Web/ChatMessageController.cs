namespace BookHub.Features.Chat.Web
{
    using BookHub.Common;
    using Infrastructure.Extensions;
    using Infrastructure.Services.CurrentUser;
    using Infrastructure.Services.Result;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Service;
    using Service.Models;
    using static Common.Constants.ApiRoutes;

    public class ChatMessageController(
        IChatMessageService service,
        ICurrentUserService userService) : ApiController
    {
        private readonly IChatMessageService service = service;
        private readonly ICurrentUserService userService = userService;

        [HttpPost]
        public async Task<ActionResult<ChatMessageServiceModel>> Create(CreateChatMessageWebModel webModel)
        {
            //var serviceModel = this.mapper.Map<CreateChatMessageServiceModel>(webModel);
            var serviceModel = new CreateChatMessageServiceModel(); // TODO: implement mapping
            var createdServiceModel = await this.service.Create(serviceModel);

            return this.Created(nameof(this.Create), createdServiceModel);
        }

        [HttpPut(Id)]
        public async Task<ActionResult<ResultWith<ChatMessageServiceModel>>> Edit(
            int id,
            CreateChatMessageWebModel webModel)
        {
            //var serviceModel = this.mapper.Map<CreateChatMessageServiceModel>(webModel);
            var serviceModel = new CreateChatMessageServiceModel(); // TODO: implement mapping
            var result = await this.service.Edit(id, serviceModel);

            if (result.Succeeded)
            {
                return this.Ok(result.Data);
            }

            return this.BadRequest(result.ErrorMessage);
        }

        [HttpDelete(Id)]
        public async Task<ActionResult<Result>> Delete(int id)
        {
            var result = await this.service.Delete(id);

            return this.NoContentOrBadRequest(result);
        }
    }
}
