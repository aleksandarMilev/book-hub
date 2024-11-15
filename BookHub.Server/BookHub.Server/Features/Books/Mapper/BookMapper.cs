namespace BookHub.Server.Features.Books.Mapper
{
    using Authors.Service.Models;
    using AutoMapper;
    using Data.Models;
    using Service.Models;
    using Web.Models;

    public class BookMapper : Profile
    {
        public BookMapper()
        {
            this.CreateMap<Book, BookServiceModel>()
               .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.BooksGenres.Select(bg => bg.Genre.Name).ToList()))
               .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author == null ? null : src.Author.Name));

            this.CreateMap<Book, BookDetailsServiceModel>()
                .IncludeBase<Book, BookServiceModel>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author))
                .ForMember(dest => dest.PublishedDate, opt => opt.MapFrom(src => src.PublishedDate.ToString()));

            this.CreateMap<Author, AuthorServiceModel>()
                .ForMember(dest => dest.BooksCount, opt => opt.MapFrom(src => src.Books.Count()));

            this.CreateMap<CreateBookWebModel, CreateBookServiceModel>();

            this.CreateMap<CreateBookServiceModel, Book>()
                .ForMember(dest => dest.PublishedDate, opt => opt.MapFrom(src => MapperHelper.ParseDateTime(src.PublishedDate)));
        }
    }
}
