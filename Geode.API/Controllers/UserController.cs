using Application.Dtos;
using Application.Services;
using Auth.Dtos;
using Auth.Services.Interfaces;
using AutoMapper;
using DataAccess.Entities;
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
        private readonly IMapper _mapper;

        public UserController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
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

        [HttpGet("all")]
        public async Task<IActionResult> GetUsersList([FromQuery] UserListFilterDto dto)
        {
            IEnumerable<UserInfoDto> usersList = await _mediator.Send(_mapper.Map<GetUsersListQuery>(dto));

            return Ok(usersList);
        }
    }
}
