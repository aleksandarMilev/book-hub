namespace BookHub.Server.Features.Notification.Web
{
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Service;
    using Service.Models;

    [Authorize]
    public class NotificationController(INotificationService service) : ApiController
    {
        private readonly INotificationService service = service;

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<NotificationServiceModel>>> LastThree()
            => this.Ok(await this.service.LastThreeAsync());


        [HttpPatch("{id}/read")]
        public async Task<ActionResult> Read(int id)
        {
            var result = await service.MarkAsReadAsync(id);

            return this.NoContentOrBadRequest(result);
        }
    }
}
