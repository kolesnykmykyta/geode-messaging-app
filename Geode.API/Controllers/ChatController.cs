using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Geode.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        public ChatController()
        {

        }

        [Authorize]
        [HttpGet("all")]
        public IActionResult GetUserChat()
        {
            string userId = User.FindFirst(ClaimTypes.Email)!.Value;
            return Ok(userId);
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
