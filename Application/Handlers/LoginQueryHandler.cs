using Application.Services;
using Auth.Services.Interfaces;
using MediatR;

namespace Application.Handlers
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, string?>
    {
        private readonly IAuthService _authService;

        public LoginQueryHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public Task<string?> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            return _authService.LoginAsync(request.Dto!);
        }
    }
}
