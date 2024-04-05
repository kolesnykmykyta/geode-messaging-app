using Auth.Dtos;
using MediatR;

namespace Application.Services
{
    public class LoginQuery : IRequest<TokenDto?>
    {
        public LoginDto? Dto { get; set; }
    }
}
