namespace BookHub.Features.ReadingList.Web
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
            string userId,
            string status,
            int pageIndex = DefaultPageIndex,
            int pageSize = DefaultPageSize)
            => this.Ok(await this.service.All(userId, status, pageIndex, pageSize));

        [HttpPost]
        public async Task<ActionResult> Add(ReadingListWebModel webModel) 
        {
            var result = await this.service.Add(webModel.BookId, webModel.Status);

            return this.NoContentOrBadRequest(result);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(ReadingListWebModel webModel)
        {
            var result = await this.service.Delete(webModel.BookId, webModel.Status);

            return this.NoContentOrBadRequest(result);
        }
    }
}
