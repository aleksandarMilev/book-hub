namespace BookHub.Server.Features.ReadingList.Service
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Book.Service.Models;
    using Data;
    using Data.Models;
    using Data.Models.Enums;
    using Infrastructure.Services;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;

    using static Common.Messages.Error.ReadingList;

    public class ReadingListService(
        BookHubDbContext data,
        ICurrentUserService userService,
        IMapper mapper) : IReadingListService
    {
        private readonly BookHubDbContext data = data;
        private readonly ICurrentUserService userService = userService;
        private readonly IMapper mapper = mapper;

        public async Task<PaginatedModel<BookServiceModel>> AllAsync(
            string status,
            int pageIndex,
            int pageSize)
        {
            var books = this.data
                .ReadingLists
                .Where(rl =>
                    rl.UserId == this.userService.GetId() &&
                    rl.Status == ParseToEnum(status))
                .ProjectTo<BookServiceModel>(this.mapper.ConfigurationProvider);

            var total = await books.CountAsync();

            var paginatedBooks = await books
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedModel<BookServiceModel>(paginatedBooks, total, pageIndex, pageSize);
        }

        public async Task<Result> AddAsync(int bookId, string status)
        {
            var mapEntity = new ReadingList()
            {
                UserId = this.userService.GetId()!,
                BookId = bookId,
                Status = ParseToEnum(status),
            };

            try
            {
                this.data.Add(mapEntity);
                await this.data.SaveChangesAsync();

                return true;
            }
            catch (SqlException)
            {
                return BookAlreadyInTheList;
            }
        }

        public async Task<Result> DeleteAsync(int bookId, string status)
        {
            var mapEntity = await this.data
                .ReadingLists
                .FirstOrDefaultAsync(rl =>
                    rl.UserId == this.userService.GetId() && 
                    rl.BookId == bookId && 
                    rl.Status == ParseToEnum(status));

            if (mapEntity is null) 
            {
                return BookNotInTheList;
            }

            this.data.Remove(mapEntity);
            await this.data.SaveChangesAsync();

            return true;
        }

        private static ReadingListStatus ParseToEnum(string status)
        {
            if (Enum.TryParse(status, ignoreCase: true, out ReadingListStatus statusEnum))
            {
                return statusEnum;
            }

            throw new InvalidOperationException("Invalid reading list status type!");
        }
    }
}
