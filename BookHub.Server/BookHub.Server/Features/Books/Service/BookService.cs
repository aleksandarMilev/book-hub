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

        public async Task<IEnumerable<BookServiceModel>> GetAllAsync()
            => await this.data
                .Books
                .ProjectTo<BookServiceModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<IEnumerable<BookServiceModel>> GetTopThreeAsync()
            => await this.data
               .Books
               .ProjectTo<BookServiceModel>(this.mapper.ConfigurationProvider)
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
            model.ImageUrl ??= DefaultBookImageUrl;
            var book = this.mapper.Map<Book>(model);

            var authorId = await this.data
                .Authors
                .Select(a => a.Id)
                .FirstOrDefaultAsync(id => id == model.AuthorId);

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
            //var book = await this.data
            //     .Books
            //     .FindAsync(id);

            //if (book is null)
            //{
            //    return BookNotFound;
            //}

            //if (book.CreatorId != model.CreatorId)
            //{
            //    return UnauthorizedBookEdit;
            //}

            //this.mapper.Map(model, book);

            //var authorId = await this.GetAuthorIdByNameAsync(model.AuthorName);

            //if (authorId != 0)
            //{
            //    book.AuthorId = authorId;
            //}

            //await this.data.SaveChangesAsync();

            //await this.MapBookAndGenreAsync(book.Id, model.Genres);

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

        private async Task MapBookAndGenreAsync(int bookId, IEnumerable<int> genreIds)
        {
            if (genreIds.Any())
            {
                foreach (var genreId in genreIds)
                {
                    var bookGenre = new BookGenre()
                    {
                        BookId = bookId,
                        GenreId = genreId
                    };

                    this.data.Add(bookGenre);
                }
            }
            else
            {
                var bookGenre = new BookGenre()
                {
                    BookId = bookId,
                    GenreId = await this.data
                        .Genres
                        .Where(g => g.Name == OtherGenreName)
                        .Select(g => g.Id)
                        .FirstOrDefaultAsync()
                };

                this.data.Add(bookGenre);
            }

            await this.data.SaveChangesAsync();
        }
    }
}
