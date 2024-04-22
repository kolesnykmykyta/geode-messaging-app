using Application.Dtos;
using DataAccess.Entities;
using MediatR;

namespace Application.Services.Users
{
    public class GetUsersListQuery : IRequest<IEnumerable<UserInfoDto>>
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public string? SearchParam { get; set; }

        public string? SortProp { get; set; }

        public bool SortByDescending { get; set; }

        public string? SelectProps { get; set; }
    }
}
