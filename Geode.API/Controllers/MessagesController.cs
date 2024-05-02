using Application.Dtos;
using Application.Services.Messages;
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
    public class MessagesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IApiUserHelper _userHelper;

        public MessagesController(IMediator mediator, IMapper mapper, IApiUserHelper userHelper)
        {
            _mediator = mediator;
            _mapper = mapper;
            _userHelper = userHelper;
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUserMessages([FromQuery] FilterDto dto)
        {
            GetUserMessagesQuery query = _mapper.Map<GetUserMessagesQuery>(dto);
            query.SenderId = _userHelper.ExtractIdFromUser(User);

            IEnumerable<MessageDto> allMessages = await _mediator.Send(query);
            return Ok(allMessages);
        }

        [Authorize]
        [HttpGet("chat/{chatId}")]
        public async Task<IActionResult> GetAllMessagesInChat(int chatId)
        {
            GetChatMessagesQuery query = new GetChatMessagesQuery(chatId);

            IEnumerable<ChatMessageDto>? messages = await _mediator.Send(query);
            return messages != null ? Ok(messages) : BadRequest();
        }
    }
}
