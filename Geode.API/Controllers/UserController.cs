using Application.Services;
using Auth.Dtos;
using Auth.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Geode.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            RegisterResultDto result = await _mediator.Send(new RegisterNewUserCommand { Dto = dto });

            if (result.IsSuccess)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            string? token = await _mediator.Send(new LoginQuery { Dto = dto });

            if (token != null)
            {
                return Ok(token);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("changename")]
        public IActionResult UpdateUsername()
        {
            throw new NotImplementedException();
        }
    }
}
