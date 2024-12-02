namespace BookHub.Server.Features.Statistics.Web
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Service;
    using Service.Models;

    [Authorize]
    public class StatisticsController(IStatisticsService service) : ApiController
    {
        private readonly IStatisticsService service = service;

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<StatisticsServiceModel>> Get()
            => await this.service.GetAsync();
    }
}
