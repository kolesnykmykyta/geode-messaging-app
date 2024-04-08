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
            CreateMap<UserListFilterDto, GetUsersListQuery>().ReverseMap();

            // Entities to Dtos
            CreateMap<UserInfoDto, User>().ReverseMap();
        }
    }
}
