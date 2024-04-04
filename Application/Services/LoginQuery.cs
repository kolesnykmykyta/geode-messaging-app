using Auth.Dtos;
using MediatR;

namespace Application.Services
{
    public class LoginQuery : IRequest<string?>
    {
        public LoginDto? Dto { get; set; }
    }
}
