using Application.Dtos;
using DataAccess.Entities;
using MediatR;

namespace Application.Services
{
    public class GetUsersListQuery : IRequest<IEnumerable<UserInfoDto>>
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public string? UsernameContains { get; set; }

        public string? EmailContains { get; set; }
    }
}
