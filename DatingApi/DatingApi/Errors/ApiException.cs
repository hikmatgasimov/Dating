using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApi.Errors
{
    public class ApiException
    {
        public ApiException(int StatusCode, string Message=null, string Details = null)
        {
            this.StatusCode = StatusCode;
            this.Message = Message;
            this.Details = Details;
        }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
    }
}
