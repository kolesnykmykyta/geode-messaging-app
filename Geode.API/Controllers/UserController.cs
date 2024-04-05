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
            TokenDto? tokens = await _mediator.Send(new LoginQuery { Dto = dto });

            if (tokens != null)
            {
                return Ok(tokens);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(TokenDto dto)
        {
            TokenDto? tokens = await _mediator.Send(new RefreshTokenQuery { Dto = dto });

            if (tokens != null)
            {
                return Ok(tokens);
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
