namespace BookHub.Features.Search.Mapper
{
    using Article.Data.Models;
    using Authors.Data.Models;
    using AutoMapper;
    using Book.Data.Models;
    using Chat.Data.Models;
    using Genre.Data.Models;
    using Genre.Service.Models;
    using Service.Models;
    using UserProfile.Data.Models;

    public class SearchMapper : Profile
    {
        public SearchMapper()
        {
            this.CreateMap<Genre, GenreNameServiceModel>();

            this.CreateMap<Book, SearchBookServiceModel>()
                 .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.BooksGenres.Select(bg => bg.Genre)))
                 .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author == null ? null : src.Author.Name));

            this.CreateMap<ArticleDbModel, SearchArticleServiceModel>();

            this.CreateMap<Author, SearchAuthorServiceModel>();

            this.CreateMap<UserProfile, SearchProfileServiceModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId));

            this.CreateMap<Chat, SearchChatServiceModel>();
        }
    }
}
