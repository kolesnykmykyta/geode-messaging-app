    using Application.Dtos;
using Application.Services;
using AutoMapper;
using DataAccess.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity.Data;
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

        public ChatController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetUserChats([FromQuery] FilterDto dto)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            GetUserChatsQuery query = _mapper.Map<GetUserChatsQuery>(dto);
            query.OwnerId = userId;

            IEnumerable<ChatDto> chatsList = await _mediator.Send(query);
            return Ok(chatsList);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateNewChat(ChatDto dto)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            CreateChatCommand command = _mapper.Map<CreateChatCommand>(dto);

            command.ChatOwnerId = userId;
            bool result = await _mediator.Send(command);

            return result ? Ok() : BadRequest();
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateChat(ChatDto dto)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            ChangeChatNameCommand command = _mapper.Map<ChangeChatNameCommand>(dto);
            command.ChatOwnerId = userId;

            bool result = await _mediator.Send(command);
            return result ? Ok() : BadRequest();
        }

        [Authorize]
        [HttpPost("join/{chatId}")]
        public async Task<IActionResult> JoinChat(int chatId)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            JoinChatCommand command = new() { ChatId = chatId, UserId = userId};

            bool result = await _mediator.Send(command);
            return result ? Ok() : BadRequest();
        }

        [Authorize]
        [HttpPost("leave/{chatId}")]
        public async Task<IActionResult> LeaveChat(int chatId)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            LeaveChatCommand command = new() { ChatId = chatId, UserId = userId };

            bool result = await _mediator.Send(command);
            return result ? Ok() : BadRequest();
        }
    }
}
