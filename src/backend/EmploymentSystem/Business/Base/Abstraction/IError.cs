using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Base
{
    public interface IError
    {
        public string Code { get; }
        public string Message { get; }
    }
}
