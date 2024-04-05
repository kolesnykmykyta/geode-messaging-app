using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class User : IdentityUser
    {
        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpirationDate { get; set; }
    }
}
