using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Geode.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        public ChatController()
        {

        }

        [HttpGet("all")]
        public IActionResult GetUserChat()
        {
            throw new NotImplementedException();
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
