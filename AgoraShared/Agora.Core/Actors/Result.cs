namespace Agora.Core.Actors
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string Error { get; }
        public bool IsFailure => !IsSuccess;

        protected Result(bool isSuccess, string error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new Result(true, string.Empty);
        public static Result Failure(string error) => new Result(false, error);
    }

    public class Result<T> : Result
    {
        public T Value { get; }

        protected Result(bool isSuccess, string error, T value) : base(isSuccess, error)
        {
            Value = value;
        }

        public static Result<T> Success(T value) => new Result<T>(true, string.Empty, value);
        public new static Result<T> Failure(string error) => new Result<T>(false, error, default!);
        
        // This allows you to return "T" directly and have it auto-convert to Result<T>
        public static implicit operator Result<T>(T value) => Success(value);
    }
}