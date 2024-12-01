namespace BookHub.Server.Features.ReadingList.Web
{
    using Book.Service.Models;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Service;

    using static Common.Constants.DefaultValues;

    [Authorize]
    public class ReadingListController(IReadingListService service) : ApiController
    {
        private readonly IReadingListService service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookServiceModel>>> All(
            string status,
            int pageIndex = DefaultPageIndex,
            int pageSize = DefaultPageSize) => this.Ok(await this.service.AllAsync(status, pageIndex, pageSize));

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Add(ReadingListWebModel webModel) 
        {
            var result = await this.service.AddAsync(webModel.BookId, webModel.Status);

            return this.NoContentOrBadRequest(result);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(ReadingListWebModel webModel)
        {
            var result = await this.service.DeleteAsync(webModel.BookId, webModel.Status);

            return this.NoContentOrBadRequest(result);
        }
    }
}
