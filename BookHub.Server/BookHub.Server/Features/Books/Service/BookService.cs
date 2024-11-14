namespace BookHub.Server.Features.Books.Service
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;
    using Models;

    using static Common.Constants.DefaultValues;
    using static Common.Messages.Error.Book;

    public class BookService(
        BookHubDbContext data,
        IMapper mapper) : IBookService
    {
        private readonly BookHubDbContext data = data;
        private readonly IMapper mapper = mapper;

        public async Task<IEnumerable<string>> GetGenreNamesAsync()
           => await this.data
               .Genres
               .Select(g => g.Name)
               .ToListAsync();

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
            book.ImageUrl ??= DefaultBookImageUrl;

            var authorId = await this.GetAuthorIdByNameAsync(model.AuthorName);

            if (authorId != 0)
            {
                book.AuthorId = authorId;
            }

            this.data.Add(book);
            await this.data.SaveChangesAsync();

            await this.MapBookAndGenreAsync(book.Id, model.Genres);

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

            await this.MapBookAndGenreAsync(book.Id, model.Genres);

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

        private async Task MapBookAndGenreAsync(int bookId, IEnumerable<string> genreNames)
        {
            foreach (var name in genreNames)
            {
                var genreId = await this.GetGenreIdByNameAsync(name);

                if (genreId != 0)
                {
                    var bookGenre = new BookGenre()
                    {
                        BookId = bookId,
                        GenreId = genreId
                    };

                    this.data.Add(bookGenre);
                }
            }

            await this.data.SaveChangesAsync();
        }

        private async Task<int> GetGenreIdByNameAsync(string name)
            => await this.data
                .Genres
                .Where(g => g.Name == name)
                .Select(g => g.Id)
                .FirstOrDefaultAsync();
    }
}
