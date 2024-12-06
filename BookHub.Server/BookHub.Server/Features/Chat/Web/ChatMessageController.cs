namespace BookHub.Server.Features.Chat.Web
{
    using AutoMapper;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Service;
    using Service.Models;

    public class ChatMessageController(
        IChatMessageService service,
        IMapper mapper) : ApiController
    {
        private readonly IChatMessageService service = service;
        private readonly IMapper mapper = mapper;

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateChatMessageWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateChatMessageServiceModel>(webModel);
            var id = await this.service.CreateAsync(serviceModel);

            return this.Created(nameof(this.Create), id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, CreateChatMessageWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateChatMessageServiceModel>(webModel);
            var result = await this.service.EditAsync(id, serviceModel);

            return this.NoContentOrBadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await this.service.DeleteAsync(id);

            return this.NoContentOrBadRequest(result);
        }
    }
}
