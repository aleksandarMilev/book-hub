namespace BookHub.Server.Features.Books.Mapper
{
    using Authors.Service.Models;
    using AutoMapper;
    using BookHub.Server.Features.Genre.Service.Models;
    using Data.Models;
    using Service.Models;
    using Web.Models;

    public class BookMapper : Profile
    {
        public BookMapper()
        {
            this.CreateMap<CreateBookWebModel, CreateBookServiceModel>();

            this.CreateMap<CreateBookServiceModel, Book>()
                .ForMember(dest => dest.PublishedDate, opt => opt.MapFrom(src => MapperHelper.ParseDateTime(src.PublishedDate)));
        }
    }
}
