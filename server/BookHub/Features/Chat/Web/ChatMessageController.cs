namespace BookHub.Features.Chat.Web;

using BookHub.Common;
using BookHub.Features.Chat.Shared;
using Infrastructure.Extensions;
using Infrastructure.Services.Result;
using Microsoft.AspNetCore.Mvc;
using Models;
using Service;
using Service.Models;

using static Common.Constants.ApiRoutes;

public class ChatMessageController(IChatMessageService service) : ApiController
{
    [HttpPost]
    public async Task<ActionResult<ChatMessageServiceModel>> Create(
        CreateChatMessageWebModel webModel,
        CancellationToken token = default)
    {
        var serviceModel = webModel.ToCreateChatMessageServiceModel();
        var result = await service.Create(serviceModel, token);

        if (result.Succeeded)
        {
            return this.Ok(result.Data);
        }

        return this.BadRequest(result.ErrorMessage);
    }

    [HttpPut(Id)]
    public async Task<ActionResult<ResultWith<ChatMessageServiceModel>>> Edit(
        int id,
        CreateChatMessageWebModel webModel,
        CancellationToken token = default)
    {
        var serviceModel = webModel.ToCreateChatMessageServiceModel();
        var result = await service.Edit(
            id,
            serviceModel,
            token);

        if (result.Succeeded)
        {
            return this.Ok(result.Data);
        }

        return this.BadRequest(result.ErrorMessage);
    }

    [HttpDelete(Id)]
    public async Task<ActionResult<Result>> Delete(
        int id,
        CancellationToken token = default)
    {
        var result = await service.Delete(id, token);

        return this.NoContentOrBadRequest(result);
    }
}
