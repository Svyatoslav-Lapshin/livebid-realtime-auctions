using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Application.Common
{
    public class Result
    {
        protected Result(bool isSuccess, Error error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get; }
        public Error Error { get; }
        public bool IsFailure => !IsSuccess;

        public static Result Success() => new Result(true, Error.None);

    }

    public sealed class Result<T> : Result
    {
        private readonly T _value;
        private Result(bool isSuccess, Error error, T value) : base(isSuccess, error)
        {
            _value = value;
        }
        public T Value
        {
            get
            {
                if (IsFailure)
                    throw new InvalidOperationException("Cannot access the value of a failed result.");
                return _value;
            }
        }
        public static Result<T> Success(T value) => new Result<T>(true, Error.None, value);
        public static Result<T> Failure(Error error) => new Result<T>(false, error, default!);
    }
}
