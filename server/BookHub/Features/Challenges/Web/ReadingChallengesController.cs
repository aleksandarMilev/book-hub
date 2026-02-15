namespace BookHub.Features.Challenges.Web;

using BookHub.Common;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Models;
using Shared;
using Web.Models;

using static ApiRoutes;
using static Common.Constants.ApiRoutes;

[Authorize]
public class ReadingChallengesController(IReadingChallengeService service) : ApiController
{
    [HttpGet(Id)]
    public async Task<ActionResult<ReadingChallengeServiceModel?>> Get(
        int id,
        CancellationToken cancellationToken = default)
        => this.Ok(await service.Get(id, cancellationToken));

    [HttpPut]
    public async Task<ActionResult> Upsert(
        UpsertReadingChallengeWebModel webModel,
        CancellationToken cancellationToken = default)
    {
        var serviceModel = webModel.ToServiceModel();
        var result = await service.Upsert(
            serviceModel,
            cancellationToken);

        return this.NoContentOrBadRequest(result);
    }

    [HttpGet(Id + ProgressRoute)]
    public async Task<ActionResult<ReadingChallengeProgressServiceModel?>> Progress(
        int id,
        CancellationToken cancellationToken = default)
        => this.Ok(await service.Progress(id, cancellationToken));

    [HttpPost(CheckInRoute)]
    public async Task<ActionResult> CheckInToday(
        CancellationToken cancellationToken = default)
    {
        var result = await service.CheckInToday(cancellationToken);
        return this.NoContentOrBadRequest(result);
    }

    [HttpGet(StreakRoute)]
    public async Task<ActionResult<ReadingStreakServiceModel>> Streak(
        CancellationToken cancellationToken = default)
        => this.Ok(await service.Streak(cancellationToken));
}
