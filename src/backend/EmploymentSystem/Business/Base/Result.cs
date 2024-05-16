using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Business.Base
{
    public class Result<T> : IResult<T>
    {
        public T? Payload { get; private set; }
        public bool Success { get; private set; }
        public string Message { get; private set; }
        public List<IError> Errors { get; private set; }

        public Result()
        {
            Success = true;
            Message = string.Empty;
            Errors = new List<IError>();
        }


        public Result<T> Succeeded(string message = "Your request was successful.")
        {
            Payload = default(T);
            Message = message;
            Success = true;
            return this;
        }
        public Result<T> SucceededWithPayload(T payload, string message = "")
        {
            Payload = payload;
            Message = message;
            Success = true;
            return this;
        }

        public Result<T> Failed(string message = "")
        {
            Message = message;
            Success = false;
            return this;
        }
        public Result<T> FailedWithError(IError error, string message = "")
        {
            Errors.Add(error);
            Message = message;
            Success = false;
            return this;
        }
        public Result<T> FailedWithErrors(object errorCode, List<string> errors, string message = "")
        {
            foreach (var error in errors)
            {
                Errors.Add(new Error(errorCode, error));
            }
            Message = message;
            Success = false;
            return this;
        }
    }

    public class Result : Result<object>, IResult
    {

    }
}
