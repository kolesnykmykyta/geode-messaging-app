using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Geode.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        public UserController()
        {

        }

        [HttpPost("changename")]
        public IActionResult UpdateUsername()
        {
            throw new NotImplementedException();
        }
    }
}
