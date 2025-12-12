namespace BookHub.Features.Chat.Web;

using BookHub.Common;
using Infrastructure.Extensions;
using Infrastructure.Services.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Service;
using Service.Models;
using Shared;
using UserProfile.Service.Models;

using static Common.Constants.ApiRoutes;

[Authorize]
public class ChatController(IChatService service) : ApiController
{
    [HttpGet(Id)]
    public async Task<ActionResult<ChatDetailsServiceModel>> Details(
        Guid id,
        CancellationToken token = default)
       => this.Ok(await service.Details(id, token));

    [HttpGet(ApiRoutes.NotJoined)]
    public async Task<ActionResult<IEnumerable<ChatServiceModel>>> NotJoined(
        string userId,
        CancellationToken token = default)
        => this.Ok(await service.NotJoined(userId, token));

    [HttpGet(Id + ApiRoutes.Access)]
    public async Task<ActionResult<bool>> CanAccessChat(
        Guid chatId,
        string userId,
        CancellationToken token = default)
        => this.Ok(await service.CanAccessChat(chatId, userId, token));

    [HttpGet(Id + ApiRoutes.Invited)]
    public async Task<ActionResult<bool>> IsInvited(
        Guid id,
        string userId,
        CancellationToken token = default)
        => this.Ok(await service.IsInvited(id, userId, token));

    [HttpPost]
    public async Task<ActionResult<int>> Create(
        CreateChatWebModel webModel,
        CancellationToken token = default)
    {
        var serviceModel = webModel.ToCreateChatServiceModel();
        var id = await service.Create(serviceModel, token);

        return this.Created(nameof(this.Create), id);
    }

    [HttpPut(Id)]
    public async Task<ActionResult<Result>> Edit(
        Guid id,
        CreateChatWebModel webModel,
        CancellationToken token = default)
    {
        var serviceModel = webModel.ToCreateChatServiceModel();
        var result = await service.Edit(id, serviceModel, token);

        return this.NoContentOrBadRequest(result);
    }

    [HttpDelete(Id)]
    public async Task<ActionResult<Result>> Delete(
        Guid id,
        CancellationToken token = default)
    {
        var result = await service.Delete(id, token);

        return this.NoContentOrBadRequest(result);
    }

    [HttpPost(ApiRoutes.AcceptInvite)]
    public async Task<ActionResult<ResultWith<PrivateProfileServiceModel>>> Accept(
        ProcessChatInvitationWebModel webModel,
        CancellationToken token = default)
    {
        var serviceModel = webModel.ToProcessChatInvitationServiceModel();
        var result = await service.Accept(serviceModel, token);

        if (result.Succeeded)
        {
            return this.Ok(result.Data);
        }

        return this.BadRequest(result.ErrorMessage);
    }

    [HttpPost(ApiRoutes.RejectInvite)]
    public async Task<ActionResult<Result>> Reject(
        ProcessChatInvitationWebModel webModel,
        CancellationToken token = default)
    {
        var serviceModel = webModel.ToProcessChatInvitationServiceModel();
        var result = await service.Reject(serviceModel, token);

        return this.NoContentOrBadRequest(result);
    }

    [HttpPost(Id + ApiRoutes.Invite)]
    public async Task<ActionResult<int>> InviteUser(
       Guid id,
       AddUserToChatWebModel webModel,
       CancellationToken token = default)
    {
        var serviceModel = webModel.ToAddUserToChatWebModel();
        var result = await service.InviteUserToChat(
            id,
            serviceModel,
            token);

        if (result.Succeeded)
        {
            return this.Created(nameof(this.Created), id);
        }

        return this.BadRequest(new { errorMessage = result.ErrorMessage });
    }

    [HttpDelete(ApiRoutes.RemoveUser)]
    public async Task<ActionResult<Result>> RemoveUser(
        Guid chatId,
        string userId,
        CancellationToken token = default)
    {
        var result = await service.RemoveUserFromChat(
            chatId,
            userId,
            token);

        return this.NoContentOrBadRequest(result);
    }
}
