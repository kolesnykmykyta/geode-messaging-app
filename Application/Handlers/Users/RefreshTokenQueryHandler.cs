using Application.Services.Users;
using Auth.Dtos;
using Auth.Services.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.Users
{
    public class RefreshTokenQueryHandler : IRequestHandler<RefreshTokenQuery, TokenDto?>
    {
        private readonly IAuthService _authService;

        public RefreshTokenQueryHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public Task<TokenDto?> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
        {
            return _authService.RefreshAsync(request.Dto!);
        }
    }
}
