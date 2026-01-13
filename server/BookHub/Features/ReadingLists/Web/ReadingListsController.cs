namespace BookHub.Features.ReadingLists.Web;

using BookHub.Common;
using Books.Service.Models;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Service;
using Shared;

using static ApiRoutes;
using static Common.Constants.DefaultValues;

[Authorize]
public class ReadingListsController(IReadingListService service) : ApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookServiceModel>>> All(
        string userId,
        ReadingListStatus status,
        int pageIndex = DefaultPageIndex,
        int pageSize = DefaultPageSize,
        CancellationToken token = default)
    {
        var result = await service.All(
            userId,
            status,
            pageIndex,
            pageSize,
            token);

        if (result.Succeeded)
        {
            return this.Ok(result.Data);
        }

        return this.BadRequest(result.ErrorMessage);
    }

    [HttpGet(LastCurrentlyReadingRoute)]
    public async Task<ActionResult<BookServiceModel>> LastCurrentlyReading(
        string userId,
        CancellationToken cancelationToken = default)
        => this.Ok(await service.LastCurrentlyReading(userId, cancelationToken));

    [HttpPost]
    public async Task<ActionResult> Add(
        ReadingListWebModel webModel,
        CancellationToken cancelationToken = default) 
    {
        var serviceModel = webModel.ToServiceModel();
        var result = await service.Add(
            serviceModel,
            cancelationToken);

        return this.NoContentOrBadRequest(result);
    }

    [HttpDelete]
    public async Task<ActionResult> Delete(
        ReadingListWebModel webModel,
        CancellationToken cancelationToken = default)
    {
        var serviceModel = webModel.ToServiceModel();
        var result = await service.Delete(
            serviceModel,
            cancelationToken);

        return this.NoContentOrBadRequest(result);
    }
}
