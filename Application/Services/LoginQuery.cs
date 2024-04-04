using Auth.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class LoginQuery : IRequest<string?>
    {
        public LoginDto? Dto { get; set; }
    }
}
