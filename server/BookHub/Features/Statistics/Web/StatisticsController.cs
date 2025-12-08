namespace BookHub.Features.Statistics.Web;

using BookHub.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Models;

[Authorize]
public class StatisticsController(IStatisticsService service) : ApiController
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<StatisticsServiceModel>> All(
        CancellationToken token = default)
        => this.Ok(await service.All(token));
}
