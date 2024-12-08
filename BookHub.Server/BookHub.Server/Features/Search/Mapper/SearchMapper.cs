namespace BookHub.Server.Features.Search.Mapper
{
    using AutoMapper;
    using BookHub.Server.Features.Article.Data.Models;
    using BookHub.Server.Features.Authors.Data.Models;
    using BookHub.Server.Features.Book.Data.Models;
    using BookHub.Server.Features.Chat.Data.Models;
    using BookHub.Server.Features.Genre.Data.Models;
    using BookHub.Server.Features.UserProfile.Data.Models;
    using Data.Models;
    using Genre.Service.Models;
    using Service.Models;

    public class SearchMapper : Profile
    {
        public SearchMapper()
        {
            this.CreateMap<Genre, GenreNameServiceModel>();

            this.CreateMap<Book, SearchBookServiceModel>()
                 .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.BooksGenres.Select(bg => bg.Genre)))
                 .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author == null ? null : src.Author.Name));

            this.CreateMap<Article, SearchArticleServiceModel>();

            this.CreateMap<Author, SearchAuthorServiceModel>();

            this.CreateMap<UserProfile, SearchProfileServiceModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId));

            this.CreateMap<Chat, SearchChatServiceModel>();
        }
    }
}
