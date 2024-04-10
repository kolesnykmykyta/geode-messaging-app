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

        [HttpPost("join/{id}")]
        public IActionResult JoinChat(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost("new")]
        public IActionResult CreateChat()
        {
            throw new NotImplementedException();
        }

        [HttpPost("changeName/{id}")]
        public IActionResult ChangeChatName(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost("leave/{id}")]
        public IActionResult LeaveChat(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost("{id}/send")]
        public IActionResult SendMessageInChat()
        {
            throw new NotImplementedException();
        }
    }
}
