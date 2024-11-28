namespace BookHub.Server.Features.Notification.Web
{
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
    }
}
