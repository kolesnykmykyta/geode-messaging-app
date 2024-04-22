using Auth.Dtos;
using MediatR;

namespace Application.Services.Users
{
    public class RegisterNewUserCommand : IRequest<RegisterResultDto>
    {
        public RegisterDto? Dto { get; set; }
    }
}
