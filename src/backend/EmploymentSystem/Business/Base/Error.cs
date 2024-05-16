using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Base
{
    public class Error : IError
    {
        public string Code { get; set; }
        public string Message { get; set; }

        public Error()
        {
            Code = string.Empty;
            Message = string.Empty;
        }

        public Error(object code, string message)
        {
            Code = code.ToString() ?? "";
            Message = message;
        }
    }

}
