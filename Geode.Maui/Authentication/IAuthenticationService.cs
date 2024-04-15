using Auth.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geode.Maui.Authentication
{
    public interface IAuthenticationService
    {
        Task LogInAsync(LoginDto dto);

        Task<IEnumerable<string>?> RegisterAsync(RegisterDto dto);

        Task LogoutAsync();
    }
}
