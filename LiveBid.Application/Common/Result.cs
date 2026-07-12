
using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Application.Common
{

    // The Result class represents the outcome of an operation, indicating success or failure.
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
    // The Result<T> class represents the outcome of an operation that returns a value of type T, indicating success or failure.
    public sealed class Result<T> : Result
    {
        private readonly T _value;
        // The constructor initializes the Result<T> instance with success status, error information, and the value of type T.
        internal Result(bool isSuccess, Error error, T value) : base(isSuccess, error)
        {
            _value = value;
        }
        // The Value property retrieves the value of type T if the operation was successful; otherwise, it throws an exception.
        public T Value
        {
            get
            {
                if (IsFailure)
                    throw new InvalidOperationException("Cannot access the value of a failed result.");
                return _value;
            }
        }
        // The Success method creates a successful Result<T> instance with the provided value.
        public static Result<T> Success(T value) => new Result<T>(true, Error.None, value);
        // The Failure method creates a failed Result<T> instance with the provided error information.
        public static Result<T> Failure(Error error) => new Result<T>(false, error, default!);

 

    }
}
