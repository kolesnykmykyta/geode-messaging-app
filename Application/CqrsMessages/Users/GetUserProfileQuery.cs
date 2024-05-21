using Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CqrsMessages.Users
{
    public class GetUserProfileQuery : IRequest<UserProfileDto>
    {
        public string UserId { get; set; } = string.Empty;

        public GetUserProfileQuery(string id)
        {
            UserId = id;
        }
    }
}
