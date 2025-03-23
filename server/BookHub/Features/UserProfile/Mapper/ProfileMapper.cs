namespace BookHub.Features.UserProfile.Mapper
{
    using AutoMapper;
    using Data.Models;
    using Service.Models;
    using Web.Models;

    public class ProfileMapper : Profile
    {
        public ProfileMapper()
        {
            this.CreateMap<UserProfile, ProfileServiceModel>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.UserId));

            this.CreateMap<CreateProfileWebModel, CreateProfileServiceModel>();

            this.CreateMap<CreateProfileServiceModel, UserProfile>();

            this.CreateMap<ProfileServiceModel, PrivateProfileServiceModel>();
        }
    }
}
