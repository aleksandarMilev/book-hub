namespace BookHub.Server.Features.Chat.Web
{
    using AutoMapper;
    using Infrastructure.Extensions;
    using Infrastructure.Services;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Service;
    using Service.Models;

    using static Server.Common.ApiRoutes;

    public class ChatMessageController(
        IChatMessageService service,
        ICurrentUserService userService,
        IMapper mapper) : ApiController
    {
        private readonly IChatMessageService service = service;
        private readonly ICurrentUserService userService = userService;
        private readonly IMapper mapper = mapper;

        [HttpPost]
        public async Task<ActionResult<ChatMessageServiceModel>> Create(CreateChatMessageWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateChatMessageServiceModel>(webModel);
            var createdServiceModel = await this.service.CreateAsync(serviceModel);

            return this.Created(nameof(this.Create), createdServiceModel);
        }

        [HttpPut(Id)]
        public async Task<ActionResult<ResultWith<ChatMessageServiceModel>>> Edit(int id, CreateChatMessageWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateChatMessageServiceModel>(webModel);
            var result = await this.service.EditAsync(id, serviceModel);

            if (result.Succeeded)
            {
                return this.Ok(result.Data);
            }

            return this.BadRequest(result.ErrorMessage);
        }

        [HttpDelete(Id)]
        public async Task<ActionResult<Result>> Delete(int id)
        {
            var result = await this.service.DeleteAsync(id);

            return this.NoContentOrBadRequest(result);
        }
    }
}
