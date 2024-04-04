using Application.Services;
using Auth.Dtos;
using Auth.Services.Interfaces;
using MediatR;

namespace Application.Handlers
{
    public class RegisterNewUserCommandHandler : IRequestHandler<RegisterNewUserCommand, RegisterResultDto>
    {
        private readonly IAuthService _authService;

        public RegisterNewUserCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public Task<RegisterResultDto> Handle(RegisterNewUserCommand request, CancellationToken cancellationToken)
        {
            return (_authService.RegisterAsync(request.Dto!));
        }
    }
}
