namespace BookHub.Server.Features.Chat.Mapper
{
    using AutoMapper;
    using Data.Models;
    using Service.Models;
    using UserProfile.Service.Models;
    using Web.Models;

    public class ChatMapper : Profile
    {
        public ChatMapper()
        {
            this.CreateMap<CreateChatServiceModel, Chat>();

            this.CreateMap<Chat, ChatServiceModel>();

            this.CreateMap<UserProfile, PrivateProfileServiceModel>();
            this.CreateMap<ChatMessage, ChatMessageServiceModel>();

            this.CreateMap<Chat, ChatDetailsServiceModel>()
                .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.ChatsUsers.Select(cu => cu.User.Profile)))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.Messages));

            this.CreateMap<CreateChatMessageServiceModel, ChatMessage>();

            this.CreateMap<CreateChatWebModel, CreateChatServiceModel>();

            this.CreateMap<CreateChatMessageWebModel, CreateChatMessageServiceModel>();
        }
    }
}
