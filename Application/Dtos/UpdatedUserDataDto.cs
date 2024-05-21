using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class UpdatedUserDataDto
    {
        [JsonPropertyName("userName")]
        public string UserName { get; set; } = string.Empty;

        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
