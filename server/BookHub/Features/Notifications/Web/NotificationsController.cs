namespace BookHub.Features.Notifications.Web;

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
        CancellationToken cancellationToken = default)
        => this.Ok(await service.LastThree(cancellationToken));

    [HttpGet]
    public async Task<ActionResult<PaginatedModel<NotificationServiceModel>>> All(
       int pageIndex = DefaultPageIndex,
       int pageSize = DefaultPageSize,
       CancellationToken cancellationToken = default)
       => this.Ok(await service.All(pageIndex, pageSize, cancellationToken));

    [HttpDelete(Id)]
    public async Task<ActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await service.Delete(
            id,
            cancellationToken);

        return this.NoContentOrBadRequest(result);
    }

    [HttpPatch(Id + ApiRoutes.Read)]
    public async Task<ActionResult> MarkRead(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await service.MarkAsRead(
            id,
            cancellationToken);

        return this.NoContentOrBadRequest(result);
    }
}
