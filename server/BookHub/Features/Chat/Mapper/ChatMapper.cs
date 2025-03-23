namespace BookHub.Features.Chat.Mapper
{
    using AutoMapper;
    using Data.Models;
    using Service.Models;
    using UserProfile.Data.Models;
    using UserProfile.Service.Models;
    using Web.Models;

    public class ChatMapper : Profile
    {
        public ChatMapper()
        {
            this.CreateMap<CreateChatServiceModel, Chat>();

            this.CreateMap<Chat, ChatServiceModel>();

            this.CreateMap<UserProfile, PrivateProfileServiceModel>()
                .ForMember(
                    dest => dest.Id, 
                    opt => opt.MapFrom(src => src.UserId));

            this.CreateMap<ChatMessage, ChatMessageServiceModel>()
                .ForMember(
                    dest => dest.SenderImageUrl,
                    opt => opt.MapFrom(src => src.Sender.Profile!.ImageUrl))
                .ForMember(
                    dest => dest.SenderName,
                    opt => opt.MapFrom(src => src.Sender.Profile!.FirstName + " " + src.Sender.Profile.LastName));

            this.CreateMap<Chat, ChatDetailsServiceModel>()
                .ForMember(
                    dest => dest.Messages, 
                    opt => opt.MapFrom(src => src.Messages))
                .ForMember(
                    dest => dest.Participants,
                    opt => opt.MapFrom(src => src
                        .ChatsUsers
                        .Where(cu => cu.HasAccepted)
                        .Select(cu => cu.User.Profile)));

            this.CreateMap<CreateChatMessageServiceModel, ChatMessage>();

            this.CreateMap<CreateChatWebModel, CreateChatServiceModel>();

            this.CreateMap<CreateChatMessageWebModel, CreateChatMessageServiceModel>();
        }
    }
}
