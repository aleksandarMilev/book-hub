namespace BookHub.Server.Features.ReadingList.Mapper
{
    using AutoMapper;
    using Book.Service.Models;
    using Data.Models;
    using Features.Book.Data.Models;
    using Features.Genre.Data.Models;
    using Genre.Service.Models;

    public class ReadingListMapper : Profile
    {
        public ReadingListMapper()
        {
            this.CreateMap<Genre, GenreNameServiceModel>();

            this.CreateMap<Book, BookServiceModel>();

            this.CreateMap<ReadingList, BookServiceModel>()
                .IncludeMembers(rl => rl.Book);
        }
    }
}
