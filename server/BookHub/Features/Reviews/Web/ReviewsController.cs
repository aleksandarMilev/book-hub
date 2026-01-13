namespace BookHub.Features.Reviews.Web;

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
using static Shared.Constants.RouteNames;


[Authorize]
public class ReviewsController(IReviewService service) : ApiController
{
    [HttpGet(ByBook + Id)]
    public async Task<ActionResult<PaginatedModel<ReviewServiceModel>>> AllForBook(
        Guid id,
        int pageIndex = DefaultPageIndex,
        int pageSize = DefaultPageSize,
        CancellationToken cancellationToken = default)
        => this.Ok(await service.AllForBook(
            id,
            pageIndex,
            pageSize,
            cancellationToken));

    [HttpGet(Id, Name = DetailsRouteName)]
    public async Task<ActionResult<ReviewServiceModel>> Details(
        Guid id,
        CancellationToken cancellationToken = default)
        => this.Ok(await service.Details(id, cancellationToken));

    [HttpPost]
    public async Task<ActionResult<ReviewServiceModel>> Create(
        CreateReviewWebModel webModel,
        CancellationToken cancellationToken = default)
    {
        var serviceModel = webModel.ToServiceModel();
        var result = await service.Create(
            serviceModel,
            cancellationToken);

        if (result.Succeeded)
        {
            return this.CreatedAtRoute(
                routeName: DetailsRouteName,
                routeValues: new { id = result.Data!.Id },
                value: result.Data);
        }

        return this.BadRequest(result.ErrorMessage);
    }

    [HttpPut(Id)]
    public async Task<ActionResult> Edit(
        Guid id,
        CreateReviewWebModel webModel,
        CancellationToken cancellationToken = default)
    {
        var serviceModel = webModel.ToServiceModel();
        var result = await service.Edit(
            id,
            serviceModel,
            cancellationToken);

        return this.NoContentOrBadRequest(result);
    }

    [HttpDelete(Id)]
    public async Task<ActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await service.Delete(
            id,
            cancellationToken);

        return this.NoContentOrBadRequest(result);
    }
}
