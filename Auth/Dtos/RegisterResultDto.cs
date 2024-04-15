using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Auth.Dtos
{
    public class RegisterResultDto
    {
        public RegisterResultDto(bool IsSuccess, IEnumerable<string>? Errors = null)
        {
            this.IsSuccess = IsSuccess;
            this.Errors = Errors;
        }

        [JsonPropertyName("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonPropertyName("errors")]
        public IEnumerable<string>? Errors { get; set; }
    }
}
