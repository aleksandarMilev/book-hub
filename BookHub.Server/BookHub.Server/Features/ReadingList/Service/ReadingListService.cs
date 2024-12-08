namespace BookHub.Server.Features.ReadingList.Service
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Book.Service.Models;
    using Data.Models;
    using Features.UserProfile.Data.Models;
    using Infrastructure.Services;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Server.Data;
    using UserProfile.Service;

    using static ErrorMessage;

    public class ReadingListService(
        BookHubDbContext data,
        ICurrentUserService userService,
        IProfileService profileService,
        IMapper mapper) : IReadingListService
    {
        private readonly BookHubDbContext data = data;
        private readonly ICurrentUserService userService = userService;
        private readonly IProfileService profileService = profileService;
        private readonly IMapper mapper = mapper;

        public async Task<PaginatedModel<BookServiceModel>> AllAsync(
            string userId,
            string status,
            int pageIndex,
            int pageSize)
        {
            var books = this.data
                .ReadingLists
                .Where(rl =>
                    rl.UserId == userId &&
                    rl.Status == ParseStatusToEnum(status))
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
            var userId = this.userService.GetId();
            var statusEnum = ParseStatusToEnum(status);

            if (statusEnum == ReadingListStatus.CurrentlyReading &&
                await this.profileService.MoreThanFiveCurrentlyReadingAsync(userId!))
            {
                return MoreThanFiveCurrentlyReading;
            }

            var mapEntity = new ReadingList()
            {
                UserId = userId!,
                BookId = bookId,
                Status = statusEnum,
            };

            try
            {
                this.data.Add(mapEntity);
                await this.data.SaveChangesAsync();
            }
            catch (SqlException)
            {
                return BookAlreadyInTheList;
            }

            await this.profileService.UpdateCountAsync(
                userId!,
                GetPropertyName(statusEnum),
                x => ++x);

            return true;
        }

        public async Task<Result> DeleteAsync(int bookId, string status)
        {
            var userId = this.userService.GetId();
            var statusEnum = ParseStatusToEnum(status);

            var mapEntity = await this.data
                .ReadingLists
                .FirstOrDefaultAsync(rl =>
                    rl.UserId == userId &&
                    rl.BookId == bookId &&
                    rl.Status == statusEnum);

            if (mapEntity is null) 
            {
                return BookNotInTheList;
            }

            this.data.Remove(mapEntity);
            await this.data.SaveChangesAsync();

            await this.profileService.UpdateCountAsync(
               userId!,
               GetPropertyName(statusEnum),
               x => --x);

            return true;
        }

        private static ReadingListStatus ParseStatusToEnum(string status)
        {
            if (Enum.TryParse(status, ignoreCase: true, out ReadingListStatus statusEnum))
            {
                return statusEnum;
            }

            throw new ReadingListTypeException(status);
        }

        private static string GetPropertyName(ReadingListStatus status)
            => status switch
            {
                ReadingListStatus.Read => nameof(UserProfile.ReadBooksCount),
                ReadingListStatus.ToRead => nameof(UserProfile.ToReadBooksCount),
                ReadingListStatus.CurrentlyReading => nameof(UserProfile.CurrentlyReadingBooksCount),
                _ => throw new ReadingListTypeException(status.ToString())
            };
    }
}
