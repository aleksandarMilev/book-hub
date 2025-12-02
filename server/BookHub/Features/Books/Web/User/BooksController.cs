namespace BookHub.Features.Book.Web.User;

using Features.Authors.Service.Models;
using Features.Authors.Shared;
using Features.Book.Shared;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Service;
using Service.Models;

using static Common.Constants.ApiRoutes;
using static Common.Constants.DefaultValues;

[Authorize]
public class BooksController(IBookService service) : ApiController
{
    [AllowAnonymous]
    [HttpGet(ApiRoutes.Top)]
    public async Task<ActionResult<IEnumerable<BookServiceModel>>> TopThree(
        CancellationToken token = default)
      => this.Ok(await service.TopThree(token));

    [HttpGet(ApiRoutes.ByGenre + Id)]
    public async Task<ActionResult<PaginatedModel<BookServiceModel>>> ByGenre(
        Guid id,
        int page = DefaultPageIndex,
        int pageSize = DefaultPageSize,
        CancellationToken token = default) 
        => this.Ok(await service.ByGenre(id, page, pageSize, token));

    [HttpGet(ApiRoutes.ByAuthor + Id)]
    public async Task<ActionResult<PaginatedModel<BookServiceModel>>> ByAuthor(
       Guid id,
       int page = DefaultPageIndex,
       int pageSize = DefaultPageSize,
       CancellationToken token = default) 
       => this.Ok(await service.ByAuthor(id, page, pageSize, token));

    [HttpGet(Id)]
    public async Task<ActionResult<BookDetailsServiceModel>> Details(
        Guid id,
        CancellationToken token = default)
        => this.Ok(await service.Details(id, token));

    [HttpPost]
    public async Task<ActionResult<AuthorDetailsServiceModel>> Create(
        CreateBookWebModel webModel,
        CancellationToken token = default)
    {
        var serviceModel = webModel.ToCreateServiceModel();
        var createdBook = await service.Create(serviceModel, token);

        return this.CreatedAtRoute(
            routeName: nameof(this.Details),
            routeValues: new { id = createdBook.Id },
            value: createdBook);
    }

    [HttpPut(Id)]
    public async Task<ActionResult> Edit(
        Guid id,
        CreateBookWebModel webModel,
        CancellationToken token = default)
    {
        var serviceModel = webModel.ToCreateServiceModel();
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
