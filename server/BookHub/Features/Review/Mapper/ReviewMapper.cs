namespace BookHub.Features.Review.Mapper
{
    using AutoMapper;
    using Data.Models;
    using Service.Models;
    using Web.Models;

    public class ReviewMapper : Profile
    {
        public ReviewMapper()
        {
            this.CreateMap<CreateReviewWebModel, CreateReviewServiceModel>();

            this.CreateMap<CreateReviewServiceModel, Review>();

            this.CreateMap<Review, ReviewServiceModel>()
                .ForMember(
                    dest => dest.Upvotes, 
                    opt => opt.MapFrom(
                        src => src.Votes.Where(v => v.IsUpvote).Count()))
                .ForMember(
                    dest => dest.Downvotes,
                    opt => opt.MapFrom(
                        src => src.Votes.Where(v => !v.IsUpvote).Count()))
                .ForMember(
                    dest => dest.ModifiedOn,
                    opt => opt.MapFrom(
                        src => src.ModifiedOn == null ? null : src.ModifiedOn.ToString()));
        }
    }
}
