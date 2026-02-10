namespace BookHub.Features.Chat.Web;

using Common;
using Infrastructure.Extensions;
using Infrastructure.Services.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Service;
using Service.Models;
using Shared;
using System.Threading;
using UserProfile.Service.Models;

using static Common.Constants.ApiRoutes;
using static Shared.Constants.RouteNames;

[Authorize]
public class ChatController(
    IChatService service,
    IChatMessageService messageService) : ApiController
{
    [HttpGet(Id + ApiRoutes.Messages)]
    public async Task<ActionResult<IEnumerable<ChatMessageServiceModel>>> Messages(
        Guid id,
        int? before,
        int take = 50,
        CancellationToken cancellationToken = default)
    {
        var result = await messageService.GetForChat(
            id,
            before,
            take,
            cancellationToken);

        if (result.Succeeded)
        {
            return this.Ok(result.Data);
        }

        return this.BadRequest(result.ErrorMessage);
    }

    [HttpGet(Id, Name = DetailsRouteName)]
    public async Task<ActionResult<ChatDetailsServiceModel>> Details(
        Guid id,
        CancellationToken cancellationToken = default)
       => this.Ok(await service.Details(id, cancellationToken));

    [HttpGet(ApiRoutes.NotJoined)]
    public async Task<ActionResult<IEnumerable<ChatServiceModel>>> NotJoined(
        string userId,
        CancellationToken cancellationToken = default)
        => this.Ok(await service.NotJoined(userId, cancellationToken));

    [HttpGet(Id + ApiRoutes.Access)]
    public async Task<ActionResult<bool>> CanAccessChat(
        Guid id,
        string userId,
        CancellationToken cancellationToken = default)
        => this.Ok(await service.CanAccessChat(id, userId, cancellationToken));

    [HttpGet(Id + ApiRoutes.Invited)]
    public async Task<ActionResult<bool>> IsInvited(
        Guid id,
        string userId,
        CancellationToken cancellationToken = default)
        => this.Ok(await service.IsInvited(id, userId, cancellationToken));

    [HttpPost]
    public async Task<ActionResult<ChatDetailsServiceModel>> Create(
        CreateChatWebModel webModel,
        CancellationToken cancellationToken = default)
    {
        var serviceModel = webModel.ToCreateChatServiceModel();
        var result = await service.Create(
            serviceModel,
            cancellationToken);

        if (result.Succeeded)
        {
            return this.CreatedAtRoute(
                routeName: DetailsRouteName,
                routeValues: new { id = result.Data!.Id },
                value: result.Data);
        }

        return this.BadRequest(result.ErrorMessage);
    }

    [HttpPut(Id)]
    public async Task<ActionResult<Result>> Edit(
        Guid id,
        CreateChatWebModel webModel,
        CancellationToken cancellationToken = default)
    {
        var serviceModel = webModel.ToCreateChatServiceModel();
        var result = await service.Edit(
            id,
            serviceModel,
            cancellationToken);

        return this.NoContentOrBadRequest(result);
    }

    [HttpDelete(Id)]
    public async Task<ActionResult<Result>> Delete(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await service.Delete(
            id,
            cancellationToken);

        return this.NoContentOrBadRequest(result);
    }

    [HttpPost(ApiRoutes.AcceptInvite)]
    public async Task<ActionResult<ResultWith<PrivateProfileServiceModel>>> Accept(
        ProcessChatInvitationWebModel webModel,
        CancellationToken cancellationToken = default)
    {
        var serviceModel = webModel.ToProcessChatInvitationServiceModel();
        var result = await service.Accept(
            serviceModel,
            cancellationToken);

        if (result.Succeeded)
        {
            return this.Ok(result.Data);
        }

        return this.BadRequest(result.ErrorMessage);
    }

    [HttpPost(ApiRoutes.RejectInvite)]
    public async Task<ActionResult<Result>> Reject(
        ProcessChatInvitationWebModel webModel,
        CancellationToken cancellationToken = default)
    {
        var serviceModel = webModel.ToProcessChatInvitationServiceModel();
        var result = await service.Reject(
            serviceModel,
            cancellationToken);

        return this.NoContentOrBadRequest(result);
    }

    [HttpPost(Id + ApiRoutes.Invite)]
    public async Task<ActionResult<int>> InviteUser(
       Guid id,
       AddUserToChatWebModel webModel,
       CancellationToken cancellationToken = default)
    {
        var serviceModel = webModel.ToAddUserToChatWebModel();
        var result = await service.InviteUserToChat(
            id,
            serviceModel,
            cancellationToken);

        return this.NoContentOrBadRequest(result);
    }

    [HttpDelete(ApiRoutes.RemoveUser)]
    public async Task<ActionResult<Result>> RemoveUser(
        Guid id,
        string userId,
        CancellationToken cancellationToken = default)
    {
        var result = await service.RemoveUserFromChat(
            id,
            userId,
            cancellationToken);

        return this.NoContentOrBadRequest(result);
    }
}
