using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Business.Base
{
    public interface IResult
    {
        public bool Success { get; }
        public string Message { get; }
        public List<IError> Errors { get; }

    }

    public interface IResult<T> : IResult
    {
        public T? Payload { get; }
    }
}
