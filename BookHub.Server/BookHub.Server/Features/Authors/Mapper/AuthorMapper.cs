namespace BookHub.Server.Features.Authors.Mapper
{
    using AutoMapper;
    using Books.Service.Models;
    using Data.Models;
    using Service.Models;
    using Web.Models;

    public class AuthorMapper : Profile
    {
        public AuthorMapper()
        {
            this.CreateMap<Book, BookServiceModel>()
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.BooksGenres.Select(bg => bg.Genre.Name).ToList()));

            this.CreateMap<CreateAuthorWebModel, CreateAuthorServiceModel>();

            this.CreateMap<CreateAuthorServiceModel, Author>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Nationality, opt => opt.Ignore())
                .ForMember(dest => dest.Gender,
                           opt => opt.MapFrom(src => MapperHelper.ParseGender(src.Gender)))
                .ForMember(dest => dest.BornAt,
                           opt => opt.MapFrom(src => MapperHelper.ParseDateTime(src.BornAt)))
                .ForMember(dest => dest.DiedAt,
                           opt => opt.MapFrom(src => MapperHelper.ParseDateTime(src.DiedAt)));

            this.CreateMap<Author, AuthorDetailsServiceModel>()
                .ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => src.Nationality.Name))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.BornAt, opt => opt.MapFrom(src => src.BornAt != null ? src.BornAt.ToString() : null))
                .ForMember(dest => dest.DiedAt, opt => opt.MapFrom(src => src.DiedAt != null ? src.DiedAt.ToString() : null))
                .ForMember(dest => dest.BooksCount, opt => opt.MapFrom(src => src.Books.Count()))
                .ForMember(dest => dest.TopBooks, opt => opt.MapFrom(src => src.Books.Take(3)));
                   
            this.CreateMap<CreateAuthorWebModel, CreateAuthorServiceModel>();

            this.CreateMap<Author, AuthorServiceModel>();
        }
    }
}
