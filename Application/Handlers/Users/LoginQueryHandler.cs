﻿using Application.Services.Users;
using Auth.Dtos;
using Auth.Services.Interfaces;
using MediatR;

namespace Application.Handlers.Users
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, TokenDto?>
    {
        private readonly IAuthService _authService;

        public LoginQueryHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public Task<TokenDto?> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            return _authService.LoginAsync(request.Dto!);
        }
    }
}
