using Application.CqrsMessages.Users;
using Application.Dtos;
using Application.Services;
using Application.Services.Users;
using Application.Utils.Helpers.Interfaces;
using Auth.Dtos;
using AutoMapper;
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
        private readonly IApiUserHelper _userHelper;

        public UserController(IMediator mediator, IMapper mapper, IApiUserHelper userHelper)
        {
            _mediator = mediator;
            _mapper = mapper;
            _userHelper = userHelper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            RegisterResultDto result = await _mediator.Send(new RegisterNewUserCommand { Dto = dto });
            return result.IsSuccess ? Ok() : BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            TokenDto? tokens = await _mediator.Send(new LoginQuery { Dto = dto });
            return tokens != null ? Ok(tokens) : BadRequest();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(TokenDto dto)
        {
            TokenDto? tokens = await _mediator.Send(new RefreshTokenQuery { Dto = dto });
            return tokens != null ? Ok(tokens) : BadRequest();
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetUsersList([FromQuery] FilterDto dto)
        {
            IEnumerable<UserInfoDto> usersList = await _mediator.Send(_mapper.Map<GetUsersListQuery>(dto));
            return Ok(usersList);
        }

        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateUserProfile(UserProfileDto dto)
        {
            UpdateUserDataCommand command = _mapper.Map<UpdateUserDataCommand>(dto);
            command.Id = _userHelper.ExtractIdFromUser(User);

            bool isSuccess = await _mediator.Send(command);

            return isSuccess ? Ok() : BadRequest();
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            GetUserProfileQuery query = new GetUserProfileQuery(_userHelper.ExtractIdFromUser(User));

            UserProfileDto? result = await _mediator.Send(query);

            return result != null ? Ok(result) : BadRequest();
        }

        [Authorize]
        [HttpPost("profile/picture")]
        public async Task<IActionResult> UpdateUserProfilePicture(IFormFile picture)
        {
            using Stream pictureStream = picture.OpenReadStream();
            ChangeUserPictureCommand command = new ChangeUserPictureCommand()
            {
                PictureStream = pictureStream,
                UserId = _userHelper.ExtractIdFromUser(User),
            };

            await _mediator.Send(command);

            return Ok();
        }
    }
}
