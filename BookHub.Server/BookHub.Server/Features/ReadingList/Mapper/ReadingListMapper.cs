namespace BookHub.Server.Features.ReadingList.Mapper
{
    using AutoMapper;
    using Book.Service.Models;
    using Data.Models;

    public class ReadingListMapper : Profile
    {
        public ReadingListMapper()
        {
            this.CreateMap<Book, BookServiceModel>();

            this.CreateMap<ReadingList, BookServiceModel>();
        }
    }
}
