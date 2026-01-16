namespace _src.Code.Core.Actors
{
    public class HttpResponseResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
    
    public class HttpResponseResult<T> : HttpResponseResult
    {
        public T Data { get; set; }
    }
}