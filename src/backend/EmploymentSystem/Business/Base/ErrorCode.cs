using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Base
{
    public enum ErrorCode
    {
        NoError = 0,

        NotFound = -1,
        Required = -2,
        EmptyOrNull = -3,
        LimitViolation = -4,

        InvalidData = -101,
        UnknownData = -102,

        NotAuthorized = -301,
        PolicyViolation = -302,

        AlreadyExists = -401,

        Exception = -501,
        Error = -502,

    }
}
