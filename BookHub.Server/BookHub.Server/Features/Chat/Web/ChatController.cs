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

        [HttpGet("{id}")]
        public async Task<ActionResult<ChatDetailsServiceModel>> Details(int id)
           => this.Ok(await this.service.DetailsAsync(id));

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateChatWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateChatServiceModel>(webModel);
            var id = await this.service.CreateAsync(serviceModel);

            return this.Created(nameof(this.Create), id);
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
