using Auth.Dtos;
using Auth.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Geode.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;

        public UserController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            RegisterResultDto result = await _authService.RegisterAsync(dto);

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
            string? token = await _authService.LoginAsync(dto);

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
