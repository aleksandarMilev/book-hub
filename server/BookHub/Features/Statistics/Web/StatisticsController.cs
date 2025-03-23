namespace BookHub.Features.Statistics.Web
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Service;
    using Service.Models;

    [Authorize]
    public class StatisticsController(IStatisticsService service) : ApiController
    {
        private readonly IStatisticsService service = service;

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<StatisticsServiceModel>> All()
            => this.Ok(await this.service.All());
    }
}
