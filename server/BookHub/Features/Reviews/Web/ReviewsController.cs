namespace BookHub.Features.Review.Web;

using BookHub.Common;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Service;
using Service.Models;
using Shared;

using static ApiRoutes;
using static Common.Constants.ApiRoutes;
using static Common.Constants.DefaultValues;

[Authorize]
public class ReviewsController(IReviewService service) : ApiController
{
    [HttpGet(ByBook + Id)]
    public async Task<ActionResult<PaginatedModel<ReviewServiceModel>>> AllForBook(
        Guid id,
        int pageIndex = DefaultPageIndex,
        int pageSize = DefaultPageSize,
        CancellationToken token = default)
        => this.Ok(await service.AllForBook(id, pageIndex, pageSize, token));

    [HttpGet(Id)]
    public async Task<ActionResult<ReviewServiceModel>> Details(
        Guid id,
        CancellationToken token = default)
        => this.Ok(await service.Details(id, token));

    [HttpPost]
    public async Task<ActionResult<ReviewServiceModel>> Create(
        CreateReviewWebModel webModel,
        CancellationToken token = default)
    {
        var serviceModel = webModel.ToServiceModel();
        var result = await service.Create(serviceModel, token);

        if (result.Data is null)
        {
            return this.BadRequest(result.ErrorMessage);
        }

        return this.CreatedAtAction(
            actionName: nameof(this.Details),
            routeValues: new { id = result.Data.Id },
            value: result.Data);
    }

    [HttpPut(Id)]
    public async Task<ActionResult> Edit(
        Guid id,
        CreateReviewWebModel webModel,
        CancellationToken token = default)
    {
        var serviceModel = webModel.ToServiceModel();
        var result = await service.Edit(id, serviceModel, token);

        return this.NoContentOrBadRequest(result);
    }

    [HttpDelete(Id)]
    public async Task<ActionResult> Delete(
        Guid id,
        CancellationToken token = default)
    {
        var result = await service.Delete(id, token);

        return this.NoContentOrBadRequest(result);
    }
}
