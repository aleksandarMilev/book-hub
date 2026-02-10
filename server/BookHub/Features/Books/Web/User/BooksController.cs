namespace BookHub.Features.Books.Web.User;

using Common;
using Features.Authors.Service.Models;
using Features.Authors.Shared;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Service;
using Service.Models;
using Shared;

using static Common.Constants.ApiRoutes;
using static Common.Constants.DefaultValues;
using static Shared.Constants.RouteNames;

[Authorize]
public class BooksController(IBookService service) : ApiController
{
    [AllowAnonymous]
    [HttpGet(ApiRoutes.Top)]
    public async Task<ActionResult<IEnumerable<BookServiceModel>>> TopThree(
        CancellationToken cancellationToken = default)
        => this.Ok(await service.TopThree(cancellationToken));

    [HttpGet(ApiRoutes.ByGenre + Id)]
    public async Task<ActionResult<PaginatedModel<BookServiceModel>>> ByGenre(
        Guid id,
        int page = DefaultPageIndex,
        int pageSize = DefaultPageSize,
        CancellationToken cancellationToken = default) 
        => this.Ok(await service.ByGenre(
            id,
            page,
            pageSize,
            cancellationToken));

    [HttpGet(ApiRoutes.ByAuthor + Id)]
    public async Task<ActionResult<PaginatedModel<BookServiceModel>>> ByAuthor(
       Guid id,
       int page = DefaultPageIndex,
       int pageSize = DefaultPageSize,
       CancellationToken cancellationToken = default) 
       => this.Ok(await service.ByAuthor(
           id,
           page,
           pageSize,
           cancellationToken));

    [HttpGet(Id, Name = DetailsRouteName)]
    public async Task<ActionResult<BookDetailsServiceModel>> Details(
        Guid id,
        CancellationToken cancellationToken = default)
        => this.Ok(await service.Details(id, cancellationToken));

    [HttpPost]
    public async Task<ActionResult<AuthorDetailsServiceModel>> Create(
        CreateBookWebModel webModel,
        CancellationToken cancellationToken = default)
    {
        var serviceModel = webModel.ToCreateServiceModel();
        var createdBook = await service.Create(
            serviceModel,
            cancellationToken);

        return this.CreatedAtRoute(
            routeName: DetailsRouteName,
            routeValues: new { id = createdBook.Id },
            value: createdBook);
    }

    [HttpPut(Id)]
    public async Task<ActionResult> Edit(
        Guid id,
        CreateBookWebModel webModel,
        CancellationToken cancellationToken = default)
    {
        var serviceModel = webModel.ToCreateServiceModel();
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
