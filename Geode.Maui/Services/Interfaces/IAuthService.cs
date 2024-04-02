using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;

namespace Geode.Maui.Services.Interfaces
{
    internal interface IAuthService
    {
        void Register(RegisterDto registerDto);
        void LogIn(LoginDto loginDto);
        void LogOut();
    }
}
