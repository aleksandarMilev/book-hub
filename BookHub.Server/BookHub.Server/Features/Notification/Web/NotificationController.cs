namespace BookHub.Server.Features.Notification.Web
{
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Service;
    using Service.Models;

    using static Server.Common.ApiRoutes;
    using static Server.Common.DefaultValues;

    [Authorize]
    public class NotificationController(INotificationService service) : ApiController
    {
        private readonly INotificationService service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificationServiceModel>>> All(
           int pageIndex = DefaultPageIndex,
           int pageSize = DefaultPageSize) => this.Ok(await this.service.AllAsync(pageIndex, pageSize));

        [HttpGet(ApiRoutes.Last)]
        public async Task<ActionResult<IEnumerable<NotificationServiceModel>>> LastThree()
            => this.Ok(await this.service.LastThreeAsync());

        [HttpPatch(Id + ApiRoutes.Read)]
        public async Task<ActionResult> MarkRead(int id)
        {
            var result = await this.service.MarkAsReadAsync(id);

            return this.NoContentOrBadRequest(result);
        }

        [HttpDelete(Id)]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await this.service.DeleteAsync(id);

            return this.NoContentOrBadRequest(result);
        }
    }
}
