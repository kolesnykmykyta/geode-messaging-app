using Application.Dtos;
using Application.Services;
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

            // Entities to Dtos
            CreateMap<UserInfoDto, User>().ReverseMap();
            CreateMap<ChatDto, Chat>().ReverseMap();
            CreateMap<MessageDto, Message>().ReverseMap();
        }
    }
}
