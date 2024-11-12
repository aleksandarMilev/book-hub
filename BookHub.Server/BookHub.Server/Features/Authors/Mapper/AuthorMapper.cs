namespace BookHub.Server.Features.Authors.Mapper
{
    using AutoMapper;
    using Data.Models;
    using Service.Models;
    using Web.Models;

    using static Common.Constants.DefaultValues;

    public class AuthorMapper : Profile
    {
        public AuthorMapper()
        {
            this.CreateMap<CreateAuthorWebModel, AuthorDetailsServiceModel>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl ?? DefaultAuthorImageUrl));

            this.CreateMap<AuthorDetailsServiceModel, Author>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Nationality, opt => opt.Ignore())
                .ForMember(dest => dest.Gender,
                           opt => opt.MapFrom(src => MapperHelper.ParseGender(src.Gender)))
                .ForMember(dest => dest.BornAt,
                           opt => opt.MapFrom(src => MapperHelper.ParseDateTime(src.BornAt)))
                .ForMember(dest => dest.DiedAt,
                           opt => opt.MapFrom(src => MapperHelper.ParseDateTime(src.DiedAt)))
                .ReverseMap()
                    .ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => src.Nationality.Name))
                    .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                    .ForMember(dest => dest.DiedAt, opt => opt.MapFrom(src => src.BornAt != null ? src.DiedAt.ToString() : null))
                    .ForMember(dest => dest.DiedAt, opt => opt.MapFrom(src => src.DiedAt != null ? src.DiedAt.ToString() : null))
                    .ForMember(dest => dest.BooksCount, opt => opt.MapFrom(src => src.Books.Count()));
        }
    }
}
