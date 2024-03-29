using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Geode.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        public UserController()
        {

        }

        public IActionResult UpdateUsername()
        {
            throw new NotImplementedException();
        }
    }
}
