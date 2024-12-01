#pragma warning disable ASP0023 
namespace BookHub.Server.Features.Notification.Web
{
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Service;
    using Service.Models;

    using static Common.Constants.DefaultValues;

    [Authorize]
    public class NotificationController(INotificationService service) : ApiController
    {
        private readonly INotificationService service = service;

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<NotificationServiceModel>>> LastThree()
            => this.Ok(await this.service.LastThreeAsync());

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<NotificationServiceModel>>> All(
            int pageIndex = DefaultPageIndex,
            int pageSize = DefaultPageSize) 
                => this.Ok(await this.service.AllAsync(pageIndex, pageSize));


        [HttpPatch("{id}/[action]")]
        public async Task<ActionResult> MarkRead(int id)
        {
            var result = await service.MarkAsReadAsync(id);

            return this.NoContentOrBadRequest(result);
        }
    }
}
