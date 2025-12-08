namespace BookHub.Features.ReadingList.Service
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Book.Service.Models;
    using BookHub.Common;
    using BookHub.Data;
    using BookHub.Infrastructure.Services.CurrentUser;
    using BookHub.Infrastructure.Services.Result;
    using Data.Models;
    using Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;
    using UserProfile.Data.Models;
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

        public async Task<PaginatedModel<BookServiceModel>> All(
            string userId,
            string status,
            int pageIndex,
            int pageSize)
        {
            var enumStatus = ParseStatusToEnum(status);

            var books = this.data
                .ReadingLists
                .Where(rl =>
                    rl.UserId == userId &&
                    rl.Status == enumStatus)
                .ProjectTo<BookServiceModel>(this.mapper.ConfigurationProvider);

            var total = await books.CountAsync();
            var paginatedBooks = await books
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedModel<BookServiceModel>(
                paginatedBooks,
                total,
                pageIndex,
                pageSize);
        }

        public async Task<Result> Add(Guid bookId, string status)
        {
            var userId = this.userService.GetId();
            var statusEnum = ParseStatusToEnum(status);

            if (statusEnum == ReadingListStatus.CurrentlyReading &&
                await this.profileService.HasMoreThanFiveCurrentlyReading(userId!))
            {
                return MoreThanFiveCurrentlyReading;
            }
          
            var exists = await this.data
               .ReadingLists
               .AnyAsync(rl =>
                   rl.UserId == userId &&
                   rl.BookId == bookId &&
                   rl.Status == statusEnum);

            if (exists)
            {
                return BookAlreadyInTheList;
            }

            var mapEntity = new ReadingList()
            {
                UserId = userId!,
                BookId = bookId,
                Status = statusEnum,
            };

            this.data.Add(mapEntity);
            await this.data.SaveChangesAsync();

            await this.profileService.UpdateCount(
                userId!,
                GetPropertyName(statusEnum),
                x => ++x);

            return true;
        }

        public async Task<Result> Delete(Guid bookId, string status)
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

            await this.profileService.UpdateCount(
               userId!,
               GetPropertyName(statusEnum),
               x => --x);

            return true;
        }

        private static ReadingListStatus ParseStatusToEnum(string status)
        {
            if (Enum.TryParse(
                status,
                ignoreCase: true,
                out ReadingListStatus statusEnum))
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
