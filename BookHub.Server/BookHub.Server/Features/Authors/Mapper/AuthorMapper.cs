namespace BookHub.Server.Features.Authors.Mapper
{
    using AutoMapper;
    using Data.Models;
    using Service.Models;
    using Web.Models;

    public class AuthorMapper : Profile
    {
        public AuthorMapper()
        {
            this.CreateMap<CreateAuthorWebModel, AuthorDetailsServiceModel>();

            this.CreateMap<AuthorDetailsServiceModel, Author>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
