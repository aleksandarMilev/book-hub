﻿namespace BookHub.Server.Features.Authors.Service
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;
    using Models;

    using static Common.Constants.DefaultValues;
    using static Common.Messages.Error.Author;

    public class AuthorService(
        BookHubDbContext data,
        ICurrentUserService userService,
        IMapper mapper) : IAuthorService
    {
        private readonly BookHubDbContext data = data;
        private readonly ICurrentUserService userService = userService;
        private readonly IMapper mapper = mapper;

        public async Task<IEnumerable<AuthorNamesServiceModel>> GetNamesAsync()
          => await this.data
              .Authors
              .Select(a => new AuthorNamesServiceModel() 
              {
                  Id = a.Id,
                  Name = a.Name
              })
              .ToListAsync();

        public async Task<IEnumerable<AuthorServiceModel>> GetTopThreeAsync()
            => await this.data
                .Authors
                .ProjectTo<AuthorServiceModel>(this.mapper.ConfigurationProvider)
                .OrderByDescending(a => a.AverageRating)
                .Take(3)
                .ToListAsync();

        public async Task<AuthorDetailsServiceModel?> GetDetailsAsync(int id)
            => await this.data
                .Authors
                .ProjectTo<AuthorDetailsServiceModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(a => a.Id == id);

        public async Task<int> CreateAsync(CreateAuthorServiceModel model)
        {
            model.ImageUrl ??= DefaultAuthorImageUrl;

            var author = this.mapper.Map<Author>(model);
            author.CreatorId = this.userService.GetId()!;
            author.NationalityId = await this.MapNationalityToAuthor(model.NationalityId);

            this.data.Add(author);
            await this.data.SaveChangesAsync();

            return author.Id;
        }

        public async Task<Result> EditAsync(int id, CreateAuthorServiceModel model)
        {
            var author = await this.data
                 .Authors
                 .FindAsync(id);

            if (author is null)
            {
                return AuthorNotFound;
            }

            if (author.CreatorId != this.userService.GetId()!)
            {
                return UnauthorizedAuthorEdit;
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
                return AuthorNotFound;
            }

            if (author.CreatorId != this.userService.GetId()!)
            {
                return UnauthorizedAuthorDelete;
            }

            this.data.Remove(author);
            await this.data.SaveChangesAsync();

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
