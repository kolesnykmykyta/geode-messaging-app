using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Geode.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController
    {
        public ChatController()
        {

        }

        public IActionResult GetUserChat()
        {
            throw new NotImplementedException();
        }

        public IActionResult JoinChat()
        {
            throw new NotImplementedException();
        }

        public IActionResult CreateChat()
        {
            throw new NotImplementedException();
        }

        public IActionResult ChangeChatName()
        {
            throw new NotImplementedException();
        }

        public IActionResult LeaveChat()
        {
            throw new NotImplementedException();
        }

        public IActionResult SendMessageInChat()
        {
            throw new NotImplementedException();
        }
    }
}
