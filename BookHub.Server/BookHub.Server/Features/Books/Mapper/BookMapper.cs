namespace BookHub.Server.Features.Books.Mapper
{
    using Authors.Service.Models;
    using AutoMapper;
    using Data.Models;
    using Service.Models;

    public class BookMapper : Profile
    {
        public BookMapper()
        {
            this.CreateMap<Book, BookListServiceModel>()
               .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.BooksGenres.Select(bg => bg.Genre.Name).ToList()))
               .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.Name));

            this.CreateMap<Book, BookDetailsServiceModel>()
                .IncludeBase<Book, BookListServiceModel>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author));

            this.CreateMap<Author, AuthorServiceModel>()
                .ForMember(dest => dest.BooksCount, opt => opt.MapFrom(src => src.Books.Count()));

            this.CreateMap<CreateBookServiceModel, Book>();
        }
    }
}
