using Application.Dtos;
using Application.Services.Chats;
using Application.Utils.Helpers.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Geode.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IApiUserHelper _userHelper;

        public ChatController(IMediator mediator, IMapper mapper, IApiUserHelper userHelper)
        {
            _mediator = mediator;
            _mapper = mapper;
            _userHelper = userHelper;
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetUserChats([FromQuery] FilterDto dto)
        {
            GetUserChatsQuery query = _mapper.Map<GetUserChatsQuery>(dto);
            query.UserId = _userHelper.ExtractIdFromUser(User);

            IEnumerable<ChatDto> chatsList = await _mediator.Send(query);
            return Ok(chatsList);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateNewChat(ChatDto dto)
        {
            CreateChatCommand command = _mapper.Map<CreateChatCommand>(dto);
            command.ChatOwnerId = _userHelper.ExtractIdFromUser(User);

            await _mediator.Send(command);
            return Ok();
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateChat(ChatDto dto)
        {

            ChangeChatNameCommand command = _mapper.Map<ChangeChatNameCommand>(dto);
            command.ChatOwnerId = _userHelper.ExtractIdFromUser(User);

            bool result = await _mediator.Send(command);
            return result ? Ok() : BadRequest();
        }

        [Authorize]
        [HttpPost("join/{chatId}")]
        public async Task<IActionResult> JoinChat(int chatId)
        {
            JoinChatCommand command = new()
            {
                ChatId = chatId,
                UserId = _userHelper.ExtractIdFromUser(User),
            };

            bool result = await _mediator.Send(command);
            return result ? Ok() : BadRequest();
        }

        [Authorize]
        [HttpPost("leave/{chatId}")]
        public async Task<IActionResult> LeaveChat(int chatId)
        {
            LeaveChatCommand command = new()
            {
                ChatId = chatId,
                UserId = _userHelper.ExtractIdFromUser(User)
            };

            bool result = await _mediator.Send(command);
            return result ? Ok() : BadRequest();
        }
    }
}
