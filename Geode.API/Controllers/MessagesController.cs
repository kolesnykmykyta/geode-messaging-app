using Application.Dtos;
using Application.Services;
using AutoMapper;
using MediatR;
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

        public MessagesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllUserMessages([FromQuery] FilterDto dto)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            GetUserMessagesQuery query = _mapper.Map<GetUserMessagesQuery>(dto);
            query.SenderId = userId;

            IEnumerable<MessageDto> allMessages = await _mediator.Send(query);
            return Ok(allMessages);
        }
    }
}
