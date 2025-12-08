namespace BookHub.Features.Notification.Web
{
    using BookHub.Common;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Service;
    using Service.Models;

    using static Common.Constants.ApiRoutes;
    using static Common.Constants.DefaultValues;

    [Authorize]
    public class NotificationController(INotificationService service) : ApiController
    {
        private readonly INotificationService service = service;

        [HttpGet(ApiRoutes.Last)]
        public async Task<ActionResult<IEnumerable<NotificationServiceModel>>> LastThree()
            => this.Ok(await this.service.LastThree());

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificationServiceModel>>> All(
           int pageIndex = DefaultPageIndex,
           int pageSize = DefaultPageSize)
           => this.Ok(await this.service.All(pageIndex, pageSize));

        [HttpDelete(Id)]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await this.service.Delete(id);

            return this.NoContentOrBadRequest(result);
        }

        [HttpPatch(Id + ApiRoutes.Read)]
        public async Task<ActionResult> MarkRead(int id)
        {
            var result = await this.service.MarkAsRead(id);

            return this.NoContentOrBadRequest(result);
        }
    }
}
