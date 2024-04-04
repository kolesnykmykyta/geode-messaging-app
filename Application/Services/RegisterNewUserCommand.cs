using Auth.Dtos;
using MediatR;

namespace Application.Services
{
    public class RegisterNewUserCommand : IRequest<RegisterResultDto>
    {
        public RegisterDto? Dto { get; set; }
    }
}
