using Application.Dtos;
using Application.Services.Chats;
using Application.Services.Messages;
using Application.Services.Users;
using AutoMapper;
using DataAccess.Entities;

namespace Application.Utils.Automapper
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            // CQRS Messages
            CreateMap<FilterDto, GetUsersListQuery>().ReverseMap();
            CreateMap<FilterDto, GetUserChatsQuery>().ReverseMap();
            CreateMap<FilterDto, GetUserMessagesQuery>().ReverseMap();
            CreateMap<CreateChatCommand, Chat>().ReverseMap();
            CreateMap<CreateChatCommand, ChatDto>().ReverseMap();
            CreateMap<ChangeChatNameCommand, Chat>().ReverseMap();
            CreateMap<ChangeChatNameCommand, ChatDto>().ReverseMap();

            // Entities to Dtos
            CreateMap<UserInfoDto, User>().ReverseMap();
            CreateMap<ChatDto, Chat>().ReverseMap();
            CreateMap<MessageDto, Message>().ReverseMap();
        }
    }
}
