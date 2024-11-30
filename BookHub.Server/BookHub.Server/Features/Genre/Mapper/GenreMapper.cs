namespace BookHub.Server.Features.Genre.Mapper
{
    using AutoMapper;
    using Book.Service.Models;
    using Data.Models;
    using Service.Models;

    public class GenreMapper : Profile
    {
        public GenreMapper()
        {
            CreateMap<Book, BookServiceModel>();

            CreateMap<Genre, GenreDetailsServiceModel>()
                .ForMember(
                    dest => dest.TopBooks,
                    opt => opt.MapFrom(
                        src => src
                            .BooksGenres
                            .Select(bg => bg.Book)
                            .OrderByDescending(b => b.AverageRating)));

            CreateMap<Genre, GenreNameServiceModel>();
        }
    }
}
