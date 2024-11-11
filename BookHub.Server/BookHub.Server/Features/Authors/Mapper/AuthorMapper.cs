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

            CreateMap<AuthorDetailsServiceModel, Author>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Nationality, opt => opt.Ignore())
                .ForMember(dest => dest.Gender,
                           opt => opt.MapFrom(src => MapperHelper.ParseGender(src.Gender))) 
                .ForMember(dest => dest.BornAt,
                           opt => opt.MapFrom(src => MapperHelper.ParseDateTime(src.BornAt))) 
                .ForMember(dest => dest.DiedAt,
                           opt => opt.MapFrom(src => MapperHelper.ParseDateTime(src.DiedAt))); 
        }
    }
}
