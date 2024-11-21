namespace BookHub.Server.Features.Books.Service
{
    using Authors.Service.Models;
    using AutoMapper;
    using Genre.Service.Models;
    using Data;
    using Data.Models;
    using Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;
    using Models;

    using static Common.Constants.DefaultValues;
    using static Common.Messages.Error.Book;
    using BookHub.Server.Features.Review.Service.Models;

    public class BookService(
        BookHubDbContext data,
        IMapper mapper) : IBookService
    {
        private readonly BookHubDbContext data = data;
        private readonly IMapper mapper = mapper;

        public async Task<IEnumerable<BookServiceModel>> GetAllAsync()
            => await this.data
                .Books
                .Select(b => new BookServiceModel()
                {
                    Id = b.Id,
                    Title = b.Title,
                    AuthorName = b.Author == null ? null : b.Author.Name,
                    ImageUrl = b.ImageUrl,
                    ShortDescription = b.ShortDescription,
                    AverageRating = b.Id,
                    Genres = b
                        .BooksGenres
                    .Select(bg => new GenreNameServiceModel()
                    {
                        Id = bg.GenreId,
                        Name = bg.Genre.Name
                        })
                        .ToHashSet()

                    })
                .ToListAsync();

        public async Task<IEnumerable<BookServiceModel>> GetTopThreeAsync()
            => await this.data
               .Books
                .Select(b => new BookServiceModel()
                {
                    Id = b.Id,
                    Title = b.Title,
                    AuthorName = b.Author == null ? null : b.Author.Name,
                    ImageUrl = b.ImageUrl,
                    ShortDescription = b.ShortDescription,
                    AverageRating = b.AverageRating,
                    Genres = b
                        .BooksGenres
                        .Select(bg => new GenreNameServiceModel() 
                        {
                            Id = bg.GenreId,
                            Name = bg.Genre.Name
                        })
                        .ToHashSet()
                    
                })
               .OrderByDescending(b => b.AverageRating)
               .Take(3)
               .ToListAsync();


        public async Task<BookDetailsServiceModel?> GetDetailsAsync(int id, string userId)
            => await this.data
                  .Books
                  .Select(b => new BookDetailsServiceModel()
                  {
                      Id = b.Id,
                      Title = b.Title,
                      AuthorName = b.Author == null ? null : b.Author.Name,
                      ImageUrl = b.ImageUrl,
                      ShortDescription = b.ShortDescription,
                      AverageRating = b.AverageRating,
                      Genres = b
                        .BooksGenres
                        .Select(bg => new GenreNameServiceModel()
                        {
                            Id = bg.GenreId,
                            Name = bg.Genre.Name
                        })
                        .ToHashSet(),
                      PublishedDate = b.PublishedDate == null ? null : b.PublishedDate.ToString(),
                      RatingsCount = b.RatingsCount,
                      LongDescription = b.LongDescription,
                      CreatorId = b.CreatorId,
                      Author = b.Author == null 
                        ? null
                        : new AuthorServiceModel()
                          {
                              Id = b.Author.Id,
                              Name = b.Author.Name,
                              ImageUrl = b.Author.ImageUrl,
                              Biography = b.Author.Biography,
                              BooksCount = b.Author.Books.Count(),
                              Rating = b.Author.Rating,
                          },
                      Reviews = b
                        .Reviews
                        .OrderByDescending(r => r.CreatedBy == userId)
                        .ThenByDescending(r => r.CreatedOn)
                        .Select(r => new ReviewServiceModel() 
                        {
                            Id = r.Id,
                            Content = r.Content,
                            Rating = r.Rating,
                            Likes = r.Likes,
                            Dislikes = r.Dislikes,
                            CreatorId = r.CreatorId,
                            BookId = r.BookId,
                            CreatedBy = r.CreatedBy!,
                        })
                       .ToHashSet()
                  })
                .FirstOrDefaultAsync(b => b.Id == id);

        public async Task<int> CreateAsync(CreateBookServiceModel model, string userId)
        {
            model.ImageUrl ??= DefaultBookImageUrl;

            var book = this.mapper.Map<Book>(model);
            book.CreatorId = userId;
            book.AuthorId = await this.MapAuthorToBookAsync(model.AuthorId);

            this.data.Add(book);
            await this.data.SaveChangesAsync();

            await this.MapBookAndGenresAsync(book.Id, model.Genres);

            return book.Id;
        }

        public async Task<Result> EditAsync(int id, CreateBookServiceModel model, string userId)
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
                return UnauthorizedBookEdit;
            }

            model.ImageUrl ??= DefaultBookImageUrl;
            this.mapper.Map(model, book);
            book.AuthorId = await this.MapAuthorToBookAsync(model.AuthorId);

            await this.data.SaveChangesAsync();

            await this.MapBookAndGenresAsync(book.Id, model.Genres);

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

        private async Task<int?> MapAuthorToBookAsync(int? id) 
        {
            var authorId = await this.data
                .Authors
                .Select(a => a.Id)
                .FirstOrDefaultAsync(a => a == id);

            if (authorId == 0)
            {
                return null;
            }

            return authorId;
        }

        private async Task MapBookAndGenresAsync(int bookId, IEnumerable<int> genreIds)
        {
            await this.RemoveExistingBookGenres(bookId);

            if (genreIds.Any())
            {
                foreach (var genreId in genreIds)
                {
                    if (await this.BookGenreExistsAsync(bookId, genreId))
                    {
                        continue;
                    }

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
                var otherGenreId = await this.data
                    .Genres
                    .Where(g => g.Name == OtherGenreName)
                    .Select(g => g.Id)
                    .FirstOrDefaultAsync();

                if (!await this.BookGenreExistsAsync(bookId, otherGenreId))
                {
                    var bookGenre = new BookGenre()
                    {
                        BookId = bookId,
                        GenreId = otherGenreId
                    };

                    this.data.Add(bookGenre);
                }
            }

            await this.data.SaveChangesAsync();
        }

        private async Task<bool> BookGenreExistsAsync(int bookId, int genreId)
            => await this.data
                .BooksGenres
                .AsNoTracking()
                .AnyAsync(bg => bg.BookId == bookId && bg.GenreId == genreId);

        private async Task RemoveExistingBookGenres(int bookId) 
        {
            var existingMaps = await this.data
               .BooksGenres
               .Where(bg => bg.BookId == bookId)
               .ToListAsync();

            this.data.RemoveRange(existingMaps);
            await this.data.SaveChangesAsync();
        }
    }
}
