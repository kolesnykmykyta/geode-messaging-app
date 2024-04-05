using Auth.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Services.Interfaces
{
    public interface IAuthService
    {
        Task<RegisterResultDto> RegisterAsync(RegisterDto dto);

        Task<TokenDto?> LoginAsync(LoginDto dto);

        Task<TokenDto?> RefreshAsync(TokenDto dto);
    }
}
