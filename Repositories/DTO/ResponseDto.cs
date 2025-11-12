using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTO
{
    public class ResponseDto<T>
    {
        public string Message { get; set; }
        public string StatusCode { get; set; }
        public T Data { get; set; }

        public ResponseDto(string message, string statusCode, T data)
        {
            Message = message;
            StatusCode = statusCode;
            Data = data;
        }
    }
}
