using System.Security.Claims;

namespace Application.Utils.Helpers.Interfaces
{
    public interface IApiUserHelper
    {
        string ExtractIdFromUser(ClaimsPrincipal user);
    }
}
