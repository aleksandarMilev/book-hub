namespace BookHub.Server.Features.Authors.Service
{
    using Areas.Admin.Service;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data.Models;
    using Features.UserProfile.Data.Models;
    using Infrastructure.Extensions;
    using Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Notification.Service;
    using Server.Data;
    using UserProfile.Service;

    using static Common.ErrorMessage;

    public class AuthorService(
        BookHubDbContext data,
        ICurrentUserService userService,
        IAdminService adminService,
        INotificationService notificationService,
        IProfileService profileService,
        IMapper mapper) : IAuthorService
    {
        private const string DefaultAuthorImageUrl = "https://famouswritingroutines.com/wp-content/uploads/2022/06/daily-word-counts-of-famous-authors-1140x761.jpg";
        private const string UnknownNationalityName = "Unknown";
        private const int TopThreeCount = 3;

        private readonly BookHubDbContext data = data;
        private readonly ICurrentUserService userService = userService;
        private readonly IAdminService adminService = adminService;
        private readonly INotificationService notificationService = notificationService;
        private readonly IProfileService profileService = profileService;
        private readonly IMapper mapper = mapper;

        public async Task<IEnumerable<AuthorNamesServiceModel>> NamesAsync()
          => await this.data
              .Authors
              .Select(a => new AuthorNamesServiceModel() 
              {
                  Id = a.Id,
                  Name = a.Name
              })
              .ToListAsync();

        public async Task<IEnumerable<AuthorServiceModel>> TopThreeAsync()
            => await this.data
                .Authors
                .ProjectTo<AuthorServiceModel>(this.mapper.ConfigurationProvider)
                .OrderByDescending(a => a.AverageRating)
                .Take(TopThreeCount)
                .ToListAsync();

        public async Task<AuthorDetailsServiceModel?> DetailsAsync(int id)
            => await this.data
                .Authors
                .ProjectTo<AuthorDetailsServiceModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(a => a.Id == id);

        public async Task<AuthorDetailsServiceModel?> AdminDetailsAsync(int id)
             => await this.data
                 .Authors
                 .IgnoreQueryFilters()
                 .ApplyIsDeletedFilter()
                 .ProjectTo<AuthorDetailsServiceModel>(this.mapper.ConfigurationProvider)
                 .FirstOrDefaultAsync(a => a.Id == id);

        public async Task<int> CreateAsync(CreateAuthorServiceModel model)
        {
            model.ImageUrl ??= DefaultAuthorImageUrl;

            var author = this.mapper.Map<Author>(model);
            author.CreatorId = this.userService.GetId();
            author.NationalityId = await this.MapNationalityToAuthor(model.NationalityId);

            var userIsAdmin = this.userService.IsAdmin();

            if (userIsAdmin)
            {
                author.IsApproved = true;
            }

            this.data.Add(author);
            await this.data.SaveChangesAsync();

            if (!userIsAdmin)
            {
                await this.notificationService.CreateOnEntityCreationAsync(
                    author.Id,
                    nameof(Author), 
                    author.Name,
                    await this.adminService.GetIdAsync());
            }

            return author.Id;
        }

        public async Task<Result> EditAsync(int id, CreateAuthorServiceModel model)
        {
            var author = await this.data
                .Authors
                .FindAsync(id);

            if (author is null)
            {
                return string.Format(DbEntityNotFound, nameof(Author), id);
            }

            if (author.CreatorId != this.userService.GetId())
            {
                return string.Format(
                    UnauthorizedDbEntityAction,
                    this.userService.GetUsername(),
                    nameof(Author),
                    id);
            }

            model.ImageUrl ??= DefaultAuthorImageUrl;

            this.mapper.Map(model, author);

            author.NationalityId = await this.MapNationalityToAuthor(model.NationalityId);

            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var author = await this.data
                 .Authors
                 .FindAsync(id);

            if (author is null)
            {
                return string.Format(DbEntityNotFound, nameof(Author), id);
            }

            if (author.CreatorId != this.userService.GetId() &&
                !this.userService.IsAdmin())
            {
                return string.Format(
                    UnauthorizedDbEntityAction,
                    this.userService.GetUsername(),
                    nameof(Author),
                    id);
            }

            this.data.Remove(author);
            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<Result> ApproveAsync(int id)
        {
            var author = await this.data
                 .Authors
                 .IgnoreQueryFilters()
                 .ApplyIsDeletedFilter()
                 .FirstOrDefaultAsync(a => a.Id == id);

            if (author is null)
            {
                return string.Format(DbEntityNotFound, nameof(Author), id);
            }

            if (!this.userService.IsAdmin())
            {
                return string.Format(
                    UnauthorizedDbEntityAction,
                    this.userService.GetUsername(),
                    nameof(Author),
                    id);
            }

            author.IsApproved = true;
            await this.data.SaveChangesAsync();

            await this.notificationService.CreateOnEntityApprovalStatusChangeAsync(
                id,
                nameof(Author),
                author.Name,
                author.CreatorId!,
                true);

            await this.profileService.UpdateCountAsync(
                this.userService.GetId()!,
                nameof(UserProfile.CreatedAuthorsCount),
                x => ++x);

            return true;
        }

        public async Task<Result> RejectAsync(int id)
        {
            var author = await this.data
                 .Authors
                 .IgnoreQueryFilters()
                 .ApplyIsDeletedFilter()
                 .FirstOrDefaultAsync(a => a.Id == id);

            if (author is null)
            {
                return string.Format(DbEntityNotFound, nameof(Author), id);
            }

            if (!this.userService.IsAdmin())
            {
                return string.Format(
                    UnauthorizedDbEntityAction,
                    this.userService.GetUsername(),
                    nameof(Author),
                    id);
            }

            this.data.Remove(author);
            await this.data.SaveChangesAsync();

            await this.notificationService.CreateOnEntityApprovalStatusChangeAsync(
                id,
                nameof(Author),
                author.Name,
                author.CreatorId!,
                false);

            return true;
        }

        private async Task<int> MapNationalityToAuthor(int? id)
        {
            var nationalityId = await this.data
               .Nationalities
               .Select(n => n.Id)
               .FirstOrDefaultAsync(n => n == id);

            if (nationalityId == 0)
            {
                nationalityId = await this.data
                   .Nationalities
                   .Where(n => n.Name == UnknownNationalityName)
                   .Select(n => n.Id)
                   .FirstOrDefaultAsync();
            }

            return nationalityId;
        }
    }
}
