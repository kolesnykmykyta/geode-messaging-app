using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class UserInfoDto
    {
        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }
}
