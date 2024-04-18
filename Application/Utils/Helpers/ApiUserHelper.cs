using Application.Utils.Helpers.Interfaces;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils.Helpers
{
    public class ApiUserHelper : IApiUserHelper
    {
        public string ExtractIdFromUser(ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier)!;
        }

        public string ExtractNameFromUser(ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Name)!;
        }
    }
}
