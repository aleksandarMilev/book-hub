namespace BookHub.Server.Features.Authors.Mapper
{
    using AutoMapper;
    using BookHub.Server.Features.Genre.Service.Models;
    using Books.Service.Models;
    using Data.Models;
    using Nationality.Service.Models;
    using Service.Models;
    using Web.Models;

    public class AuthorMapper : Profile
    {
        public AuthorMapper()
        {
            this.CreateMap<Genre, GenreNameServiceModel>();

            this.CreateMap<Book, BookServiceModel>()
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.BooksGenres.Select(bg => bg.Genre)));

            this.CreateMap<CreateAuthorWebModel, CreateAuthorServiceModel>();

            this.CreateMap<CreateAuthorServiceModel, Author>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => MapperHelper.ParseGender(src.Gender)))
                .ForMember(dest => dest.BornAt, opt => opt.MapFrom(src => MapperHelper.ParseDateTime(src.BornAt)))
                .ForMember(dest => dest.DiedAt, opt => opt.MapFrom(src => MapperHelper.ParseDateTime(src.DiedAt)));

            this.CreateMap<Nationality, NationalityServiceModel>();

            this.CreateMap<Author, AuthorDetailsServiceModel>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.BornAt, opt => opt.MapFrom(src => src.BornAt != null ? src.BornAt.ToString() : null))
                .ForMember(dest => dest.DiedAt, opt => opt.MapFrom(src => src.DiedAt != null ? src.DiedAt.ToString() : null))
                .ForMember(dest => dest.BooksCount, opt => opt.MapFrom(src => src.Books.Count()))
                .ForMember(dest => dest.TopBooks, opt => opt.MapFrom(src => src.Books.Take(3)));

            this.CreateMap<Author, AuthorServiceModel>();
        }
    }
}
