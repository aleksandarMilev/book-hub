namespace BookHub.Server.Features.Books.Service
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;
    using Models;

    using static Common.Messages.Error.Book;

    public class BookService(
        BookHubDbContext data,
        IMapper mapper) : IBookService
    {
        private readonly BookHubDbContext data = data;
        private readonly IMapper mapper = mapper;

        public async Task<IEnumerable<BookListServiceModel>> GetAllAsync()
            => await this.data
                .Books
                .ProjectTo<BookListServiceModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<IEnumerable<BookListServiceModel>> GetTopThreeAsync()
            => await this.data
               .Books
               .ProjectTo<BookListServiceModel>(this.mapper.ConfigurationProvider)
               .OrderByDescending(b => b.AverageRating)
               .Take(3)
               .ToListAsync();


        public async Task<BookDetailsServiceModel?> GetDetailsAsync(int id)
            => await this.data
                .Books
                .ProjectTo<BookDetailsServiceModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(b => b.Id == id);

        public async Task<int> CreateAsync(CreateBookServiceModel model)
        {
            var book = this.mapper.Map<Book>(model);
            var authorId = await this.GetAuthorIdByNameAsync(model.AuthorName);

            if (authorId != 0)
            {
                book.AuthorId = authorId;
            }

            this.data.Add(book);
            await this.data.SaveChangesAsync();

            return book.Id;
        }

        public async Task<Result> EditAsync(int id, CreateBookServiceModel model)
        {
            var book = await this.data
                 .Books
                 .FindAsync(id);

            if (book is null)
            {
                return BookNotFound;
            }

            if (book.CreatorId != model.CreatorId)
            {
                return UnauthorizedBookEdit;
            }

            this.mapper.Map(model, book);

            var authorId = await this.GetAuthorIdByNameAsync(model.AuthorName);

            if (authorId != 0)
            {
                book.AuthorId = authorId;
            }

            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<Result> DeleteAsync(int id, string userId)
        {
            var book = await this.data
                 .Books
                 .FindAsync(id);

            if (book is null)
            {
                return BookNotFound;
            }

            if (book.CreatorId != userId)
            {
                return UnauthorizedBookDelete;
            }

            this.data.Remove(book);
            await this.data.SaveChangesAsync();

            return true;
        }

        private async Task<int> GetAuthorIdByNameAsync(string? name)
            => await this.data
                .Authors
                .Where(a => a.Name == name)
                .Select(a => a.Id)
                .FirstOrDefaultAsync();
    }
}
