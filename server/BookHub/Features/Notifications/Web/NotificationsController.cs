namespace BookHub.Features.Notification.Web;

using BookHub.Common;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Models;

using static Common.Constants.ApiRoutes;
using static Common.Constants.DefaultValues;

[Authorize]
public class NotificationsController(INotificationService service) : ApiController
{
    [HttpGet(ApiRoutes.Last)]
    public async Task<ActionResult<IEnumerable<NotificationServiceModel>>> LastThree(
        CancellationToken token = default)
        => this.Ok(await service.LastThree(token));

    [HttpGet]
    public async Task<ActionResult<PaginatedModel<NotificationServiceModel>>> All(
       int pageIndex = DefaultPageIndex,
       int pageSize = DefaultPageSize,
       CancellationToken token = default)
       => this.Ok(await service.All(pageIndex, pageSize, token));

    [HttpDelete(Id)]
    public async Task<ActionResult> Delete(
        Guid id,
        CancellationToken token = default)
    {
        var result = await service.Delete(id, token);

        return this.NoContentOrBadRequest(result);
    }

    [HttpPatch(Id + ApiRoutes.Read)]
    public async Task<ActionResult> MarkRead(
        Guid id,
        CancellationToken token = default)
    {
        var result = await service.MarkAsRead(id, token);

        return this.NoContentOrBadRequest(result);
    }
}
