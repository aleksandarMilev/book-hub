namespace BookHub.Features.ReadingList.Mapper
{
    using AutoMapper;
    using Book.Data.Models;
    using Book.Service.Models;
    using Data.Models;
    using Genre.Data.Models;
    using Genre.Service.Models;

    public class ReadingListMapper : Profile
    {
        public ReadingListMapper()
        {
            this.CreateMap<Genre, GenreNameServiceModel>();

            this.CreateMap<BookDbModel, BookServiceModel>();

            this.CreateMap<ReadingList, BookServiceModel>()
                .IncludeMembers(rl => rl.Book);
        }
    }
}
