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
            this.CreateMap<Book, BookServiceModel>();

            this.CreateMap<Genre, GenreDetailsServiceModel>()
                .ForMember(
                    dest => dest.TopBooks,
                    opt => opt.MapFrom(
                        src => src
                            .BooksGenres
                            .Select(bg => bg.Book)
                            .OrderByDescending(b => b.AverageRating)
                            .Take(3)));

            this.CreateMap<Genre, GenreNameServiceModel>();
        }
    }
}
