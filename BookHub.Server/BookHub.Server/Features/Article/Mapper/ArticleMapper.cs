namespace BookHub.Server.Features.Article.Mapper
{
    using AutoMapper;
    using Data.Models;
    using Service.Models;
    using Web.Models;

    public class ArticleMapper : Profile
    {
        public ArticleMapper()
        {
            this.CreateMap<CreateArticleWebModel, CreateArticleServiceModel>();

            this.CreateMap<CreateArticleServiceModel, Article>();
        }
    }
}
