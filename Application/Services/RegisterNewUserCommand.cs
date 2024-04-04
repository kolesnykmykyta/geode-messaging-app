using Application.Services.Interfaces;
using Auth.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class RegisterNewUserCommand : IRequest<RegisterResultDto>
    {
        public RegisterDto? Dto { get; set; }
    }
}
