namespace BookHub.Features.ReadingList.Web;

using AutoMapper;
using Book.Service.Models;
using BookHub.Common;
using Data.Models;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Service;
using Shared;

using static Common.Constants.DefaultValues;

[Authorize]
public class ReadingListController(IReadingListService service) : ApiController
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

    [HttpPost]
    public async Task<ActionResult> Add(
        ReadingListWebModel webModel,
        CancellationToken token = default) 
    {
        var serviceModel = webModel.ToServiceModel();
        var result = await service.Add(
            serviceModel,
            token);

        return this.NoContentOrBadRequest(result);
    }

    [HttpDelete]
    public async Task<ActionResult> Delete(
        ReadingListWebModel webModel,
        CancellationToken token = default)
    {
        var serviceModel = webModel.ToServiceModel();
        var result = await service.Delete(
            serviceModel,
            token);

        return this.NoContentOrBadRequest(result);
    }
}
