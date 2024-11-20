namespace BookHub.Server.Features.Review.Mapper
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
        }
    }
}
