using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class ResponseBodyDto
    {
        public bool IsSuccess { get; set; }

        public string? Error { get; set; }

        public static ResponseBodyDto SuccessResponse()
        {
            return new ResponseBodyDto() { IsSuccess = true };
        }

        public static ResponseBodyDto FailureResponse()
        {
            return new ResponseBodyDto() { IsSuccess = false };
        }

        public static ResponseBodyDto FailureResponse(string error)
        {
            return new ResponseBodyDto() { IsSuccess = false, Error = error };
        }
    }
}
