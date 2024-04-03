using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public bool IsSuccess { get; set; }

        public IEnumerable<string>? Errors { get; set; }
    }
}
